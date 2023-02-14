namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;

    interface IConfigsRegistry
    {
        void Register(ConfigFile config);
        
        IEnumerable<ConfigFile> AllConfigs { get; }
    }

    class ConfigsRegistry : IConfigsRegistry
    {
        private readonly List<ConfigFile> configs = new List<ConfigFile>();
        
        public void Register(ConfigFile configObject)
        {
            configs.Add(configObject);
        }

        public IEnumerable<ConfigFile> AllConfigs => configs;
    }
}