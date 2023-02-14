namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;

    public class ConfigsValidator
    {
        private readonly List<ConfigMeta> metas;

        private readonly IDictionary<ConfigMeta, object> preparedObjects = new Dictionary<ConfigMeta, object>();

        public ConfigsValidator(List<ConfigMeta> metas)
        {
            this.metas = metas;
        }

        public void OnConfigPrepared(ConfigMeta configMeta, object configObject)
        {
            if (configObject is ConfigFile)
            {
                preparedObjects[configMeta] = configObject;    
            }
            else
            {
                throw new Exception($"Configuration file for config {configMeta.Key} ({configMeta.ImplementationType.Name}) " +
                                    $"must be a inheritor of {nameof(ConfigFile)} class to work correctly with A/B tests");
            }
        }


        public void Validate()
        {
            foreach (var meta in metas)
            {
                if (!preparedObjects.ContainsKey(meta))
                {
                    throw new Exception(
                        $"Config '{meta.Key}' is discovered, but no config file found. " +
                        $"Probably, you forget to put it to Configs/ folder");
                }
            }
        }
    }
}