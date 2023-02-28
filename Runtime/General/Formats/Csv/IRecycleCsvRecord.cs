namespace Unibrics.Configuration.General.Formats.Csv
{
    /// <summary>
    /// Mark your CsvConfigFile with this interface if you want to reuse a single instance of TRecord
    /// in each iteration of calling Process() method and decrease total allocations count
    /// </summary>
    public interface IRecycleCsvRecord
    {
        
    }
}