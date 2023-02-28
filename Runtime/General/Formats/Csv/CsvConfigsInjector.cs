namespace Unibrics.Configuration.General.Formats.Csv
{
    using System;
    using Core;
    using Parser;

    public class CsvConfigsInjector : IFormattedConfigValuesInjector
    {
        public Priority Priority => Priority.Lowest;

        public void InjectTo(ConfigFile configFile, string config)
        {
            if (configFile is not ICsvConfigFile csvConfigFile)
            {
                throw new Exception($"Config {configFile.GetType().Name} must inherit ICsvConfigFile in order to" +
                    $"be filled with values from .csv file");
            }
            
            var reader = new CsvReader(new ConfigCsvParsingVisitor(csvConfigFile.RecordType, rec => csvConfigFile.Process(rec)));
            reader.Read(config);
        }
        

        public bool CanProcess(string value) => true;
    }
}