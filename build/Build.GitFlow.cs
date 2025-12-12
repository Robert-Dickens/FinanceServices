using System.IO;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Nuke.Components;
using Octokit;
using Serilog;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.GitVersion.GitVersionTasks;

partial class Build : IHazGitVersion, IHazGitRepository
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string ReleaseBranchPrefix = "release";
    const string HotfixBranchPrefix = "hotfix";
    const string BugBranchPrefix = "bugfix";
    const string FeaatureBranchPrefix = "feature";
    const string BetaBranchPrefix = "beta";
    const string SupportBranchPrefix = "support";


    [Parameter] readonly bool AutoStash = true;
    [Parameter] readonly bool Major;

    GitVersion GitVersion => From<IHazGitVersion>().Versioning;
    GitRepository GitRepository => From<IHazGitRepository>().GitRepository;


    string MajorMinorPatchVersion => Major ? $"{GitVersion.Major + 1}.0.0" : GitVersion.MajorMinorPatch;
    string MilestoneTitle => $"v{MajorMinorPatchVersion}";

    static string _semver = string.Empty;
    public string BuildVersion
    {
        get
        {
            if (string.IsNullOrEmpty(_semver))
            {
                _semver = GitRepository.IsOnMainOrMasterBranch() ? MajorMinorPatchVersion : GitVersion.SemVer;
            }
            return _semver;
        }
        set => _semver = value;
    }


    Target Milestone => _ => _
        .Unlisted()
        .OnlyWhenStatic(() => GitRepository.IsGitHubRepository() && (GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch()))
        .Executes(async () =>
        {
            var milestone = await GitRepository.GetGitHubMilestone(MilestoneTitle);
            if (milestone == null)
                return;

            Assert.True(milestone.OpenIssues == 0);
            Assert.True(milestone.ClosedIssues != 0);
            Assert.True(milestone.State == ItemState.Closed);
        });

    Target Changelog => _ => _
        .Unlisted()
        .DependsOn(Milestone)
        .OnlyWhenStatic(() => GitRepository.IsGitHubRepository() && (GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch()))
        .Executes(() =>
        {
            var changelogFile = From<IHazChangelog>().ChangelogFile;
            FinalizeChangelog(changelogFile, MajorMinorPatchVersion, GitRepository);
            Log.Information("Please review CHANGELOG.md and press any key to continue ...");
            //System.Console.ReadKey();

            Git($"add {changelogFile}");
            Git($"commit -m \"chore: {Path.GetFileName(changelogFile)} for {MajorMinorPatchVersion}\"");
        });

    Target Release => _ => _
        .DependsOn(Changelog)
        .Requires(() => !GitRepository.IsOnReleaseBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            if (!GitRepository.IsOnReleaseBranch())
                Checkout($"{ReleaseBranchPrefix}/{MajorMinorPatchVersion}", start: DevelopBranch);
            else
                FinishReleaseOrHotfix();
        });

    Target Hotfix => _ => _
        .DependsOn(Changelog)
        .Requires(() => !GitRepository.IsOnHotfixBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            var masterVersion = GitVersion(s => s
                .SetFramework("netcoreapp3.1")
                .SetUrl(RootDirectory)
                .SetBranch(MasterBranch)
                .EnableNoFetch()
                .DisableProcessOutputLogging()).Result;

            if (!GitRepository.IsOnHotfixBranch())
                Checkout($"{HotfixBranchPrefix}/{masterVersion.Major}.{masterVersion.Minor}.{masterVersion.Patch + 1}", start: MasterBranch);
            else
                FinishReleaseOrHotfix();
        });

    void FinishReleaseOrHotfix()
    {
        Git($"checkout {MasterBranch}");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");
        Git($"tag {MajorMinorPatchVersion}");

        Git($"checkout {DevelopBranch}");
        Git($"merge --no-ff --no-edit {GitRepository.Branch}");

        Git($"branch -D {GitRepository.Branch}");

        Git($"push origin {MasterBranch} {DevelopBranch} {MajorMinorPatchVersion}");
    }

    void CreateBranchTag(string? versionTag = null)
    {
        versionTag = versionTag ?? MajorMinorPatchVersion;
        Git($"tag {versionTag}");
        Git("push origin --tags");
    }

    void Checkout(string branch, string start)
    {
        var hasCleanWorkingCopy = GitHasCleanWorkingCopy();

        if (!hasCleanWorkingCopy && AutoStash)
            Git("stash");

        Git($"checkout -b {branch} {start}");

        if (!hasCleanWorkingCopy && AutoStash)
            Git("stash apply");
    }
}