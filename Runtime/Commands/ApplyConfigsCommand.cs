namespace Unibrics.Configuration.Commands
{
    using System.Collections.Generic;
    using Core.Execution;
    using Cysharp.Threading.Tasks;
    using General;
    using Zenject;

    public abstract class ApplyConfigsCommand : ExecutableCommand
    {
        [Inject]
        IConfigsRegistry ConfigsRegistry { get; set; }

        [Inject]
        public IConfigMetaProvider ConfigMetaProvider { get; set; }

        [Inject]
        IConfigsFactory ConfigsFactory { get; set; }
        
        protected async UniTask Process(IConfigsFetcher configsFetcher, List<ConfigMeta> configMetas)
        {
            await configsFetcher.StopFetchingAndApply();
            var configs = ConfigsFactory.PrepareConfigs(configsFetcher, configMetas);
            foreach (var config in configs)
            {
                ConfigsRegistry.Register(config);
            }
        }
    }
}