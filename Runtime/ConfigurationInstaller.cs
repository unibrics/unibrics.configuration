namespace Unibrics.Configuration
{
    using System;
    using Core;
    using Core.Services;
    using General;
    using General.Application;
    using General.Fetch;
    using ResourcesService;
    using Saves.General.Fetch;

    [Install]
    public class ConfigurationInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.Lowest;

        public override void Install(IServicesRegistry services)
        {
            var configMetaProvider = new ConfigMetaProvider();
            
            services.Add<IConfigsFetcher>().ImplementedBy<CombinedConfigsFetcher>().AsSingleton();
            services.Add<IConfigFetchTimeoutProvider>().ImplementedByInstance(new PredefinedConfigFetchTimeoutProvider(TimeSpan.FromSeconds(5)));
            services.Add<IDefaultConfigsFetcher>().ImplementedBy<LocalResourcesFetcher>().AsSingleton();
            services.Add<IConfigsConfigurator>().ImplementedBy<ConfigsConfigurator>().AsSingleton();
            services.Add<IConfigApplyCheckerFactory>().ImplementedBy<ConfigApplyCheckerFactory>().AsSingleton();
            services.Add<IConfigMetaProvider>().ImplementedByInstance(configMetaProvider);
            services.Add<IConfigsRegistry>().ImplementedBy<ConfigsRegistry>().AsSingleton();
            services.Add<IConfigsFactory>().ImplementedBy<ConfigsFactory>().AsSingleton();
            services.Add<IAppliedConfigsHolder>().ImplementedBy<AppliedConfigsHolder>().AsSingleton();
            services.Add<IVersionRunsCounter>().ImplementedBy<LocalVersionsRunCounter>().AsSingleton();
            services.Add<IConfigObjectCreator>().ImplementedBy<ConfigObjectResolverCreator>().AsSingleton();
            
            var metas = configMetaProvider.FetchMetas();
            foreach (var meta in metas)
            {
                services.Add(meta.InterfaceType).ImplementedBy(meta.ImplementationType).AsSingleton();
            }
        }
    }
}