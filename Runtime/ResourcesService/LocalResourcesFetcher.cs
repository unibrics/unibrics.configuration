namespace Unibrics.Configuration.ResourcesService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using General;
    using UnityEngine;

    public class LocalResourcesFetcher : IDefaultConfigsFetcher
    {
        private IDictionary<string, string> values;
        
        public void StartFetching(TimeSpan fetchLimitTime)
        {
            if (values != null)
            {
                return;
            }
            var allAssets = Resources.LoadAll<TextAsset>("Configs/");
            values = allAssets.ToDictionary(asset => asset.name, asset => asset.text);
        }

        public UniTask StopFetchingAndApply()
        {
            //local fetching is completed immediatly
            return UniTask.CompletedTask;
        }

        public bool ApplyFetched()
        {
            return true;
        }

        public IEnumerable<string> GetKeys()
        {
            return values.Keys;
        }

        public string GetValue(string key)
        {
            if (!values.ContainsKey(key))
            {
                return null;
            }
            
            return values[key];
        }

        public bool HasKey(string key)
        {
            return values.ContainsKey(key);
        }
    }
}