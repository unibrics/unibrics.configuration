namespace Unibrics.Configuration.General.Formats.Csv
{

    public abstract class CsvConfigFile<TRecord> where TRecord : ICsvRecord, new()
    {
        public abstract void Process(TRecord record);
    }

    public interface ICsvRecord
    {
        
    }

}