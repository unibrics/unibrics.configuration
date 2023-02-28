namespace Unibrics.Configuration.General
{
    using Core;

    public interface IFormattedConfigValuesHandler : IConfigValuesInjector
    {
        Priority Priority { get; }

        bool CanProcess(string value);

        ConfigFile ExtractMetadata(string raw);
    }
}