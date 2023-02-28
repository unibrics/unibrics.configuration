namespace Unibrics.Configuration.General.Formats.Csv.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public interface ICsvParsingVisitor
    {
        void OnCellParsed(string value);
        
        void OnLineEnd();

        void OnFileEnd();
    }

    class ConfigCsvParsingVisitor : ICsvParsingVisitor
    {
        private enum CsvParsingState { Metadata, Header, Values }
        
        private readonly List<FieldSetter> headersTemp = new();

        private readonly List<PropertyInfo> properties;

        private FieldSetter[] headers;

        private CsvParsingState state = CsvParsingState.Header;

        private int index;

        private ICsvRecord currentObject;

        private readonly Action<ICsvRecord> onRecordReady;

        private readonly Type recordType;

        public ConfigCsvParsingVisitor(Type recordType, Action<ICsvRecord> onRecordReady)
        {
            this.recordType = recordType;
            this.onRecordReady = onRecordReady;
            properties = recordType.GetProperties().ToList();
            currentObject = (ICsvRecord)Activator.CreateInstance(recordType);
        }

        public void OnCellParsed(string value)
        {
            if (state == CsvParsingState.Header)
            {
                if (value == "metadata:")
                {
                    // switch to parsing metadata, it must be already processed, so we're just skipping
                    state = CsvParsingState.Metadata;
                    return;
                }
                var property = properties.FirstOrDefault(pr =>
                    string.Equals(pr.Name, value, StringComparison.CurrentCultureIgnoreCase));

                headersTemp.Add(new FieldSetter()
                {
                    Name = value,
                    Setter = GetSetterFor(property)
                });
                return;
            }
            
            if (state == CsvParsingState.Metadata)
            {
                if (value == "values:")
                {
                    state = CsvParsingState.Header;
                }
                return;
            }

            headers[index].Setter?.Invoke(currentObject, value);
            index++;

            Action<object, string> GetSetterFor(PropertyInfo propertyInfo)
            {
                if (propertyInfo == null)
                {
                    return null;
                }
                if (propertyInfo.PropertyType == typeof(int))
                {
                    return (obj, val) =>
                    {
                        if (int.TryParse(val, out var intValue))
                        {
                            propertyInfo.SetValue(obj, intValue);
                        }
                        else
                        {
                            throw new Exception($"Types mismatch, value {val} can not be casted to int");
                        }
                    };
                }
                if (propertyInfo.PropertyType == typeof(float))
                {
                    return (obj, val) =>
                    {
                        if (float.TryParse(val, out var floatValue))
                        {
                            propertyInfo.SetValue(obj, floatValue);
                        }
                        else
                        {
                            throw new Exception($"Types mismatch, value {val} can not be casted to float");
                        }
                    };
                }
                if (propertyInfo.PropertyType == typeof(string))
                {
                    return propertyInfo.SetValue;
                }

                throw new Exception($"Type {propertyInfo.PropertyType} is not supported for .csv fields currently");
            }
        }
        
        public void OnLineEnd()
        {
            switch (state)
            {
                case CsvParsingState.Metadata:
                    return;
                case CsvParsingState.Header:
                    if (index == 0)
                    {
                        // state was just switched from metadata
                        return;
                    }
                    state = CsvParsingState.Values;
                    headers = headersTemp.ToArray();
                    return;
            }

            onRecordReady?.Invoke(currentObject);
            currentObject = (ICsvRecord)Activator.CreateInstance(recordType);
            index = 0;
        }
        
        public void OnFileEnd()
        {
        }
        
        private struct FieldSetter
        {
            public string Name { get; set; }
            
            public Action<object, string> Setter { get; set; }
        }
    }
}