namespace Unibrics.Configuration.General.Fetch
{
    using System.Collections.Generic;

    /// <summary>
    /// After configs are applied, we need to store them somewhere
    /// to prevent from overriding with another ones
    /// </summary>
    interface IAppliedConfigsHolder
    {
        string TryGetAppliedConfigFor(string key, string version);

        void Store(string key, string value, string currentVersion, string limitVersion);

        IEnumerable<string> GetKeysByVersion(string version);
    }

   
}