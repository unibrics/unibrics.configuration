namespace Unibrics.Configuration.Commands
{
    using System;
    using System.Linq;
    using General;
    using Zenject;

    public class ApplyLocalOnlyConfigsCommand : ApplyConfigsCommand
    {
        [Inject]
        public IDefaultConfigsFetcher ConfigsFetcher { get; set; }
        
        protected override async void ExecuteInternal()
        {
            UnityEngine.Application.targetFrameRate = 60;
            UnityEngine.Input.multiTouchEnabled = false;
            
            var configMetas = ConfigMetaProvider.FetchMetas().Where(meta => meta.LocalOnly).ToList();
            ConfigsFetcher.StartFetching(TimeSpan.MaxValue);
            await Process(ConfigsFetcher, configMetas);
        }
    }
}