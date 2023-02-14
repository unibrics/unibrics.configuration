namespace Unibrics.Configuration.General.Fetch
{
    class AlwaysApplyAndCacheConfigApplyChecker : ConfigApplyChecker
    {
        public override bool ShouldApply() => true;

        public override bool ShouldCache() => true;
        
        public override bool IsCachePreservedBetweenVersions => false;
    }
    
    class AlwaysApplyAndDontCacheConfigApplyChecker : ConfigApplyChecker
    {
        public override bool ShouldApply() => true;

        public override bool ShouldCache() => false;
        
        public override bool IsCachePreservedBetweenVersions => false;
    }
}