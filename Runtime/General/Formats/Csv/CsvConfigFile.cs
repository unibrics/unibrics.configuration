namespace Unibrics.Configuration.General.Formats.Csv
{
    using System;

    public abstract class CsvConfigFile<TRecord> : ConfigFile, ICsvConfigFile where TRecord : ICsvRecord, new()
    {
        protected abstract void ProcessInternal(TRecord record);

        public Type RecordType => typeof(TRecord);
        
        public void Process(ICsvRecord record)
        {
            ProcessInternal((TRecord)record);
        }
    }

    interface ICsvConfigFile
    {
        Type RecordType { get; }
        
        void Process(ICsvRecord record);
    }

    public interface ICsvRecord
    {
        
    }

}