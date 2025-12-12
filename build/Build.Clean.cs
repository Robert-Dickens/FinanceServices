using Nuke.Common;
using PlatformBuild.Internals;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;

partial class Build
{
    Target Clean => _ => _
    .Before<IRestore>(r => r.Restore)
    .OnlyWhenStatic(() => IsLocalBuild)
    .Executes(() =>
    {
        var dotnetCliArtifacts = BuildArtifactsDirectory;

        dotnetCliArtifacts.CreateOrCleanDirectory();

        foreach (var project in Solution.GetAllProjects("*"))
        {
            if (project.IsNukeBuildProject()) continue;
            project.Directory.GlobDirectories("bin", "obj", "node-modules").ForEach(d => d.DeleteDirectory());
        }
    });
}
