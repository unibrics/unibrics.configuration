namespace Unibrics.Configuration.General
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using UnityEngine;

    /// <summary>
    /// Use this interface in specific cases when you can not use usual config system: e.g., configs haven't been processed yet.
    /// Be aware that config converters won't process config
    /// </summary>
    public interface ILocalConfigLoader
    {
        TConfig Load<TConfig>();
    }

    public class LocalConfigLoader : ILocalConfigLoader
    {
        private const string Path = "Configs/";
        
        public TConfig Load<TConfig>()
        {
            var attr = (ConfigAttribute)typeof(TConfig).GetCustomAttribute(typeof(ConfigAttribute));
            if (attr == null)
            {
                throw new ArgumentException(
                    $"type {typeof(TConfig)} must be annotated with {nameof(ConfigAttribute)} attribute");
            }

            var key = attr.Key;
            var raw = Resources.Load<TextAsset>($"{Path}{key}").text;
            var instance = (TConfig)Activator.CreateInstance(attr.ImplementedBy);
            JsonConvert.PopulateObject(raw, instance);

            return instance;
        }
    }
}