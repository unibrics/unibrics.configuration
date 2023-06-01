namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tools;

    public interface IConfigMetaProvider
    {
        List<ConfigMeta> FetchMetas();
    }

    class ConfigMetaProvider : IConfigMetaProvider
    {
        private readonly List<ConfigMeta> metas = new();
        
        public List<ConfigMeta> FetchMetas()
        {
            if (metas.Any())
            {
                return metas;
            }
            
            var tuples = Types.AnnotatedWith<ConfigAttribute>();
            foreach (var (attr, type) in tuples)
            {
                var key = attr.Key;
                if (attr.IsMultiConfig && (key[^1] != '*' || key.IndexOf('*') != key.Length - 1))
                {
                    throw new Exception(
                        $"MultiConfig {type} key '{attr.Key}' must end with singular '*' to be correctly processed");
                }
                metas.Add(new ConfigMeta()
                {
                    Key = attr.IsMultiConfig? key[..^1] : key,
                    ImplementationType = attr.ImplementedBy,
                    InterfaceType = type,
                    LocalOnly = attr.LocalOnly,
                    IsMultiConfig = attr.IsMultiConfig
                });
            }

            return metas;
        }
    }
}