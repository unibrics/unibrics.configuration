namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IConfigValuesInjector
    {
        void InjectTo(ConfigFile configFile, string config);
    }

    class MultiFormatConfigValuesHandler : IConfigValuesInjector, IConfigMetadataExtractor
    {
        private readonly List<IFormattedConfigValuesHandler> handlers;

        private IFormattedConfigValuesHandler GetHandlerFor(string raw) =>
            handlers.First(injector => injector.CanProcess(raw));

        public MultiFormatConfigValuesHandler(List<IFormattedConfigValuesHandler> injectors)
        {
            handlers = injectors.OrderByDescending(injector => injector.Priority).ToList();
        }

        public void InjectTo(ConfigFile configFile, string config)
        {
            GetHandlerFor(config).InjectTo(configFile, config);
        }
        
        public ConfigFile ExtractMetadata(string value)
        {
            return GetHandlerFor(value).ExtractMetadata(value);
        }
    }

}