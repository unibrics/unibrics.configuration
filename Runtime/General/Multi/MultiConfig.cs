namespace Unibrics.Configuration.General.Multi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    abstract class MultiConfig
    {
        internal abstract void Add(string key, ConfigFile config);
    }

    class MultiConfig<TConfig> : MultiConfig, IMultiConfig<TConfig> where TConfig : class
    {
        private readonly Dictionary<string, TConfig> configs = new();

        public TConfig GetBuyId(string id)
        {
            if (configs.TryGetValue(id, out var config))
            {
                return config;
            }

            return default;
        }

        public IEnumerable<TConfig> GetAll() => configs.Values;
        
        public IEnumerable<(string key, TConfig value)> GetAllWithKeys()
        {
            return configs.Select(config => (config.Key, config.Value));
        }

        internal override void Add(string key, ConfigFile config)
        {
            if (config is not TConfig typedConfig)
            {
                throw new Exception($"Config type {config.GetType()} must implement interface {typeof(TConfig)} " +
                    $"to be used in MultiConfig");
            }
            configs.Add(key, typedConfig);
        }
    }
}