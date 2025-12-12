using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.Tooling;
using System.Collections.Generic;
using PlatformTools;
using JetBrains.Annotations;
using Nuke.Components;
using System.Linq;

partial class Build
{
    Target LibManRestore => _ => _
    .OnlyWhenStatic(() => !SucceededTargets.Contains(LibManRestore) || !SucceededTargets.Contains(((ICompile)this).Compile))
    .Executes(() =>
    {
        var srcDirectory = RootDirectory / "src";
        var libmanFiles = srcDirectory.GlobFiles("**/libman.json");

        int projectsRestored = 0;
        int totalPackages = 0;
        if (libmanFiles.Count > 0)
        {
            Logger.Info("Restoring LibMan packages...");

            foreach (var libmanFile in libmanFiles)
            {

                var libmanPath = libmanFile.Parent;
                Logger.Info($"Restoring LibMan packages for {libmanFile}");

                var settings = new DotNetLibManRestoreSettings()
                   .SetLibManPath(libmanPath);

                var libmanRestore = LabManTool.LibManRestore(settings);

                if (libmanRestore.Count > 0)
                {
                    var errors = libmanRestore.Where(x => x.Type == OutputType.Err).ToList();

                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            Assert.Fail(error.Text);
                        }
                    }

                    totalPackages = totalPackages + libmanRestore.Count;
                    Logger.Info($"LibMan packages restored {libmanRestore.Count} for {libmanFile}");
                }
            }

        }
        ReportSummary(_ => _
                .AddPair("Restored", projectsRestored)
                .AddPair("TotalPackages", totalPackages));

    });
}


namespace PlatformTools
{
    [Command(Type = typeof(LabManTool), Command = nameof(LabManTool.LibManRestore), Arguments = "restore")]
    public partial class DotNetLibManRestoreSettings : ToolOptions
    {
    }

    [NuGetTool(Id = PackageId, Executable = PackageExecutable)]
    public class LabManTool : ToolTasks, IRequireNuGetPackage
    {
        public static string LabManToolPath { get => new LabManTool().GetToolPathInternal(); set => new LabManTool().SetToolPath(value); }
        public const string PackageId = "Microsoft.Web.LibraryManager.Cli";
        public const string PackageExecutable = "libman.dll";

        public static IReadOnlyCollection<Output> LibManRestore(DotNetLibManRestoreSettings options = null) => new LabManTool().Run<DotNetLibManRestoreSettings>(options);

    }
    public static partial class DotNetLibManRestoreSettingsExtensions
    {
        [Pure]
        [Builder(Type = typeof(DotNetLibManRestoreSettings), Property = nameof(DotNetLibManRestoreSettings.ProcessWorkingDirectory))]
        public static T SetLibManPath<T>(this T o, string v) where T : DotNetLibManRestoreSettings => o.SetProcessWorkingDirectory(v);

    }
}