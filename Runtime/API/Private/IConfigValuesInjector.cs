namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IConfigValuesInjector
    {
        void InjectTo(ConfigFile configFile, string config);
    }

    class MultiFormatConfigValuesInjector : IConfigValuesInjector
    {
        private readonly List<IFormattedConfigValuesInjector> injectors;

        public MultiFormatConfigValuesInjector(List<IFormattedConfigValuesInjector> injectors)
        {
            this.injectors = injectors.OrderByDescending(injector => injector.Priority).ToList();
        }

        public void InjectTo(ConfigFile configFile, string config)
        {
            injectors.First(injector => injector.CanProcess(config)).InjectTo(configFile, config);
        }
    }

}