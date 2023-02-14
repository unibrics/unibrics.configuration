namespace Unibrics.Configuration.General.ABTests
{
    using System.Collections.Generic;

    public interface IABTestsReporter
    {
        /// <summary>
        /// tests must is TestName: TestVariant dictionary 
        /// </summary>
        void ReportActiveTests(Dictionary<string, string> tests);

        void ReportTestActivation(ConfigFile config);
    }
}