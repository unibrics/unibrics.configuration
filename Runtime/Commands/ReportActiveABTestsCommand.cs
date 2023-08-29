namespace Unibrics.Configuration.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Execution;
    using General;
    using General.ABTests;
    using Zenject;

    public class ReportActiveAbTestsCommand : ExecutableCommand
    {
        [Inject]
        public List<IABTestsReporter> TestsReporters { get; set; }

        [Inject]
        IConfigsRegistry ConfigsRegistry { get; set; }

        protected override void ExecuteInternal()
        {
            var activeTests = ConfigsRegistry.AllConfigs
                    .Where(file => file.AbTestName != null)
                    .Select(file => (name: file.AbTestName, variant: file.AbTestVariant))
                ;

            var args = new Dictionary<string, string>();
            foreach (var (name, variant) in activeTests)
            {
                args[variant] = name;
            }

            TestsReporters.ForEach(reporter => reporter.ReportActiveTests(args));;
        }
    }
}