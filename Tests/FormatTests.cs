namespace Unibrics.Configuration.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.DI;
    using General;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FormatTests
    {
        [Test]
        public void ShouldRead_JsonConfig()
        {
            var resolver = Substitute.For<IResolver>();
            resolver.Resolve(typeof(IJsonConfig)).Returns(new JsonConfig());

            var configurator = new ConfigsConfigurator();
            var configFactory = new ConfigsFactory(configurator, new ConfigObjectResolverCreator(resolver));
            var fetcher = Substitute.For<IConfigsFetcher>();
            var jsonKey = "json_key";
            fetcher.GetKeys().Returns(new[] { jsonKey });
            fetcher.GetValue(jsonKey).Returns(FilesProvider.ProvideJson());

            var configs = configFactory.PrepareConfigs(fetcher,
                new List<ConfigMeta>()
                {
                    new()
                        { Key = jsonKey, ImplementationType = typeof(ConfigFile), InterfaceType = typeof(IJsonConfig) }
                });

            var jsonConfig = configs.OfType<JsonConfig>().First();
            Assert.That(jsonConfig.IntValue, Is.EqualTo(12));
            Assert.That(jsonConfig.stringValue, Is.EqualTo("test"));
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
    
    public 
}