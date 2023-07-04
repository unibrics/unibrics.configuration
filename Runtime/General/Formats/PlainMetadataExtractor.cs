namespace Unibrics.Configuration.General.Formats
{
    using System;
    using System.IO;

    public class PlainMetadataExtractor
    {
        public void ExtractMetadataTo(ConfigFile metadata, string raw)
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

        /// <summary>
        /// Returns first line after metadata section
        /// </summary>
        public string SkipMetadata(StringReader sr)
        {
            var firstLine = sr.ReadLine();
            if (firstLine is not "metadata:")
            {
                return firstLine;
            }

            while (true)
            {
                var next = sr.ReadLine();
                if (next == null)
                {
                    return null;
                }
                if (next == "values:")
                {
                    return sr.ReadLine();
                }
            }
        }
    }
}