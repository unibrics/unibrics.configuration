namespace Unibrics.Configuration.General.Fetch
{
    public class OncePerVersionFirstSessionChecker : ConfigApplyChecker
    {
        private readonly IVersionRunsCounter versionRunsCounter;

        public OncePerVersionFirstSessionChecker(IVersionRunsCounter versionRunsCounter)
        {
            this.versionRunsCounter = versionRunsCounter;
        }

        public override bool ShouldApply()
        {
            return versionRunsCounter.GetTotalRuns() == 1;
        }

        public override bool ShouldCache() => true;
        
        public override bool IsCachePreservedBetweenVersions => false;
    }
}