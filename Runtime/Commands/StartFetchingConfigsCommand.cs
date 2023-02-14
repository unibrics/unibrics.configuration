namespace Unibrics.Configuration.Commands
{
    using Core.Execution;
    using General;

    public class StartFetchingConfigsCommand : ExecutableCommand
    {
        private readonly IConfigsFetcher configsFetcher;

        private readonly IConfigFetchTimeoutProvider timeoutProvider;

        public StartFetchingConfigsCommand(IConfigsFetcher configsFetcher, IConfigFetchTimeoutProvider timeoutProvider)
        {
            this.configsFetcher = configsFetcher;
            this.timeoutProvider = timeoutProvider;
        }

        protected override void ExecuteInternal()
        {
            configsFetcher.StartFetching(timeoutProvider.Timeout);
        }
    }
}