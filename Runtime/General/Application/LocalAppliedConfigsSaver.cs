namespace Unibrics.Configuration.General.Application
{
    using System.Collections.Generic;
    using Saves.General.Fetch;
    using UnityEngine;
    using Utils.Json;

    class LocalAppliedConfigsSaver : IAppliedConfigsSaver
    {
        private IJsonSerializer serializer;

        private const string Key = "applied.configs";
        
        public List<AppliedConfigData> Load()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                return serializer.Deserialize<ConfigsWrapper>(PlayerPrefs.GetString(Key)).Configs;
            }

            return new List<AppliedConfigData>();
        }

        public void Save(List<AppliedConfigData> configs)
        {
            PlayerPrefs.SetString(Key, serializer.Serialize(new ConfigsWrapper(configs)));
        }

        class ConfigsWrapper
        {
            public List<AppliedConfigData> Configs { get; set; }

            public ConfigsWrapper(List<AppliedConfigData> configs)
            {
                Configs = configs;
            }
        }
    }
}