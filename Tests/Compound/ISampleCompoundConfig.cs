namespace Unibrics.Configuration.Tests
{
    using General;
    using General.Compound;

    [Config("compound", typeof(SampleCompoundConfig))]
    public interface ISampleCompoundConfig
    {
        public JsonConfig Json { get; }
        
        public CsvConfig Csv { get; }
        
        public CsvConfig AnotherCsv { get; }
    }

    class SampleCompoundConfig : CompoundConfig, ISampleCompoundConfig
    {
        public CsvConfig Csv { get; set; }
        
        public CsvConfig AnotherCsv { get; set; }
        
        public JsonConfig Json { get; set; }
    }
}