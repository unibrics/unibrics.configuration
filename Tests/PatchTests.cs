namespace Unibrics.Configuration.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using General;
    using General.Formats.Json;
    using NUnit.Framework;

    [TestFixture]
    public class PatchTests
    {
        private IConfigsFactory factory;
        
        [SetUp]
        public void SetUp()
        {
            factory = new ConfigsFactory(new ActivatorConfigObjectsCreator(), new MultiFormatConfigValuesHandler(new List<IFormattedConfigValuesHandler>()
            {
                new JsonConfigsHandler(new ConfigsConfigurator())
            }));
        }

        [Test]
        public void _01PatchShouldBeApplied()
        {
            var configs = factory.PrepareConfigs(new MockConfigsFetcher(), new List<ConfigMeta>()
            {
                new ConfigMeta() { Key = "configA", ImplementationType = typeof(ConfigAMock) },
                new ConfigMeta() { Key = "configB", ImplementationType = typeof(ConfigBMock) },
            });
            
            Assert.That(configs.OfType<ConfigAMock>().First().KeyA1, Is.EqualTo("patchedA1"));
            Assert.That(configs.OfType<ConfigBMock>().First().KeyB2, Is.EqualTo("patchedB2"));
        }
        
        [Test]
        public void _02PatchedShouldBeAppliedInOrder()
        {
            var configs = factory.PrepareConfigs(new MockConfigsFetcher(), new List<ConfigMeta>()
            {
                new ConfigMeta() { Key = "configA", ImplementationType = typeof(ConfigAMock) },
                new ConfigMeta() { Key = "configB", ImplementationType = typeof(ConfigBMock) },
            });
            
            Assert.That(configs.OfType<ConfigBMock>().First().KeyB1, Is.EqualTo("patchedB4"));
        }
        
        private class MockConfigsFetcher : IConfigsFetcher
        {
            private readonly List<(string key, string config)> configs = new List<(string key, string config)>()
            {
                ("configA", "{ \"keyA1\" : \"valueA1\", \"keyA2\" : \"valueA2\" }"),
                ("configB", "{ \"keyB1\" : \"valueB1\", \"keyB2\" : \"valueB2\" }"),
                ("patch.1", "{ \"configA\" : { \"keyA1\" : \"patchedA1\" }, \"configB\" :  { \"keyB2\" : \"patchedB2\", \"keyB1\" : \"patchedB3\" } }"),
                ("patch.2", "{ \"configA\" : { \"keyA1\" : \"patchedA1\" }, \"configB\" :  { \"keyB2\" : \"patchedB2\", \"keyB1\" : \"patchedB4\" } }"),
            };
            
            public void StartFetching(TimeSpan fetchLimitTime)
            {
            
            }

            public UniTask StopFetchingAndApply()
            {
                return UniTask.CompletedTask;
            }

            public IEnumerable<string> GetKeys()
            {
                return configs.Select(tuple => tuple.key);
            }

            public string GetValue(string key)
            {
                return configs.FirstOrDefault(tuple => tuple.key == key).config;
            }

            public bool HasKey(string key)
            {
                return true;
            }
        }
        
        private class ConfigAMock : ConfigFile
        {
            public string KeyA1 { get; set; }
            
            public string KeyA2 { get; set; }
        }

        private class ConfigBMock : ConfigFile
        {
            public string KeyB1 { get; set; }
            
            public string KeyB2 { get; set; }
        }

        private class ActivatorConfigObjectsCreator : IConfigObjectCreator
        {
            public ConfigFile CreateObject(ConfigMeta meta)
            {
                return (ConfigFile)Activator.CreateInstance(meta.ImplementationType);
            }
        }
    }
}