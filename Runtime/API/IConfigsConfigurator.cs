namespace Unibrics.Configuration.General
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface IConfigsConfigurator
    {
        List<JsonConverter> Converters { get; }
        
        IConfigsFetcher RemoteFetcher { get; }
        
        void RegisterConverter(JsonConverter converter);

        void RegisterRemoteFetcher(IConfigsFetcher fetcher);
    }

    class ConfigsConfigurator : IConfigsConfigurator
    {
        public List<JsonConverter> Converters { get; } = new List<JsonConverter>();
        
        public IConfigsFetcher RemoteFetcher { get; private set; }

        public void RegisterConverter(JsonConverter converter)
        {
            Converters.Add(converter);
        }

        public void RegisterRemoteFetcher(IConfigsFetcher fetcher)
        {
            RemoteFetcher = fetcher;
        }
    }
}