namespace Unibrics.Configuration.General
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;

    public interface IConfigsFetcher
    {
        void StartFetching(TimeSpan fetchLimitTime);

        UniTask StopFetchingAndApply();
        
        IEnumerable<string> GetKeys();

        string GetValue(string key);

        bool HasKey(string key);
    }

    public interface IDefaultConfigsFetcher : IConfigsFetcher
    {
        
    }
}