using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Components;
using PlatformBuild.Internals;
using System.Collections.Generic;

partial class Build : IHazSolution
{

    [Solution(GenerateProjects = true)] readonly Solution Solution;
    Nuke.Common.ProjectModel.Solution IHazSolution.Solution => Solution;

    private List<Project>? _projectCache = null;

    IEnumerable<Nuke.Common.ProjectModel.Project> SolutionProjects()
    {
        if (_projectCache == null)
        {
            _projectCache = new List<Project>();
            var allSolution = Solution.GetAllProjects("*");
            var exclude = ProjectsToExclude();
            if (Partition.Total == Nuke.Common.CI.Partition.Single.Total)
            {
                foreach (var project in allSolution)
                {
                    if (project.IsNukeBuildProject()) continue;
                    if (exclude?.Contains(project.Name) == true) continue;
                    _projectCache.Add(project);
                }

            }
            else
            {
                foreach (var project in Partition.GetCurrent(allSolution))
                {
                    if (project.IsNukeBuildProject()) continue;
                    if (exclude?.Contains(project.Name) == true) continue;
                    _projectCache.Add(project);
                }

            }
        }

        foreach (var item in _projectCache)
        {
            yield return item;
        }
    }

    IEnumerable<Nuke.Common.ProjectModel.Project> SolutionTestProjects()
    {
        foreach (var item in SolutionProjects())
        {
            if (item.IsTestdProject())
            {
                yield return item;
            }
        }
    }

    IEnumerable<Nuke.Common.ProjectModel.Project> SolutionPackProjects()
    {
        foreach (var item in SolutionProjects())
        {
            if (item.ProducesNugetPackage())
            {
                yield return item;
            }
        }
    }

    List<string>? ProjectsToExclude()
    {
        return null;
    }

    Target Simulate => _ => _
                .OnlyWhenStatic(() => IsLocalBuild)
                .Unlisted()
                .Executes(() =>
                {
                    int allProjects = 0;
                    int allTestProjects = 0;
                    int allPackProjects = 0;
                    Logger.Info("Quering all projects to compile");

                    foreach (var cproj in SolutionProjects())
                    {
                        allProjects++;
                        Logger.Info($"Project {cproj.Name} is found to be compiled ");
                    }


                    Logger.Info("Quering all projects to test");

                    foreach (var cproj in SolutionTestProjects())
                    {
                        allTestProjects++;
                        Logger.Info($"Project {cproj.Name} is found to be tested ");
                    }

                    Logger.Info("Quering all projects to pack");

                    foreach (var cproj in SolutionPackProjects())
                    {
                        allPackProjects++;
                        Logger.Info($"Project {cproj.Name} is found to be packed ");
                    }


                    Logger.Info($"{allProjects} total projects {allTestProjects} test projects {allPackProjects} pack projects");



                });
}
