namespace Unibrics.Configuration.General
{
    using Newtonsoft.Json;

    public class ConfigFile
    {
        [JsonProperty(ConfigFileField.AbTestName)]
        public string AbTestName { get; set; }
        
        [JsonProperty(ConfigFileField.AbTestVariant)]
        public string AbTestVariant { get; set; }
        
        [JsonProperty(ConfigFileField.Apply)]
        public ApplyMode Apply { get; set; } = ApplyMode.EveryTimeNoCache;
        
        [JsonProperty(ConfigFileField.CacheUntil)]
        public string CacheUntil { get; set; }
        
        [JsonProperty(ConfigFileField.ActivationEvent)]
        public string ActivationEvent { get; set; }

        [JsonIgnore]
        public bool HasActivationEvent => ActivationEvent != null;
    }

    public static class ConfigFileField
    {
        public const string AbTestName = "ab_test_name";
        public const string AbTestVariant = "ab_test_variant";
        public const string Apply = "apply";
        public const string CacheUntil = "cache_until";
        public const string ActivationEvent = "activation_event";
    }
}
