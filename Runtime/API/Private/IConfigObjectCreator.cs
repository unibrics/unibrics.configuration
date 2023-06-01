namespace Unibrics.Configuration.General
{
    using System;
    using Core.DI;
    using Multi;

    interface IConfigObjectCreator
    {
        ConfigFile CreateObject(ConfigMeta meta);

        MultiConfig CreateMultiConfigFor(ConfigMeta meta);
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
            // multi config are not bound via DI, so every config is just created through reflection
            var config = meta.IsMultiConfig? Activator.CreateInstance(meta.ImplementationType) : resolver.Resolve(meta.InterfaceType);
            if (config is ConfigFile configFile)
            {
                return configFile;
            }

            throw new Exception($"Configuration file for config {meta.Key} ({meta.ImplementationType.Name}) " +
                                $"must be a inheritor of {nameof(ConfigFile)} class to work correctly with A/B tests");
        }

        public MultiConfig CreateMultiConfigFor(ConfigMeta meta)
        {
            if (!meta.IsMultiConfig)
            {
                throw new Exception("Only meta marked with IsMultiConfig can be used to create one");
            }
            
            return (MultiConfig)resolver.Resolve(typeof(IMultiConfig<>).MakeGenericType(meta.InterfaceType));
        }
    }
}