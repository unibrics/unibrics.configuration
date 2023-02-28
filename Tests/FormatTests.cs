namespace Unibrics.Configuration.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.DI;
    using General;
    using General.Formats.Csv;
    using General.Formats.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FormatTests
    {
        private List<ConfigFile> configs;

        [SetUp]
        public void SetUp()
        {
            var resolver = Substitute.For<IResolver>();
            resolver.Resolve(typeof(IJsonConfig)).Returns(new JsonConfig());
            resolver.Resolve(typeof(ICsvConfig)).Returns(new CsvConfig());

            var configurator = new ConfigsConfigurator();
            var configFactory = new ConfigsFactory(new ConfigObjectResolverCreator(resolver), 
                new MultiFormatConfigValuesInjector(new List<IFormattedConfigValuesInjector>()
            {
                new JsonConfigsInjector(configurator), new CsvConfigsInjector()
            }));
            var fetcher = Substitute.For<IConfigsFetcher>();
            var jsonKey = "json_key";
            var csvKey = "csv_key";
            fetcher.GetKeys().Returns(new[] { jsonKey, csvKey});
            fetcher.GetValue(jsonKey).Returns(FilesProvider.ProvideJson());
            fetcher.GetValue(csvKey).Returns(FilesProvider.ProvideCsv());

            configs = configFactory.PrepareConfigs(fetcher,
                new List<ConfigMeta>()
                {
                    new()
                        { Key = jsonKey, ImplementationType = typeof(JsonConfig), InterfaceType = typeof(IJsonConfig) },
                    new()
                        { Key = csvKey, ImplementationType = typeof(CsvConfig), InterfaceType = typeof(ICsvConfig) }
                });
        }
        
        [Test]
        public void ShouldRead_JsonConfig()
        {
           var jsonConfig = configs.OfType<JsonConfig>().First();
            Assert.That(jsonConfig.IntValue, Is.EqualTo(12));
            Assert.That(jsonConfig.stringValue, Is.EqualTo("test"));
        }
        
        [Test]
        public void ShouldRead_CsvConfig()
        {
            var csvConfig = configs.OfType<CsvConfig>().First();
            
            Assert.That(csvConfig.Values.Count, Is.EqualTo(2));
            Assert.That(csvConfig.Values[0].SampleInt, Is.EqualTo(45));
            Assert.That(csvConfig.Values[0].SampleFloat, Is.EqualTo(2.45f));
            Assert.That(csvConfig.Values[0].SampleString, Is.EqualTo("value"));
            Assert.That(csvConfig.Values[1].SampleString, Is.EqualTo("value2"));
        }
    }


    [Config("json_key", typeof(JsonConfig))]
    public interface IJsonConfig
    {
        int IntValue { get; set; }

        string StringValue { get; set; }
    }

    public class JsonConfig : ConfigFile
    {
        public int IntValue { get; set; }

        public string stringValue { get; set; }
    }

    [Config("csv_config", typeof(CsvConfig))]
    public interface ICsvConfig
    {
        int SampleInt { get; }
        
        int SampleFloat { get; }
        
        int SampleString { get; }
    }

    public class CsvConfig : TableConfigFile<CsvConfig.CsvRecord>
    {
        public struct CsvRecord : ICsvRecord
        {
            public int SampleInt { get; set; }
            
            public float SampleFloat { get; set; }
            public string SampleString { get; set; }

            public override string ToString()
            {
                return $"{nameof(SampleInt)}: {SampleInt}, {nameof(SampleFloat)}: {SampleFloat}, {nameof(SampleString)}: {SampleString}";
            }
        }
    }
}