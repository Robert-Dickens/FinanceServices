using ByteLabs.Foundations.Threading;
using ByteLabs.Foundations.Http.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace FinanceServices.ManagementPortal.Blazor.WebAssembly
{
    public class RemoteServicesPersistentStateOptions : IPostConfigureOptions<RemoteServiceOptions>
    {
        private RemoteServiceOptions? _state = null;
        private readonly PersistentComponentState _persistentComponentState;
        private readonly ILogger<RemoteServicesPersistentStateOptions> _log;

        public RemoteServicesPersistentStateOptions(PersistentComponentState persistentComponentState, ILogger<RemoteServicesPersistentStateOptions>log)
        {
            _persistentComponentState = persistentComponentState;
            _log = log;
        }

        public void PostConfigure(string? name, RemoteServiceOptions options)
        {
            var state = AsyncHelper.RunSync(GetPersistedStateAsync);

            if(state?.RemoteServices?.Count > 0)
            {
                _log.LogInformation("Merging RemoteServiceOptions state");

                foreach (var r in state.RemoteServices)
                {
                    if(!r.Value.BaseUrl.IsNullOrWhiteSpace())
                    options.RemoteServices[r.Key] = r.Value;
                }
            }
        }

        protected virtual Task<RemoteServiceOptions?> GetPersistedStateAsync()
        {
            if (_state != null)
            {
                _log.LogDebug("Using cached state RemoteServiceOptions");
                return Task.FromResult<RemoteServiceOptions?>(_state);
            }

            _log.LogInformation("Getting state RemoteServiceOptions from PersistentComponentState");

            _state = _persistentComponentState.TryTakeFromJson<RemoteServiceOptions>("remote_services", out var token)
                ? token
                : null;

            _log.LogDebug($"Resolved {_state?.RemoteServices?.Count ?? 0} Remote Services from PersistentComponentState");

            return Task.FromResult(_state);
        }

    }
}
