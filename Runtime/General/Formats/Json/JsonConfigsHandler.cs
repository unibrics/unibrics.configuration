namespace Unibrics.Configuration.General.Formats.Json
{
    using System.Linq;
    using Core;
    using Newtonsoft.Json;
    using Utils.Json;

    public class JsonConfigsHandler : IFormattedConfigValuesHandler
    {
        public Priority Priority => Priority.High;

        private readonly JsonSerializerSettings settings;

        public JsonConfigsHandler(IConfigsConfigurator configurator)
        {
            var converters = configurator.Converters;
            settings = new JsonSerializerSettings()
            {
                Converters = converters
            };
            
            foreach (var conv in converters.OfType<IJsonSerializerInitializable>())
            {
                conv.Init(settings);
            }
        }

        // we assume that object starting with { are jsons
        public bool CanProcess(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                {
                    continue;
                }

                return value[i] == '{';
            }

            return false;
        }
        
        public ConfigFile ExtractMetadata(string raw)
        {
            return JsonConvert.DeserializeObject<ConfigFile>(raw);
        }

        public void InjectTo(ConfigFile configFile, string config)
        {
            
            JsonConvert.PopulateObject(config, configFile, settings);
        }
    }
}