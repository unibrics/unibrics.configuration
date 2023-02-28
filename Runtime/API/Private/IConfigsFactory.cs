namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using UnityEngine;
    using Utils.Json;

    interface IConfigsFactory
    {
        List<ConfigFile> PrepareConfigs(IConfigsFetcher configsFetcher, List<ConfigMeta> configMetas);
    }

    class ConfigsFactory : IConfigsFactory
    {
        private readonly IConfigObjectCreator configObjectCreator;

        private readonly IConfigValuesInjector valuesInjector;
        
        public ConfigsFactory(IConfigObjectCreator configObjectCreator, IConfigValuesInjector valuesInjector)
        {
            this.configObjectCreator = configObjectCreator;
            this.valuesInjector = valuesInjector;
        }

        public List<ConfigFile> PrepareConfigs(IConfigsFetcher configsFetcher,
            List<ConfigMeta> configMetas)
        {
            var keys = configsFetcher.GetKeys().ToList();
            var patcher = new ConfigsPatcher(configsFetcher);
            var validator = new ConfigsValidator(configMetas);


            var result = new List<ConfigFile>();
            foreach (var configMeta in configMetas)
            {
                var key = configMeta.Key;
                if (!keys.Contains(key))
                {
                    throw new Exception($"Can not find requested config '{key}' for type {configMeta.InterfaceType}");
                }

                var value = configsFetcher.GetValue(key);
                var configObject = configObjectCreator.CreateObject(configMeta);

                try
                {
                    value = patcher.TryPatch(key, value);
                    valuesInjector.InjectTo(configObject, value);
                    
                    validator.OnConfigPrepared(configMeta, configObject);
                    
                    result.Add(configObject);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error while processing {key}:");
                    Debug.LogError(value);
                    Debug.LogException(e);
                }
            }

            validator.Validate();
            return result;
        }
    }
}