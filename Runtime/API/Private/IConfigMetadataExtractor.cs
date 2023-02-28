namespace Unibrics.Configuration.General
{
    public interface IConfigMetadataExtractor
    {
        ConfigFile ExtractMetadata(string value);
    }
}