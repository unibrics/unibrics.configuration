namespace Unibrics.Configuration.Tests
{
    using General;

    [Config("csv_config", typeof(CsvConfig))]
    public interface ICsvConfig
    {
        int SampleInt { get; }
        
        int SampleFloat { get; }
        
        int SampleString { get; }
    }
}