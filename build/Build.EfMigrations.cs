using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Utilities;
using System.Xml.Linq;
using System.Linq;
using Nuke.Common.Tools.EntityFramework;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using NuGet.Common;
using static Nuke.Common.Tools.EntityFramework.EntityFrameworkTasks;
using Nuke.Common.Tools.DotNet;
using static Serilog.Log;
using Nuke.Common.Tooling;

partial class Build
{
    Target CreateEfMigrations => _ => _
    .DependsOn(Clean)
    .OnlyWhenStatic(() => IsLocalBuild)
    .After(Clean)
    .Executes(() =>
    {

        var currentSetting = EnableDisableArtifactsOutput(false);

        var migrationName = GitVersion.PreReleaseLabel + GitVersion.CommitsSinceVersionSource;
        migrationName = migrationName.Replace("-", "").Replace(".", "");

        foreach (var project in Solution.GetAllProjects("*.Domain.Context"))
        {
            Information($"Building Project {project.Name}");
            DotNetBuild(new Nuke.Common.Tools.DotNet.DotNetBuildSettings().SetProjectFile(project).SetVerbosity(DotNetVerbosity.quiet));

            var projectPath = PathUtility.GetRelativePath(Solution, project.Directory);

            Information($"Checking if project {project.Name} already contains migrations");

            var existingMigrations = EntityFrameworkMigrationsList(new EntityFrameworkMigrationsListSettings()
                            .SetProcessWorkingDirectory(Solution.Directory)
                            .SetProject(projectPath)
                            .SetNoBuild(true));

            var hasMigrations = !existingMigrations.Any(x => string.Equals(x.Text, "No migrations were found.", System.StringComparison.OrdinalIgnoreCase));

            migrationName = hasMigrations ? migrationName : "Initial";

            Information($"Creating migration {migrationName} for project {project.Name}");
            EntityFrameworkMigrationsAdd(new EntityFrameworkMigrationsAddSettings()
                            .SetProcessWorkingDirectory(Solution.Directory)
                            .SetProject(projectPath)
                            .SetName(migrationName)
                            .SetNoBuild(true));

        }


        EnableDisableArtifactsOutput(currentSetting ?? true);

    });

    private bool? EnableDisableArtifactsOutput(bool turnOn)
    {
        var buildProps = RootDirectory.GetFiles("Directory.Build.props").SingleOrDefault();
        bool? currentSetting = null;
        if (buildProps.FileExists())
        {
            var propsXml = buildProps.ReadXml();

            bool updated = false;

            foreach (var group in propsXml.Descendants("PropertyGroup"))
            {
                group.Nodes().ForEach(node =>
                {
                    if (node is XElement element)
                    {
                        if (element.Name.LocalName == "UseArtifactsOutput")
                        {
                            currentSetting = string.Equals(element.Value, bool.TrueString, System.StringComparison.OrdinalIgnoreCase);
                            element.Value = turnOn.ToString().ToLower();
                            updated = true;
                        }
                    }
                });

                if (updated)
                {
                    break;
                }
            }

            if (updated)
            {
                buildProps.WriteXml(propsXml);
            }
        }
        return currentSetting;

    }
}
