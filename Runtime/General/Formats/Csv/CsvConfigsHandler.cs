namespace Unibrics.Configuration.General.Formats.Csv
{
    using System;
    using System.IO;
    using Core;
    using Core.Utils.Csv;
    using Parser;
    using UnityEngine;

    public class CsvConfigsHandler : ISingleFormatConfigValuesHandler
    {
        public Priority Priority => Priority.Lowest;

        private readonly PlainMetadataExtractor metadataExtractor = new();

        private const string Delimiter = "<br>";

        public void InjectTo(ConfigFile configFile, string config)
        {
            if (configFile is not ICsvConfigFile csvConfigFile)
            {
                throw new Exception($"Config {configFile.GetType().Name} must inherit ICsvConfigFile in order to" +
                    $"be filled with values from .csv file");
            }

            config = config.Replace(Delimiter, "\n");
            var reader = new CsvReader(new ConfigCsvParsingVisitor(csvConfigFile.RecordType, 
                csvConfigFile is IRecycleCsvRecord, rec => csvConfigFile.Process(rec)));
            reader.Read(config);
            metadataExtractor.ExtractMetadataTo(configFile, config);
        }
        

        public bool CanProcess(string value) => true;
       
        
        public ConfigFile ExtractMetadata(string raw)
        {
            var metadata = new ConfigFile();
            metadataExtractor.ExtractMetadataTo(metadata, raw);
            return metadata;
        }
    }
}