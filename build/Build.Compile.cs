using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
using PlatformBuild.Internals;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Serilog.Log;

partial class Build : ICompile
{
    Target ICompile.Compile => _ => _
    .DependsOn<IRestore>(c => c.Restore)
    .DependsOn(LibManRestore)
    .WhenSkipped(DependencyBehavior.Skip)
    //.Executes(() => LibManRestore)
    .Executes(() =>
    {
        ReportSummary(_ => _
                .AddPair("Version", BuildVersion));

        bool requestTests = ExecutionPlan.Contains(((ITest)this).Test);
        bool requestPublish = (ExecutionPlan.Contains(((IPublish)this).Publish) || ExecutionPlan.Contains(((IPack)this).Pack));

        var buildList = requestPublish ? SolutionPackProjects() : SolutionProjects();


        DotNetBuild(_ => _
                .Apply(BuildCompileSettings)
                .CombineWith(buildList, (_, v) => _
                        .SetProjectFile(v))
                .When(requestTests && requestPublish, _ => _
                    .CombineWith(SolutionTestProjects(), (_, v) => _
                        .SetProjectFile(v)))
                , ((ITest)this).TestDegreeOfParallelism);


    }).Partition(BuildPartition);

    Configure<DotNetBuildSettings> BuildCompileSettings => _ => _
                                        .SetConfiguration(Configuration)
                                        .SetVersion(BuildVersion)
                                        .When(IsLocalBuild, _ => _
                                            .SetVerbosity(DotNetVerbosity.quiet))
                                        .When(IsServerBuild, _ => _
                                            .EnableContinuousIntegrationBuild()
                                            .SetVerbosity(DotNetVerbosity.quiet)
                                            .SetPackageProjectUrl(AzurePipeline.RepositoryUri))
                                        .When(!Solution.HasUseArtifactsOutput(), _ => _
                                            .SetOutputDirectory(AssetsDirectory))
                                        .SetNoRestore(SucceededTargets.Contains(((IRestore)this).Restore))
                                        .WhenNotNull(this as IHazGitRepository, (_, o) => _
                                            .SetRepositoryUrl(o.GitRepository.HttpsUrl))
                                        .WhenNotNull(this as IHazGitVersion, (_, o) => _
                                            .SetAssemblyVersion(o.Versioning.MajorMinorPatch)
                                            .SetFileVersion(o.Versioning.AssemblySemFileVer)
                                            .SetInformationalVersion(o.Versioning.InformationalVersion))
                                        .WhenNotNull(this as IHazNerdbankGitVersioning, (_, o) => _
                                            .SetAssemblyVersion(o.Versioning.AssemblyVersion)
                                            .SetFileVersion(o.Versioning.AssemblyFileVersion)
                                            .SetInformationalVersion(o.Versioning.AssemblyInformationalVersion));


    Configure<DotNetBuildSettings> ICompile.CompileSettings => BuildCompileSettings;

}
