using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlatformBuild.Internals
{
    internal static class SolutionExtension
    {
        internal static IEnumerable<(Nuke.Common.ProjectModel.Project Project, string Framework)> ToPackList(this Solution solution)
        {
            return from project in solution.ToProjectPackList()
                   from framework in project.GetTargetFrameworks("net8.0")
                   select (project, framework);
        }

        internal static IEnumerable<(Nuke.Common.ProjectModel.Project Project, string Framework)> ToBuildList(this Solution solution)
        {
            return from project in solution.ToProjectBuildList()
                   from framework in project.GetTargetFrameworks("net8.0")
                   select (project, framework);
        }

        internal static IEnumerable<Nuke.Common.ProjectModel.Project> ToProjectPackList(this Nuke.Common.ProjectModel.Solution solution)
        {
            foreach (var proj in solution.ToProjectBuildList())
            {
                if (proj.ProducesNugetPackage()) yield return proj;
            }
        }

        internal static IReadOnlyCollection<string> GetTargetFrameworks(this Nuke.Common.ProjectModel.Project project, string defaultTargetFramework)
        {
            var projectTargetFrameworks = project.GetTargetFrameworks();

            if (projectTargetFrameworks.Count == 0)
            {
                return new[] { defaultTargetFramework };
            }

            if (!projectTargetFrameworks.Any(x => string.Equals(x, defaultTargetFramework, StringComparison.InvariantCultureIgnoreCase)))
            {
                var l = projectTargetFrameworks.ToList();
                l.Add(defaultTargetFramework);
                return l.ToArray();
            }
            return projectTargetFrameworks;
        }

        internal static string GetRelativePath(this Nuke.Common.ProjectModel.Project project)
        {
            return project.Solution.Directory.GetRelativePathTo(project);
        }

        internal static IEnumerable<Nuke.Common.ProjectModel.Project> AllProjects(this Nuke.Common.ProjectModel.Solution solution)
        {
            foreach (var proj in solution.AllProjects)
            {
                if (proj.IsNukeBuildProject()) continue;

                yield return proj;
            }
        }

        internal static IEnumerable<Nuke.Common.ProjectModel.Project> ToProjectBuildList(this Nuke.Common.ProjectModel.Solution solution)
        {
            foreach (var proj in solution.AllProjects)
            {
                if (proj.IsTestdProject()) continue;
                if (proj.IsNukeBuildProject()) continue;

                yield return proj;
            }
        }

        internal static IEnumerable<Nuke.Common.ProjectModel.Project> GetTestProjects(this Nuke.Common.ProjectModel.Solution solution)
        {
            foreach (var proj in solution.AllProjects)
            {
                if (proj.IsTestdProject()) yield return proj;
            }
        }

        public static bool IsNukeBuildProject(this Nuke.Common.ProjectModel.Project project)
        {
            var nukeBaseDirectory = project.GetProperty("NukeBaseDirectory");

            if (!string.IsNullOrEmpty(nukeBaseDirectory))
            {
                var nukeProject = Nuke.Common.IO.AbsolutePath.Create(nukeBaseDirectory);
                var projectPath = project.Directory;
                if (nukeProject == projectPath)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ProducesNugetPackage(this Nuke.Common.ProjectModel.Project project)
        {
            var nukeBaseDirectory = project.GetProperty("IsPackable");

            if (!string.IsNullOrEmpty(nukeBaseDirectory) && bool.TryParse(nukeBaseDirectory, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool IsTestdProject(this Nuke.Common.ProjectModel.Project project)
        {
            var nukeBaseDirectory = project.GetProperty("IsTestProject");

            if (!string.IsNullOrEmpty(nukeBaseDirectory) && bool.TryParse(nukeBaseDirectory, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool IsGeneratePackageOnBuild(this Nuke.Common.ProjectModel.Project project)
        {
            var nukeBaseDirectory = project.GetProperty("GeneratePackageOnBuild");

            if (!string.IsNullOrEmpty(nukeBaseDirectory) && bool.TryParse(nukeBaseDirectory, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool IsGeneratePackageOnBuild(this Nuke.Common.ProjectModel.Solution solution)
        {
            var firstProject = solution.ToProjectBuildList().First();
            return firstProject.IsGeneratePackageOnBuild();
        }

        public static bool HasUseArtifactsOutput(this Nuke.Common.ProjectModel.Solution solution)
        {
            foreach (var proj in solution.AllProjects)
            {
                if (proj.HasUseArtifactsOutput()) return true;
            }
            return false;
        }

        public static bool HasUseArtifactsOutput(this Nuke.Common.ProjectModel.Project project)
        {
            var propUseArtifactsOutput = project.GetProperty("UseArtifactsOutput");

            if (!string.IsNullOrEmpty(propUseArtifactsOutput) && bool.TryParse(propUseArtifactsOutput, out var result))
            {
                return result;
            }

            return false;

        }

    }
}
