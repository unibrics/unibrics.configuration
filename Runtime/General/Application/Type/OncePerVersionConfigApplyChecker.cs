namespace Unibrics.Configuration.General.Fetch
{
    using Core.Version;

    class OncePerVersionConfigApplyChecker : ConfigApplyChecker
    {
        protected readonly IVersionProvider VersionProvider;

        private readonly IAppliedConfigsHolder appliedConfigsHolder;

        private readonly string key;

        public OncePerVersionConfigApplyChecker(IVersionProvider versionProvider,
            IAppliedConfigsHolder appliedConfigsHolder, string key)
        {
            this.VersionProvider = versionProvider;
            this.appliedConfigsHolder = appliedConfigsHolder;
            this.key = key;
        }

        public override bool ShouldApply()
        {
            return appliedConfigsHolder.TryGetAppliedConfigFor(key, VersionProvider.FullVersion) == null;
        }

        public override bool ShouldCache() => true;
        
        public override bool IsCachePreservedBetweenVersions => false;
    }
}