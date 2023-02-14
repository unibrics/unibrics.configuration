namespace Unibrics.Configuration.General.Fetch
{
    using Core.Version;
    using Zenject;

    public interface IConfigApplyCheckerFactory
    {
        ConfigApplyChecker Create(ApplyMode applyMode, string key);
    }

    class ConfigApplyCheckerFactory : IConfigApplyCheckerFactory
    {
        [Inject]
        public IVersionRunsCounter VersionRunsCounter { get; set; }

        [Inject]
        public IVersionProvider VersionProvider { get; set; }

        [Inject]
        public IAppliedConfigsHolder AppliedConfigsHolder { get; set; }
        
        public ConfigApplyChecker Create(ApplyMode applyMode, string key)
        {
            switch (applyMode)
            {
                case ApplyMode.EveryTimeCache:
                    return new AlwaysApplyAndCacheConfigApplyChecker();
                case ApplyMode.OncePerVersion:
                    return new OncePerVersionConfigApplyChecker(VersionProvider, AppliedConfigsHolder, key);
                case ApplyMode.OncePerVersionFirstRun:
                    return new OncePerVersionFirstRunConfigApplyChecker(VersionProvider, AppliedConfigsHolder, key, VersionRunsCounter);
                case ApplyMode.OncePerVersionOnFirstSession:
                    return new OncePerVersionFirstSessionChecker(VersionRunsCounter);
                case ApplyMode.OnFirstSession:
                    return new OnFirstSessionChecker(VersionRunsCounter);
                default:
                    return new AlwaysApplyAndDontCacheConfigApplyChecker();
            }
        }
    }
}