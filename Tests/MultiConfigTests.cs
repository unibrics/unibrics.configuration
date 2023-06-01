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
    public class MultiConfigTests
    {
        private List<ConfigFile> configs;

        private void Prepare(string raw)
        {
            var resolver = Substitute.For<IResolver>();
            resolver.Resolve(typeof(IJsonConfig)).Returns(new JsonConfig());
            resolver.Resolve(typeof(ICsvConfig)).Returns(new CsvConfig());

            var configurator = new ConfigsConfigurator();
            var configFactory = new ConfigsFactory(new ConfigObjectResolverCreator(resolver), 
                new MultiFormatConfigValuesHandler(new List<IFormattedConfigValuesHandler>()
                {
                    new JsonConfigsHandler(configurator), new CsvConfigsHandler()
                }));
            var fetcher = Substitute.For<IConfigsFetcher>();
            var key1 = "prefix_1";
            fetcher.GetValue(key1).Returns(raw);

            configs = configFactory.PrepareConfigs(fetcher,
                new List<ConfigMeta>()
                {
                    new() { Key = key1, ImplementationType = typeof(CsvConfig), InterfaceType = typeof(ICsvConfig), IsMultiConfig = true}
                });
        }
        
        [Test]
        public void ShouldRead_JsonConfig()
        {
            Prepare(FilesProvider.ProvideCsv());
            var config = configs.OfType<IMultiConfig<ICsvConfig>>().First();
            //Assert.That(config.IntValue, Is.EqualTo(12));
            //Assert.That(config.stringValue, Is.EqualTo("test"));
        }
    }

}