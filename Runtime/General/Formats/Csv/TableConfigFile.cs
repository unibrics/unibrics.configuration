namespace Unibrics.Configuration.General.Formats.Csv
{
    using System.Collections.Generic;

    public abstract class TableConfigFile<TRecord> : CsvConfigFile<TRecord> where TRecord : ICsvRecord, new()
    {
        protected List<TRecord> Values { get; } = new();

        public override void Process(TRecord record)
        {
            Values.Add(record);
        }
    }
}