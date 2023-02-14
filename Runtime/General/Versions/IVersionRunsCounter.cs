namespace Unibrics.Configuration.General.Fetch
{
    using System.Collections.Generic;

    public interface IVersionRunsCounter
    {
        void TrackRun();

        int GetTotalRunsOfCurrentVersion();

        int GetTotalRuns();
    }

    
}