using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.GitVersion;
using System.Linq;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Utilities;

partial class Build : IPack
{
    Target IPack.Pack => _ => _
        .DependsOn<ICompile>(c => c.Compile)
        .DependsOn(LibManRestore)
        .WhenSkipped(DependencyBehavior.Execute)
        .Produces(PackagesDirectory / NugetPackagePattern)
        .Executes(() =>
        {
            var eistingAssets = BuildPackagesDirectory.GlobFiles(NugetPackageSearchPattern, NugetSymbolPackagePattern);

            if (eistingAssets.Any())
            {
                Logger.Info("Copying Existing Assets From Compile To Packages Directory");
                eistingAssets.ForEach(x => x.CopyToDirectory(PackagesDirectory, ExistsPolicy.FileOverwrite));
            }
            else
            {
                Logger.Info("Calling DotNet Pack");

                DotNetPack(_ => _
                            .Apply(((IPack)this).PackSettings)
                            .CombineWith(SolutionPackProjects(), (_, v) => _
                                    .SetProject(v))
                            );

                BuildPackagesDirectory.GlobFiles(NugetPackageSearchPattern, NugetSymbolPackagePattern).ForEach(x => x.CopyToDirectory(PackagesDirectory, ExistsPolicy.FileOverwrite));
            }

            ReportSummary(_ => _
                .AddPair("Packages", PackagesDirectory.GlobFiles(NugetPackagePattern).Count.ToString()));
        });

    AbsolutePath IPack.PackagesDirectory => PackagesDirectory;


    Configure<DotNetPackSettings> IPack.PackSettings => _ => _
                        .SetConfiguration(Configuration)
                        .SetNoDependencies(true)
                        .SetNoBuild(SucceededTargets.Contains(((ICompile)this).Compile))
                        .SetIncludeSymbols(Configuration == nameof(Configuration.Debug))
                        .SetOutputDirectory(BuildPackagesDirectory)
                        .SetVersion(BuildVersion)
                        .WhenNotNull(this as IHazChangelog, (_, o) => _
                            .SetPackageReleaseNotes(o.NuGetReleaseNotes))
                         .WhenNotNull(this as IHazGitRepository, (_, o) => _
                              .SetRepositoryUrl(o.GitRepository.HttpsUrl))
                         .WhenNotNull(this as IHazGitVersion, (_, o) => _
                              .SetAssemblyVersion(o.Versioning.MajorMinorPatch)
                              .SetFileVersion(o.Versioning.AssemblySemFileVer)
                              .SetInformationalVersion(o.Versioning.InformationalVersion))
                         .WhenNotNull(this as IHazNerdbankGitVersioning, (_, o) => _
                               .SetAssemblyVersion(o.Versioning.AssemblyVersion)
                               .SetFileVersion(o.Versioning.AssemblyFileVersion)
                               .SetInformationalVersion(o.Versioning.AssemblyInformationalVersion)
                         .When(IsServerBuild, _ => _
                              .SetPackageProjectUrl(AzurePipeline.RepositoryUri)
                              .EnableContinuousIntegrationBuild())
                         );


}
