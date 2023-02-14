namespace Unibrics.Configuration.General.Application
{
    using System.Collections.Generic;
    using Saves.General.Fetch;

    public interface IAppliedConfigsSaver
    {
        List<AppliedConfigData> Load();
        
        void Save(List<AppliedConfigData> configs);
    }
}