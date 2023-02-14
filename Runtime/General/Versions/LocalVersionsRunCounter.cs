namespace Unibrics.Configuration.General.Fetch
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Version;
    using UnityEngine;
    using Utils.Json;

    public class LocalVersionsRunCounter : IVersionRunsCounter
    {
        private const string Key = "local:versions.counter";
        
        private readonly IVersionProvider versionProvider;

        private readonly IJsonSerializer serializer;

        private RunsCounter counter;

        public LocalVersionsRunCounter(IVersionProvider versionProvider, IJsonSerializer serializer)
        {
            this.versionProvider = versionProvider;
            this.serializer = serializer;
            
            RestoreCounter();
        }

        private void RestoreCounter()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                counter = serializer.Deserialize<RunsCounter>(PlayerPrefs.GetString(Key));
            }
            else
            {
                counter = new RunsCounter();
            }
        }

        public void TrackRun()
        {
            counter.Track(versionProvider.FullVersion);
            PlayerPrefs.SetString(Key, serializer.Serialize(counter));
        }

        public int GetTotalRunsOfCurrentVersion()
        {
            return counter.GetRuns(versionProvider.FullVersion);
        }

        public int GetTotalRuns()
        {
            return counter.GetTotalRuns();
        }

        class RunsCounter
        {
            private readonly Dictionary<string, int> runs = new();

            public void Track(string version)
            {
                if (runs.ContainsKey(version))
                {
                    runs[version]++;
                }
                else
                {
                    runs[version] = 1;
                }
                
            }

            public int GetRuns(string version)
            {
                return runs.TryGetValue(version, out var amount) ? amount : 0;
            }

            public int GetTotalRuns()
            {
                return runs.Values.Sum();
            }
        }
    }
}