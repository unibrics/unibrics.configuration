namespace Unibrics.Configuration.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.DI;
    using General;
    using General.Compound;
    using General.Formats.Csv;
    using General.Formats.Json;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CompoundConfigTests
    {
        private List<ConfigFile> configs;


        private void Prepare(string raw)
        {
            var resolver = Substitute.For<IResolver>();
            resolver.Resolve(typeof(IJsonConfig)).Returns(new JsonConfig());
            resolver.Resolve(typeof(ICsvConfig)).Returns(new CsvConfig());
            resolver.Resolve(typeof(ISampleCompoundConfig)).Returns(new SampleCompoundConfig());

            var configurator = new ConfigsConfigurator();
            var objectCreator = new ConfigObjectResolverCreator(resolver);
            var configFactory = new ConfigsFactory(objectCreator,
                new MultiFormatConfigValuesHandler(new List<IFormattedConfigValuesHandler>()
                {
                    new JsonConfigsHandler(configurator),
                    new CsvConfigsHandler(),
                    new CompoundConfigsHandler(new ()
                    {
                        new JsonConfigsHandler(configurator),
                        new CsvConfigsHandler(),
                    })
                }));
            var fetcher = Substitute.For<IConfigsFetcher>();
            var key1 = "compound";
            fetcher.GetValue(key1).Returns(raw);
            fetcher.GetKeys().Returns(new[] { key1 });

            configs = configFactory.PrepareConfigs(fetcher,
                new List<ConfigMeta>()
                {
                    new()
                    {
                        Key = key1, ImplementationType = typeof(SampleCompoundConfig), 
                        InterfaceType = typeof(ISampleCompoundConfig)
                    }
                });
        }
        
        [Test]
        public void ShouldRead_CompoundConfig()
        {
            Prepare(FilesProvider.ProvideCompoundConfig());
            var config = configs.OfType<ISampleCompoundConfig>().First();
            Assert.That(config.Csv.Values[0].SampleInt, Is.EqualTo(45));
            Assert.That(config.AnotherCsv.Values[1].SampleFloat, Is.EqualTo(6.42f));
            Assert.That(config.Json.stringValue, Is.EqualTo("test"));
        }
    }
}