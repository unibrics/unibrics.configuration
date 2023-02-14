namespace Unibrics.Configuration.General
{
    using Newtonsoft.Json;

    public class ConfigFile
    {
        [JsonProperty("ab_test_name")]
        public string AbTestName { get; set; }
        
        [JsonProperty("ab_test_variant")]
        public string AbTestVariant { get; set; }
        
        [JsonProperty("apply")]
        public ApplyMode Apply { get; set; } = ApplyMode.EveryTimeNoCache;
        
        [JsonProperty("cache_until")]
        public string CacheUntil { get; set; }
        
        [JsonProperty("activationEvent")]
        public string ActivationEvent { get; set; }

        [JsonIgnore]
        public bool HasActivationEvent => ActivationEvent != null;
    }
}
