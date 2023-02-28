namespace Unibrics.Configuration.General.Formats.Csv
{
    using System.Collections.Generic;

    public abstract class TableConfigFile<TRecord> : CsvConfigFile<TRecord> where TRecord : ICsvRecord, new()
    {
        public List<TRecord> Values { get; } = new();

        protected override void ProcessInternal(TRecord record)
        {
            Values.Add(record);
        }
    }
}