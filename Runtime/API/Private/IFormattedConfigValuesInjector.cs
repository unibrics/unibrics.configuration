namespace Unibrics.Configuration.General
{
    using Core;

    public interface IFormattedConfigValuesInjector : IConfigValuesInjector
    {
        Priority Priority { get; }

        bool CanProcess(string value);
    }
}