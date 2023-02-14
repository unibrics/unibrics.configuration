namespace Unibrics.Configuration.General.Fetch
{
    public class OnFirstSessionChecker : OncePerVersionFirstSessionChecker
    {
        public OnFirstSessionChecker(IVersionRunsCounter versionRunsCounter) : base(versionRunsCounter)
        {
        }
        
        public override bool IsCachePreservedBetweenVersions => true;
    }
}