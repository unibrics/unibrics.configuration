namespace Unibrics.Configuration.General
{
    using System;

    public interface IConfigFetchTimeoutProvider
    {
        TimeSpan Timeout { get; }
    }

    class PredefinedConfigFetchTimeoutProvider : IConfigFetchTimeoutProvider
    {
        public TimeSpan Timeout { get; }

        public PredefinedConfigFetchTimeoutProvider(TimeSpan timeout)
        {
            Timeout = timeout;
        }
    }
}