using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Components;
using Nuke.Common.Utilities;
using Nuke.Common.Tools.DotNet;

partial class Build : IRestore
{
    Target IRestore.Restore => _ => _
    .Inherit<IRestore>(r => r.Restore)
    .WhenSkipped(DependencyBehavior.Skip)
    .When(IsLocalBuild, _ => _
        .DependsOn(Clean));

    Configure<DotNetRestoreSettings> IRestore.RestoreSettings => _ => _
                                           .SetProjectFile(Solution)
                                            .SetVerbosity(DotNetVerbosity.minimal)
                                           .SetIgnoreFailedSources(IgnoreFailedSources);

}
