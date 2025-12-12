using Azure.Pipelines;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.AzurePipelines.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
using Serilog;
using System.Collections.Generic;
using System.Linq;

[AzurePipelines("qualitygate",
    AzurePipelinesImage.UbuntuLatest,
    AzurePipelinesImage.WindowsLatest,
    AzurePipelinesImage.MacOsLatest,
    InvokedTargets = new[] { nameof(ITest.Test) },
    NonEntryTargets = new[] { nameof(IRestore.Restore), nameof(ICompile.Compile), nameof(IReportCoverage.ReportCoverage), nameof(IReportIssues.ReportIssues), nameof(IReportDuplicates.ReportDuplicates) },
    ExcludedTargets = new[] { nameof(IPublish.Publish), nameof(IPack.Pack) },
    AutoGenerate = false,
    TriggerDisabled = true,
    PullRequestsDisabled = true,
    ImportVariableGroups = new[] { "GlobalVariablesLibrary" },
    SdkVersions = new[] { "8.0.404", "9.0.100" }
)]
[AzurePipelines(
    AzurePipelinesImage.UbuntuLatest,
    InvokedTargets = new[] { nameof(ITest.Test),  nameof(IPublish.Publish) },
    NonEntryTargets = new[] { nameof(IRestore.Restore), nameof(ICompile.Compile), nameof(IReportCoverage.ReportCoverage), nameof(IReportIssues.ReportIssues), nameof(IReportDuplicates.ReportDuplicates), nameof(IPack.Pack) },
    PullRequestsBranchesInclude = new[] { $"{ReleaseBranchPrefix}/*", $"{FeaatureBranchPrefix}/*", $"{SupportBranchPrefix}/*", $"{BetaBranchPrefix}/*", $"{BugBranchPrefix}/*", $"{HotfixBranchPrefix}/*" },
    PullRequestsPathsExclude = new[] { "docs/*", "Demo/*", "Templates/*", "Hosts/*", "Assets/*", "buildprops/*" },
    TriggerBranchesInclude = new[] { $"'*'" },
    TriggerBranchesExclude = new[] { $"{ReleaseBranchPrefix}/*", MasterBranch, DevelopBranch },
    TriggerPathsExclude = new[] { "docs/*", "Demo/*", "Templates/*", "Hosts/*", "Assets/*", "buildprops/*" },
    AutoGenerate = false,
    ImportVariableGroups = new[] { "GlobalVariablesLibrary" },
    SdkVersions = new[] { "8.0.404", "9.0.100" }

)]
partial class Build
{
    public class AzurePipelinesAttribute : Nuke.Common.CI.AzurePipelines.AzurePipelinesAttribute
    {
        public bool NuGetAuthenticate { get; set; } = true;
        public bool UseOnPremAgentPool { get; set; } = false;
        public string OnPremAgentPool { get; set; } = string.Empty;
        public string[]? Capabilities { get; set; } = System.Array.Empty<string>();
        public string[]? SdkVersions { get; set; } = System.Array.Empty<string>();

        public AzurePipelinesAttribute(string suffix, AzurePipelinesImage image, params AzurePipelinesImage[] images)
         : base(suffix, image, images)
        {
            FetchDepth = 0;
            EnableAccessToken = true;
        }

        public AzurePipelinesAttribute(AzurePipelinesImage image, params AzurePipelinesImage[] images)
            : base(image, images)
        {
            FetchDepth = 0;
            EnableAccessToken = true;
        }

        public override ConfigurationEntity GetConfiguration(IReadOnlyCollection<ExecutableTarget> relevantTargets)
        {
            if (TriggerBranchesInclude.Length > 0 || TriggerBranchesExclude.Length > 0 ||
                TriggerTagsInclude.Length > 0 || TriggerTagsExclude.Length > 0 ||
                TriggerPathsInclude.Length > 0 || TriggerPathsExclude.Length > 0)
            {
                TriggerBatch = true;
            }

            if (PullRequestsBranchesInclude.Length > 0 || PullRequestsBranchesExclude.Length > 0 ||
                PullRequestsPathsInclude.Length > 0 || PullRequestsPathsExclude.Length > 0)
            {
                TriggerBatch = true;
                PullRequestsAutoCancel = true;
            }
            return base.GetConfiguration(relevantTargets);
        }

