namespace Unibrics.Configuration.Saves.General.Fetch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration.General.Application;
    using Configuration.General.Fetch;
    using Core.Version;
    using Newtonsoft.Json;
    using UnityEngine;

    class AppliedConfigsHolder : IAppliedConfigsHolder
    {
        private readonly IVersionProvider versionProvider;

        private readonly IAppliedConfigsSaver configsSaver;

        public AppliedConfigsHolder(IVersionProvider versionProvider, IAppliedConfigsSaver configsSaver)
        {
            this.versionProvider = versionProvider;
            this.configsSaver = configsSaver;
        }

        private List<AppliedConfigData> configs;

        private List<AppliedConfigData> Configs
        {
            get
            {
                if (configs == null)
                {
                    configs = configsSaver.Load();
                    FilterOutOldConfigs(configs);
                }

                return configs;
            }
        }

        public string TryGetAppliedConfigFor(string key, string version)
        {
            return Configs.FirstOrDefault(data =>
            {
                if (data.Key == key && data.Version == version)
                {
                    return true;
                }

                if (data.Key == key && data.CacheUntilVersion != null && new Version(data.CacheUntilVersion) > new Version(version))
                {
                    return true;
                }
                
                return false;
            })?.GetValue();
        }

        public void Store(string key, string value, string version, string limitVersion)
        {
            var current = Configs.FirstOrDefault(data => data.Key == key);
            if (current != null)
            {
                Configs.Remove(current);
            }
            Configs.Add(new AppliedConfigData(version, key, value, limitVersion));
            configsSaver.Save(Configs);
        }

        public IEnumerable<string> GetKeysByVersion(string version)
        {
            return Configs.Where(data => data.Version == version).Select(data => data.Key);
        }
        
        //clean up save from cached configs from older version
        private void FilterOutOldConfigs(List<AppliedConfigData> appliedConfigs)
        {
            var current = versionProvider.FullVersion;
            for (var index = appliedConfigs.Count - 1; index >= 0; index--)
            {
                var config = appliedConfigs[index];
                if (config.Version == current)
                {
                    continue;
                }

                if (config.CacheUntilVersion != null)
                {
                    var cachedUntil = new Version(config.CacheUntilVersion);
                    var currentVersion = new Version(current);
                    if (cachedUntil > currentVersion)
                    {
                        continue;
                    }
                }
                
                appliedConfigs.RemoveAt(index);
            }
        }
    }

    public class AppliedConfigData
    {
        public string Version { get; }
        
        public string Key { get;  }

        public string Value { get; }
        
        public string CacheUntilVersion { get; }

        private const string LineBreak = "<BR>";

        public AppliedConfigData(string version, string key, string value, string cacheUntilVersion)
        {
            Version = version;
            Key = key;
            Value = value?.Replace("\n", LineBreak);
            CacheUntilVersion = cacheUntilVersion;
        }

        public string GetValue() => Value?.Replace(LineBreak, "\n");
    }
}