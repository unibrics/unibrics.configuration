namespace Unibrics.Configuration.Commands
{
    using Core.Execution;
    using General.Fetch;
    using Zenject;

    public class TrackVersionRunCommand : ExecutableCommand
    {
        [Inject]
        public IVersionRunsCounter VersionRunsCounter { get; set; }

        protected override void ExecuteInternal()
        {
            VersionRunsCounter.TrackRun();
        }
    }
}