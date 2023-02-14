namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    public interface IConfigsPatcher
    {
        string TryPatch(string key, string config);
    }

    class ConfigsPatcher : IConfigsPatcher
    {
        private readonly IDictionary<string, JObject> patches = new Dictionary<string, JObject>();

        private readonly List<string> reservedProperties = new List<string>()
        {
            "apply",
            "ab_test_name",
            "ab_test_variant"
        };

        public ConfigsPatcher(IConfigsFetcher fetcher)
        {
            var keys = fetcher.GetKeys().Where(key => key.StartsWith("patch_")).OrderBy(key => key);
            foreach (var patchKey in keys)
            {
                var rawPatch = fetcher.GetValue(patchKey);
                var patch = JObject.Parse(rawPatch);
                foreach (var property in patch.Properties())
                {
                    if (property.Value is JObject jObject)
                    {
                        patches[property.Name] = jObject;
                        continue;
                    }

                    if (reservedProperties.Contains(property.Name))
                    {
                        continue;
                    }

                    Debug.LogError(
                        $"Wrong patch inside of {patchKey}! Property {property.Name} must be object, skipping");
                }
            }
        }

        public string TryPatch(string key, string config)
        {
            if (!patches.TryGetValue(key, out var patch))
            {
                return config;
            }

            var originalConfig = JObject.Parse(config);
            foreach (var property in patch.Properties())
            {
                if (originalConfig[property.Name] == null)
                {
                    continue;
                }

                originalConfig[property.Name] = property.Value;
            }

            return originalConfig.ToString();
        }
    }
}