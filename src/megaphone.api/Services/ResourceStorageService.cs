using Dapr.Client;
using Megaphone.Api.Models.Views;
using Megaphone.Standard.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megaphone.Api.Services
{
    public class ResourceStorageService : IPartionedStorageService<ResourceListView>
    {
        const string STATE_STORE = "api-state";

        private readonly DaprClient client;
        private readonly Dictionary<string, string> trackedEtags = new Dictionary<string, string>();

        public ResourceStorageService(DaprClient client)
        {
            this.client = client;
        }

        public async Task<ResourceListView> GetAsync(string partitionKey, string contentKey)
        {
            string key = $"resources/{partitionKey}/{contentKey}";
            var (value, etag) = await client.GetStateAndETagAsync<ResourceListView>(STATE_STORE, key);
            trackedEtags[key] = etag;

            return value;
        }

        public async Task SetAsync(string partitionKey, string contentKey, ResourceListView content)
        {
            string key = $"resources/{partitionKey}/{contentKey}";

            content.Updated = DateTimeOffset.UtcNow;

            if (trackedEtags.ContainsKey(key))
            {
                var stateSaved = await client.TrySaveStateAsync(STATE_STORE, key, content, trackedEtags[key]);
                if (stateSaved)
                    return;
                throw new Exception($"failed to save state for {key}");
            }
            else
            {
                await client.SaveStateAsync(STATE_STORE, key, content);
            }
        }
    }
}
