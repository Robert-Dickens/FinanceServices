using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.IO;
using System.Collections.Generic;
using Nuke.Common.Utilities;
using Nuke.Components;
using PlatformBuild.Internals;
using Nuke.Common.ProjectModel;
using System;
using Nuke.Common.Git;
using System.Linq;

partial class Build : IPublish
{
    bool PushCompleteOnFailure => true;
    int PushDegreeOfParallelism => 5;


    Target IPublish.Publish => _ => _
        .DependsOn<IPack>(p => p.Pack)
        .WhenSkipped(DependencyBehavior.Execute)
        .Executes(() =>
        {
            DotNetNuGetPush(_ => _
                        .Apply(((IPublish)this).PushSettings)
                        .CombineWith(((IPublish)this).PushPackageFiles, (_, v) => _
                            .SetTargetPath(v)),
                    PushDegreeOfParallelism,
                    PushCompleteOnFailure);

        });

    string DiscoverNugetFeedPath()
    {
        if (IsServerBuild)
        {
            if (IsPullRequest)
            {
                if (string.Equals(AzurePipeline.PullRequestTargetBranch, MasterBranch, System.StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(AzurePipeline.PullRequestTargetBranch, "main", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    return ArtifactsFeed ?? BetaArtifactsFeed;
                }
            }
            else
            {
                if (GitRepository.IsOnMainOrMasterBranch())
                {
                    return ArtifactsFeed ?? BetaArtifactsFeed;
                }
            }
            return BetaArtifactsFeed ?? ArtifactsFeed;
        }
        //var currentUserMyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var localNugetsFeed = @"C:\Development\Packages";// System.IO.Path.Combine(currentUserMyDocumentsPath, "NuGet", "LocalNuGetFeed");
        if (!System.IO.Directory.Exists(localNugetsFeed))
        {
            System.IO.Directory.CreateDirectory(localNugetsFeed);
        }
        return localNugetsFeed;
    }

    Configure<DotNetNuGetPushSettings> IPublish.PushSettings => _ => _
                        .SetSource(DiscoverNugetFeedPath())
                        .SetApiKey("az");

    IEnumerable<AbsolutePath> IPublish.PushPackageFiles
    {
        get
        {
            var symbolPackages = From<IPack>().PackagesDirectory.GlobFiles(NugetSymbolPackagePattern).ToList();
            if (symbolPackages.Count > 0)
            {
                return symbolPackages;
            }
            return From<IPack>().PackagesDirectory.GlobFiles(NugetPackagePattern);
        }
    }

    Configure<DotNetPublishSettings> PublishSettings => _ => _
                            .SetConfiguration(Configuration)
                            .When(SucceededTargets.Contains(((ICompile)this).Compile), _ => _
                                .EnableNoBuild()
                                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                                .SetFileVersion(GitVersion.AssemblySemFileVer)
                                .SetInformationalVersion(GitVersion.InformationalVersion))
                            .EnableNoLogo()
                            .SetVerbosity(DotNetVerbosity.quiet)
                            .SetOutput(PublishDirectory)
                            .SetVersion(BuildVersion)
                            .When(IsServerBuild, _ => _
                                .EnableContinuousIntegrationBuild()
                                .SetPackageProjectUrl(AzurePipeline.RepositoryUri))
                            .WhenNotNull(this as IHazGitRepository, (_, o) => _
                                .SetRepositoryUrl(o.GitRepository.HttpsUrl));


    IEnumerable<(Project Project, string Framework)> PublishConfigurations
                                 => Solution.ToPackList();

}
