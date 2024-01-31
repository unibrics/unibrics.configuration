namespace Unibrics.Configuration.General.Compound
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Core;
    using Formats;
    using UnityEngine;

    public class CompoundConfigsHandler : IFormattedConfigValuesHandler
    {
        public Priority Priority => Priority.High;

        private readonly List<ISingleFormatConfigValuesHandler> handlers;

        private const string SectionPrefix = "section.";
        
        private const string Delimiter = "<br>";
        
        private readonly PlainMetadataExtractor metadataExtractor = new();

        internal CompoundConfigsHandler(List<ISingleFormatConfigValuesHandler> handlers)
        {
            this.handlers = handlers;
        }

        public void InjectTo(ConfigFile configFile, string config)
        {
            if (configFile is not CompoundConfig)
            {
                throw new Exception(
                    $"class {configFile.GetType().Name} should be inherited from CompoundConfig in order " +
                    $"to be processed as compound");
            } 
            
            config = config.Replace(Delimiter, "\n");
            metadataExtractor.ExtractMetadataTo(configFile, config);
            var sections = ParseSections(config);
            var properties = configFile.GetType().GetProperties();
            foreach (var (fieldName, configRaw) in sections)
            {
                var property = properties.FirstOrDefault(info =>
                    string.Equals(info.Name, fieldName, StringComparison.InvariantCultureIgnoreCase));
                if (property == null)
                {
                    continue;
                }

                if (property.PropertyType.IsAbstract)
                {
                    throw new Exception($"Currently, abstract property types are not supported in compound configs." +
                        $" Field {property.PropertyType.Name} {property.Name}");
                }

                var propertyConfig = Activator.CreateInstance(property.PropertyType);
                if (propertyConfig is not ConfigFile configProperty)
                {
                    throw new Exception($"Every property in CompoundConfig should be ConfigFile inheritor, " +
                        $"but {property.PropertyType.Name} {property.Name} is not");
                }
                var handler = handlers.First(handler => handler.CanProcess(configRaw));
                handler.InjectTo(configProperty, configRaw);

                property.SetValue(configFile, configProperty);
            }
        }

        private Dictionary<string, string> ParseSections(string raw)
        {
            var reader = new StringReader(raw);

            var sb = new StringBuilder();
            var next = metadataExtractor.SkipMetadata(reader);
            var sections = new Dictionary<string, string>();

            if (!IsSectionHeader(next))
            {
                // todo: add validator for corrupted files
            }

            var currentSection = ParseSectionHeader(next);
            next = reader.ReadLine();

            while (next != null)
            {
                if (IsSectionHeader(next))
                {
                    sections[currentSection] = sb.ToString();

                    sb.Clear();
                    currentSection = ParseSectionHeader(next);
                }
                else
                {
                    sb.AppendLine(next);
                }

                next = reader.ReadLine();
            }

            if (sb.Length > 0)
            {
                sections[currentSection] = sb.ToString();
            }

            return sections;
        }

        public bool CanProcess(string value)
        {
            var reader = new StringReader(value);
            var firstLine = metadataExtractor.SkipMetadata(reader);
            return IsSectionHeader(firstLine);
        }

        private bool IsSectionHeader(string raw)
        {
            return raw != null && raw.StartsWith(SectionPrefix) && raw.EndsWith(":");
        }

        private string ParseSectionHeader(string raw)
        {
            return raw[SectionPrefix.Length..^1];
        }

        public ConfigFile ExtractMetadata(string raw)
        {
            var metadata = new ConfigFile();
            metadataExtractor.ExtractMetadataTo(metadata, raw);
            return metadata;
        }
    }
}