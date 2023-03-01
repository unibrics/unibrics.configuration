namespace Unibrics.Configuration.General.Formats.Csv
{
    using System;
    using System.IO;
    using Core;
    using Core.Utils.Csv;
    using Parser;
    using UnityEngine;

    public class CsvConfigsHandler : IFormattedConfigValuesHandler
    {
        public Priority Priority => Priority.Lowest;

        public void InjectTo(ConfigFile configFile, string config)
        {
            if (configFile is not ICsvConfigFile csvConfigFile)
            {
                throw new Exception($"Config {configFile.GetType().Name} must inherit ICsvConfigFile in order to" +
                    $"be filled with values from .csv file");
            }
            
            var reader = new CsvReader(new ConfigCsvParsingVisitor(csvConfigFile.RecordType, 
                csvConfigFile is IRecycleCsvRecord, rec => csvConfigFile.Process(rec)));
            reader.Read(config);
            ExtractMetadataTo(configFile, config);
        }
        

        public bool CanProcess(string value) => true;

        private void ExtractMetadataTo(ConfigFile metadata, string raw)
        {
            using var reader = new StringReader(raw);
            var firstLine = reader.ReadLine();
            if (firstLine is not "metadata:")
            {
                return;
            }

            while (true)
            {
                var next = reader.ReadLine();
                if (next is null or "values:")
                {
                    return;
                }

                var split = next.Split('=');
                var parameter = split[0];
                var value = split[1];
                
                // skip reflection here for better performance
                switch (parameter)
                {
                    case ConfigFileField.AbTestName:
                        metadata.AbTestName = value;
                        break; 
                    case ConfigFileField.AbTestVariant:
                        metadata.AbTestVariant = value;
                        break;
                    case ConfigFileField.Apply:
                        metadata.Apply = Enum.Parse<ApplyMode>(value);
                        break;
                    case ConfigFileField.CacheUntil:
                        metadata.CacheUntil = value;
                        break;
                    case ConfigFileField.ActivationEvent:
                        metadata.ActivationEvent = value;
                        break;
                }
            }
        }
        
        public ConfigFile ExtractMetadata(string raw)
        {
            var metadata = new ConfigFile();
            ExtractMetadataTo(metadata, raw);
            return metadata;
        }
    }
}