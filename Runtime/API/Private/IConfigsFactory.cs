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
        private readonly IConfigsConfigurator configurator;

        private readonly IConfigObjectCreator configObjectCreator;
        
        public ConfigsFactory(IConfigsConfigurator configurator, IConfigObjectCreator configObjectCreator)
        {
            this.configurator = configurator;
            this.configObjectCreator = configObjectCreator;
        }

        public List<ConfigFile> PrepareConfigs(IConfigsFetcher configsFetcher,
            List<ConfigMeta> configMetas)
        {
            var keys = configsFetcher.GetKeys().ToList();
            var patcher = new ConfigsPatcher(configsFetcher);
            var validator = new ConfigsValidator(configMetas);

            var converters = configurator.Converters;

            var result = new List<ConfigFile>();
            var settings = new JsonSerializerSettings()
            {
                Converters = converters
            };

            foreach (var conv in converters.OfType<IJsonSerializerInitializable>())
            {
                conv.Init(settings);
            }

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
                    JsonConvert.PopulateObject(value, configObject, settings);

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