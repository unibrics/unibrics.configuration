namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;

    class RemoteConfigsMockFetcher : IConfigsFetcher
    {
        public void StartFetching(TimeSpan fetchLimitTime)
        {
            
        }

        public UniTask StopFetchingAndApply()
        {
            return UniTask.CompletedTask;
        }

        public IEnumerable<string> GetKeys()
        {
            yield break;
        }

        public string GetValue(string key)
        {
            throw new Exception("Get value shouldn't be called on mock fetcher");
        }

        public bool HasKey(string key)
        {
            return true;
        }
    }
}