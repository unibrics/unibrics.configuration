namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;
    using System.Linq;
    using Tools;

    public interface IConfigMetaProvider
    {
        List<ConfigMeta> FetchMetas();
    }

    class ConfigMetaProvider : IConfigMetaProvider
    {
        private readonly List<ConfigMeta> metas = new List<ConfigMeta>();
        
        public List<ConfigMeta> FetchMetas()
        {
            if (metas.Any())
            {
                return metas;
            }
            
            var tuples = Types.AnnotatedWith<ConfigAttribute>();
            foreach (var (attr, type) in tuples)
            {
                metas.Add(new ConfigMeta()
                {
                    Key = attr.Key,
                    ImplementationType = attr.ImplementedBy,
                    InterfaceType = type,
                    LocalOnly = attr.LocalOnly
                });
            }

            return metas;
        }
    }
}