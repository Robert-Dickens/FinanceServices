using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using C4Sharp.Diagrams;
using System.Linq;
using C4Sharp.Diagrams.Interfaces;
using System.Collections.Generic;
using C4Sharp.Diagrams.Plantuml;
using System.IO;

partial class Build
{
    [Parameter][Optional] readonly string DiagramsExportPath;

    Target GenerateDiagram => _ => _
         .Executes(() =>
         {
             var path = DiagramsExportPath?.Replace("'", "").Replace("\"", "");

             if (!string.IsNullOrEmpty(path))
             {
                 path = AbsolutePath.Create(path) / Solution.Name;
             }
             else
             {
                 path = DocFxPath / "models" / "diagrams";
             }

             AbsolutePath outputPath = AbsolutePath.Create(path);
             outputPath.CreateOrCleanDirectory();


             //var solutionComponentDiagram = new Diagrams.SolutionDiagram(Solution);


             //var diagrams = new C4Sharp.Diagrams.DiagramBuilder[] { solutionComponentDiagram };

             //var exportDirectory = outputPath.ToDirectoryInfo();

             //new C4Sharp.Diagrams.Plantuml.PlantumlContext()
             //        .UseDiagramImageBuilder()
             //        .UseDiagramSvgImageBuilder()
             //        .UseDiagramMermaidBuilder()
             //        .Export(exportDirectory.FullName, diagrams, new C4Sharp.Diagrams.Themes.ParadisoTheme());
             var result = new List<IDiagramBuilder>();

             var architecture = RootDirectory.GlobFiles("**/*.Architecture.dll").FirstOrDefault();

             if (architecture == null)
             {
                 DotNetBuild(new DotNetBuildSettings()
                                .SetConfiguration(Configuration)
                                .SetProjectFile(Solution));

                 architecture = RootDirectory.GlobFiles("**/*.Architecture.dll").FirstOrDefault();
             }

             var architectureLib = new System.IO.FileInfo(architecture.ToString());

             if (architectureLib.Exists)
             {
                 var runners = System.Reflection.Assembly.LoadFrom(architectureLib.FullName).GetTypes()
                            .Where(p => typeof(IDiagramBuilder).IsAssignableFrom(p) && p.IsClass && p != typeof(DiagramBuilder))
                            .Select(r => (IDiagramBuilder)System.Activator.CreateInstance(r)!).ToArray();
                 result.AddRange(runners);
             }

             GenerateC4Diagrams(result, outputPath);

         });

    private static void GenerateC4Diagrams(IEnumerable<IDiagramBuilder> runners, string? ouput)
    {
        var directory = new DirectoryInfo(ouput).FullName;

        var context = new PlantumlContext()
            .UseDiagramImageBuilder()
            .UseDiagramMermaidBuilder()
            .UseDiagramSvgImageBuilder();

        context.Export(directory, runners);
    }

}
