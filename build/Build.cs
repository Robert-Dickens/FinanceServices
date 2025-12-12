using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Components;
using Nuke.Common.Git;

[DotNetVerbosityMapping]
[ShutdownDotNetAfterServerBuild]
partial class Build : NukeBuild, INukeBuild, IHazArtifacts, IHazConfiguration
{

    int BuildPartition => 2;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    BuildConfiguration Configuration = IsLocalBuild ? BuildConfiguration.Debug : BuildConfiguration.Release;

    [CI] readonly AzurePipelines AzurePipeline;

    AbsolutePath BuildArtifactsDirectory => RootDirectory / "buildartifacts";
    AbsolutePath DocFxPath => RootDirectory / "docs";

    AbsolutePath AssetsDirectory => BuildArtifactsDirectory / "libs";
    AbsolutePath PublishDirectory => BuildArtifactsDirectory / "published";
    AbsolutePath BuildPackagesDirectory => BuildArtifactsDirectory / "package";

    AbsolutePath ArtifactsDirectory => BuildArtifactsDirectory / "artifacts";


    AbsolutePath IHazArtifacts.ArtifactsDirectory => ArtifactsDirectory;
    AbsolutePath PackagesDirectory => ArtifactsDirectory / "package";


    //public static int Main() => Execute<Build>(x => ((IPack)x).Pack);
    public static int Main() => Execute<Build>(x => x.CreateEfMigrations);

    protected override void OnBuildInitialized()
    {
        if (IsServerBuild)
        {
            if (!(IsPullRequest || GitRepository.IsOnMainOrMasterBranch()))
                Configuration = BuildConfiguration.Debug;
            AzurePipeline.UpdateBuildNumber(BuildVersion);
        }
    }

    protected override void OnBuildFinished()
    {
        if (IsServerBuild && Partition.Part == Partition.Total)
        {
            AzurePipeline.AddBuildTag(BuildVersion);

            //if (IsPullRequest || GitRepository.IsOnDevelopBranch() || GitRepository.IsOnMainOrMasterBranch())
            //{
            //    CreateBranchTag(BuildVersion);

            //}
        }

    }

    bool IsPullRequest => !string.IsNullOrEmpty(AzurePipeline?.PullRequestTargetBranch);

    T From<T>()
    where T : INukeBuild
    => (T)(object)this;

}
