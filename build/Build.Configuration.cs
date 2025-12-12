using System.ComponentModel;
using Nuke.Common;
using Nuke.Common.Tooling;

partial class Build
{
    [Parameter("Ignore unreachable sources during Restore")]
    bool IgnoreFailedSources => ((INukeBuild)this).TryGetValue<bool?>(() => IgnoreFailedSources) ?? false;

    [Parameter("Sets the desired number of parrallel publish tasks ")]
    int PublishDegreeOfParallelism => ((INukeBuild)this).TryGetValue<int?>(() => PublishDegreeOfParallelism) ?? 10;

    [Parameter] readonly string BetaArtifactsFeed;
    [Parameter] readonly string ArtifactsFeed;

    internal const string NugetPackagePattern = "*.nupkg";
    internal const string NugetSymbolPackagePattern = "**/*.symbols.nupkg";
    internal const string NugetPackageSearchPattern = "**/" + NugetPackagePattern;
}


[TypeConverter(typeof(TypeConverter<BuildConfiguration>))]
public class BuildConfiguration : Enumeration
{
    public static BuildConfiguration Debug = new BuildConfiguration { Value = nameof(Debug) };
    public static BuildConfiguration Release = new BuildConfiguration { Value = nameof(Release) };

    public static implicit operator string(BuildConfiguration configuration)
    {
        return configuration.Value;
    }
}
