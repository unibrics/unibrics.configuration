namespace Unibrics.Configuration
{
    using System;
    using System.Linq;
    using Core;
    using Core.Services;
    using General;
    using General.ABTests;
    using General.Application;
    using General.Compound;
    using General.Fetch;
    using General.Formats.Csv;
    using General.Formats.Json;
    using General.Multi;
    using ResourcesService;
    using Saves.General.Fetch;
    using Settings;
    using UnityEngine;

    [Install]
    public class ConfigurationInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.Lowest;

        public override void Install(IServicesRegistry services)
        {
            var configMetaProvider = new ConfigMetaProvider();

            var timeout = AppSettings.Get<ConfigurationSettings>().TimeoutSeconds;
            
            services.Add<IConfigsFetcher>().ImplementedBy<CombinedConfigsFetcher>().AsSingleton();
            services.Add<IConfigFetchTimeoutProvider>().ImplementedByInstance(new PredefinedConfigFetchTimeoutProvider(TimeSpan.FromSeconds(timeout)));
            services.Add<IDefaultConfigsFetcher>().ImplementedBy<LocalResourcesFetcher>().AsSingleton();
            services.Add<IConfigsConfigurator>().ImplementedBy<ConfigsConfigurator>().AsSingleton();
            services.Add<IConfigApplyCheckerFactory>().ImplementedBy<ConfigApplyCheckerFactory>().AsSingleton();
            services.Add<IConfigMetaProvider>().ImplementedByInstance(configMetaProvider);
            services.Add<IConfigsRegistry>().ImplementedBy<ConfigsRegistry>().AsSingleton();
            services.Add<IConfigsFactory>().ImplementedBy<ConfigsFactory>().AsSingleton();
            services.Add<IConfigValuesInjector, IConfigMetadataExtractor>().ImplementedBy<MultiFormatConfigValuesHandler>().AsSingleton();
            services.Add<IAppliedConfigsHolder>().ImplementedBy<AppliedConfigsHolder>().AsSingleton();
            services.Add<IConfigObjectCreator>().ImplementedBy<ConfigObjectResolverCreator>().AsSingleton();
            
            services.Add<IFormattedConfigValuesHandler, ISingleFormatConfigValuesHandler>().ImplementedBy<JsonConfigsHandler>().AsSingleton();
            services.Add<IFormattedConfigValuesHandler, ISingleFormatConfigValuesHandler>().ImplementedBy<CsvConfigsHandler>().AsSingleton();
            services.Add<IFormattedConfigValuesHandler>().ImplementedBy<CompoundConfigsHandler>().AsSingleton();
            
            // this is potentially rebindable part
            services.Add<IVersionRunsCounter>().ImplementedBy<LocalVersionsRunCounter>().AsSingleton();
            services.Add<IAppliedConfigsSaver>().ImplementedBy<LocalAppliedConfigsSaver>().AsSingleton();
            services.Add<IABTestsReporter>().ImplementedBy<LogAbTestsReporter>().AsSingleton();
            
            var metas = configMetaProvider.FetchMetas();
            foreach (var meta in metas.Where(m => !m.IsMultiConfig))
            {
                services.Add(meta.InterfaceType).ImplementedBy(meta.ImplementationType).AsSingleton();
            }

            foreach (var meta in metas.Where(m => m.IsMultiConfig))
            {
                services.Add(typeof(IMultiConfig<>).MakeGenericType(meta.InterfaceType))
                    .ImplementedBy(typeof(MultiConfig<>).MakeGenericType(meta.InterfaceType)).AsSingleton();
            }
        }
    }
}