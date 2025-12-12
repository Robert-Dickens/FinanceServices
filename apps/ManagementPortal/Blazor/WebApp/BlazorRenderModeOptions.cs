using Microsoft.AspNetCore.Components;

namespace FinanceServices.ManagementPortal.Blazor
{
    [Flags]
    public enum SupportedRenderingModes
    {
        InteractiveServer,
        InteractiveWebAssembly
    }

    public class BlazorRenderModeOptions
    {
        public IComponentRenderMode? DefaultRenderMode { get; set; }

        public SupportedRenderingModes SupportedRenderingModes { get; set; } = SupportedRenderingModes.InteractiveServer;

    }
}
