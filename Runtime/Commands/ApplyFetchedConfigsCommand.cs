namespace Unibrics.Configuration.Commands
{
    using System.Linq;
    using General;
    using Zenject;

    public class ApplyFetchedConfigsCommand : ApplyConfigsCommand
    {
        [Inject]
        public IConfigsFetcher ConfigsFetcher { get; set; }
        
        protected override async void ExecuteInternal()
        {
            Retain();
            var configMetas = ConfigMetaProvider.FetchMetas().Where(meta => !meta.LocalOnly).ToList();
            await Process(ConfigsFetcher, configMetas);
            ReleaseAndComplete();
        }
    }
}