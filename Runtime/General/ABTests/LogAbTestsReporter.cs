namespace Unibrics.Configuration.General.ABTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Logs;

    class LogAbTestsReporter : IABTestsReporter
    {
        public void ReportActiveTests(Dictionary<string, string> tests)
        {
            var args = tests.Select(pair => $"{pair.Key}:{pair.Value}").ToList();
            Logger.Log($"Active AB tests: {string.Join(";", args)}");
        }

        public void ReportTestActivation(ConfigFile config)
        {
            Logger.Log($"{config.ActivationEvent} activated!");
        }
    }
}