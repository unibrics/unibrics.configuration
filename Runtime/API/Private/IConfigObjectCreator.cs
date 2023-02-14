namespace Unibrics.Configuration.General
{
    using System;
    using Core.DI;

    public interface IConfigObjectCreator
    {
        ConfigFile CreateObject(ConfigMeta meta);
    }

    class ConfigObjectResolverCreator : IConfigObjectCreator
    {
        private readonly IResolver resolver;

        public ConfigObjectResolverCreator(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public ConfigFile CreateObject(ConfigMeta meta)
        {
            var config = resolver.Resolve(meta.InterfaceType);
            if (config is ConfigFile configFile)
            {
                return configFile;
            }

            throw new Exception($"Configuration file for config {meta.Key} ({meta.ImplementationType.Name}) " +
                                $"must be a inheritor of {nameof(ConfigFile)} class to work correctly with A/B tests");
        }
    }
}