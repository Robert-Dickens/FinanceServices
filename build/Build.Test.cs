using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
using System.Collections.Generic;
using static Nuke.Common.Tools.ReSharper.ReSharperTasks;

partial class Build : IReportCoverage, IReportIssues, IReportDuplicates, ITest
{
    [Parameter] readonly string TestFilters;

    bool IReportCoverage.CreateCoverageHtmlReport => true;
    bool IReportCoverage.ReportToCodecov => false;

    IEnumerable<(string PackageId, string Version)> IReportIssues.InspectCodePlugins
        => new (string PackageId, string Version)[]
           {
               new("ReSharperPlugin.CognitiveComplexity", ReSharperPluginLatest)
           };

    bool IReportIssues.InspectCodeFailOnWarning => false;
    bool IReportIssues.InspectCodeReportWarnings => true;
    IEnumerable<string> IReportIssues.InspectCodeFailOnIssues => new string[0];
    IEnumerable<string> IReportIssues.InspectCodeFailOnCategories => new string[0];

    public IEnumerable<Project> TestProjects => SolutionTestProjects();


    AbsolutePath ITest.TestResultDirectory => ArtifactsDirectory / "test-results";
    AbsolutePath IReportCoverage.CoverageReportDirectory => ArtifactsDirectory / "coverage-report";

    Configure<ReportGeneratorSettings> IReportCoverage.ReportGeneratorSettings => _ => _
            .SetReportTypes(ReportTypes.Cobertura, ReportTypes.HtmlInline_AzurePipelines)
            .SetReports(((ITest)this).TestResultDirectory / "**/*.xml");

    Configure<DotNetTestSettings, Project> ITest.TestProjectSettings => (_, v) => _
                                    .SetConfiguration(Configuration);

    Configure<DotNetTestSettings> ITest.TestSettings => _ => _
                         .When(!string.IsNullOrEmpty(TestFilters), _ => _
                              .SetFilter(TestFilters))
                        .When(InvokedTargets.Contains((this as IReportCoverage)?.ReportCoverage) || IsServerBuild, _ => _
                            .SetDataCollector("XPlat Code Coverage"));

    void IReportCoverage.UploadCoverageData()
    {
        ((ITest)this).TestResultDirectory.GlobFiles("**/*.xml").ForEach(delegate (AbsolutePath x)
        {
            AzurePipelines.Instance?.PublishCodeCoverage(AzurePipelinesCodeCoverageToolType.Cobertura, x, ((IReportCoverage)this).CoverageReportDirectory);
        });
    }


    Target ITest.Test => _ => _
    .Inherit<ITest>()
    .WhenSkipped(DependencyBehavior.Skip)
    .Partition(BuildPartition);

}
