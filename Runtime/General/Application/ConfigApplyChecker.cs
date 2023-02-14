namespace Unibrics.Configuration.General.Fetch
{
    using System;

    public abstract class ConfigApplyChecker
    {
        /// <summary>
        /// Remote config should be applied and override local
        /// </summary>
        public abstract bool ShouldApply();

        /// <summary>
        /// Remote config should be cached for further prioritized use
        /// </summary>
        /// <returns></returns>
        public abstract bool ShouldCache();
        
        /// <summary>
        /// Remote config should be cached for further prioritized use and used event in next versions
        /// </summary>
        public abstract bool IsCachePreservedBetweenVersions { get; }
    }
}