        protected override IEnumerable<AzurePipelinesStep> GetSteps(ExecutableTarget executableTarget, IReadOnlyCollection<ExecutableTarget> relevantTargets, AzurePipelinesImage image)
        {
            if (NuGetAuthenticate)
            {
                yield return new AzurePipelinesNuGetAuthenticateStep();
            }

            if (SdkVersions?.Length > 0)
            {
                foreach (var version in SdkVersions)
                {
                    yield return new AzurePipelinesSdkInstallStep(version);
                }
            }

            foreach (var step in base.GetSteps(executableTarget, relevantTargets, image))
            {
                yield return step;
            }
        }

        protected override AzurePipelinesStage GetStage(AzurePipelinesImage image, IReadOnlyCollection<ExecutableTarget> relevantTargets)
        {
            Log.Information("The image is {image}", image.ToString());
            if (UseOnPremAgentPool)
            {
                var lookupTable = new LookupTable<ExecutableTarget, AzurePipelinesJob>();
                var jobs = relevantTargets
                    .Select(x => (ExecutableTarget: x, Job: GetJob(x, lookupTable, relevantTargets, image)))
                    .ForEachLazy(x => lookupTable.Add(x.ExecutableTarget, x.Job))
                    .Select(x => x.Job).ToArray();

                return new AzurePipelinesOnPremStage
                {
                    Name = image.GetValue().Replace("-", "_").Replace(".", "_"),
                    DisplayName = image.GetValue(),
                    Image = image,
                    Dependencies = new AzurePipelinesStage[0],
                    Jobs = jobs,
                    UseOnPremAgentPool = UseOnPremAgentPool,
                    PoolName = OnPremAgentPool,
                    Capabilities = Capabilities
                };
            }

            return base.GetStage(image, relevantTargets);
        }

    }
}

namespace Azure.Pipelines
{
    public class AzurePipelinesNuGetAuthenticateStep : AzurePipelinesStep
    {
        public override void Write(CustomFileWriter writer) => writer.WriteLine("- task: NuGetAuthenticate@1");
    }

    public class AzurePipelinesSdkInstallStep : AzurePipelinesStep
    {
        public string Version { get; }

        public AzurePipelinesSdkInstallStep(string version)
        {
            Version = version;
        }

        public override void Write(CustomFileWriter writer)
        {
            using (writer.WriteBlock("- task: UseDotNet@2"))
            {
                writer.WriteLine($"displayName: Installing SDK Version {Version}");
                using (writer.WriteBlock("inputs:"))
                {
                    writer.WriteLine("packageType: 'sdk'");
                    writer.WriteLine($"version: {Version.SingleQuote()}");
                }
            }
        }
    }

    public class AzurePipelinesOnPremStage : AzurePipelinesStage
    {
        public bool UseOnPremAgentPool { get; set; }
        public string[]? Capabilities { get; set; } = System.Array.Empty<string>();
        public string PoolName { get; set; } = string.Empty;
        public override void Write(CustomFileWriter writer)
        {
            using (writer.WriteBlock($"- stage: {Name}"))
            {
                writer.WriteLine($"displayName: {DisplayName.SingleQuote()}");
                writer.WriteLine($"dependsOn: [ {Dependencies.Select(x => x.Name).JoinCommaSpace()} ]");

                writer.WriteLine($"pool: {PoolName.SingleQuote()}");

                if (Capabilities?.Length > 0)
                {
                    writer.WriteLine("demands:");

                    foreach (var tag in Capabilities)
                    {
                        writer.WriteLine($"- {tag}");
                    }
                }

                using (writer.WriteBlock("jobs:"))
                {
                    Jobs.ForEach(x => x.Write(writer));
                }
            }
        }
    }
}