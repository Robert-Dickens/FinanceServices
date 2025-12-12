using Aspire.Hosting.Lifecycle;
using FinanceServices.Shared;

namespace MyCompanyName.FinanceServices
{
    internal static class Extensions
    {
        /// <summary>
        /// Adds a hook to set the ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable to true for all projects in the application.
        /// </summary>
        public static IDistributedApplicationBuilder AddForwardedHeaders(this IDistributedApplicationBuilder builder)
        {
            builder.Services.TryAddLifecycleHook<AddForwardHeadersHook>();
            return builder;
        }

        private class AddForwardHeadersHook : IDistributedApplicationLifecycleHook
        {
            public Task BeforeStartAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
            {
                string? gatewayEndpointUrl = $"https://{GlobalConstants.Services.GatewayServiceName}".ToLower();
                string? oauthTokenUrl = $"https://{GlobalConstants.Services.IdentityStsServiceName}".ToLower();

                if (appModel.TryGetProjectEndpoints(GlobalConstants.Services.GatewayServiceName, out var gatewayEndpoints))
                {
                    gatewayEndpointUrl = gatewayEndpoints;
                }

                if (appModel.TryGetProjectEndpoints(GlobalConstants.Services.IdentityStsServiceName, out var stsEndpoints))
                {
                    oauthTokenUrl = stsEndpoints;
                }

                foreach (var p in appModel.GetProjectResources())
                {
                    p.Annotations.Add(new EnvironmentCallbackAnnotation(context =>
                    {
                        if (!string.IsNullOrEmpty(oauthTokenUrl))
                        {
                            context.EnvironmentVariables["AuthServer__Authority"] = oauthTokenUrl;
                            context.EnvironmentVariables["OpenApi__Authority"] = oauthTokenUrl;
                        }
                        if (!string.IsNullOrEmpty(gatewayEndpointUrl))
                        {
                            context.EnvironmentVariables["RemoteServices__Default__BaseUrl"] = gatewayEndpointUrl;
                        }
                        context.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] = "true";
                    }));
                }

                return Task.CompletedTask;
            }
        }

        internal static bool TryGetProjectEndpoints(this DistributedApplicationModel projectModel, string serviceName, out string? endpoints)
        {
            endpoints = string.Empty;
            var service = projectModel.GetProjectResources().FirstOrDefault(x => x.Name == serviceName);
            if (service?.TryGetEndpoints(out var tokenEndpoints) == true)
            {
                var allocatedUrl = tokenEndpoints.FirstOrDefault(x => x.Name == "https");
                if (allocatedUrl != null)
                    endpoints = $"{allocatedUrl.UriScheme}://{allocatedUrl.TargetHost}:{allocatedUrl.Port}";
            }


            return !string.IsNullOrEmpty(endpoints);
        }

        internal static IResourceBuilder<T> ConfigureSelf<T>(this IResourceBuilder<T> builder, string? endpointName = null) where T : ProjectResource
        {
            return builder.WithEnvironment(context =>
            {
                var selfUrl = builder.GetEndpoint(endpointName ?? "https");
                context.EnvironmentVariables["App__SelfUrl"] = selfUrl;
                context.EnvironmentVariables["App__Urls__Applications__MVC__RootUrl"] = selfUrl;
            });
        }
    }
}
