namespace Unibrics.Configuration.General.Fetch
{
    using Core.Version;

    class OncePerVersionFirstRunConfigApplyChecker : OncePerVersionConfigApplyChecker
    {
        private readonly IVersionRunsCounter versionRunsCounter;


        public OncePerVersionFirstRunConfigApplyChecker(IVersionProvider versionProvider,
            IAppliedConfigsHolder appliedConfigsHolder, string key, IVersionRunsCounter versionRunsCounter) : base(
            versionProvider, appliedConfigsHolder, key)
        {
            this.versionRunsCounter = versionRunsCounter;
        }

        public override bool ShouldApply() => base.ShouldApply() && versionRunsCounter.GetTotalRunsOfCurrentVersion() == 1;

        public override bool ShouldCache() => true;
    }
}