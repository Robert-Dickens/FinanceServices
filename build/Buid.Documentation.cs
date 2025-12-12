using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;

partial class Build
{
    Target GenerateDocumentation => _ => _
    .DependsOn(GenerateDiagram)
     .Executes(() =>
     {

         var tocGeneratorScript = RootDirectory / "GenerateDocWebsite.cmd";

         if (tocGeneratorScript.ToFileInfo().Exists)
         {
             Logger.Info("Generating documentation website");
             ProcessTasks.StartProcess("GenerateDocWebsite.cmd", workingDirectory: RootDirectory.ToDirectoryInfo().FullName).WaitForExit();
         }

     });
}
