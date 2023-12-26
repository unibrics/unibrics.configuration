namespace Unibrics.Configuration.General.Formats.Csv.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Core.Utils.Csv;
    using UnityEngine;

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

        private readonly bool recycleRecord;

        public ConfigCsvParsingVisitor(Type recordType, bool recycleRecord, Action<ICsvRecord> onRecordReady)
        {
            this.recycleRecord = recycleRecord;
            this.recordType = recordType;
            this.onRecordReady = onRecordReady;
            properties = recordType.GetProperties().ToList();
            currentObject = (ICsvRecord)Activator.CreateInstance(recordType);
        }

        public void OnCellParsed(string value)
        {
            Debug.Log($"On Cell parsed {value} {state}");
            if (state == CsvParsingState.Header)
            {
                if (value == "metadata:")
                {
                    // switch to parsing metadata, it must be already processed, so we're just skipping
                    state = CsvParsingState.Metadata;
                    return;
                }
                var property = properties.FirstOrDefault(pr =>
                    string.Equals(pr.Name, value, StringComparison.InvariantCultureIgnoreCase));

                headersTemp.Add(new FieldSetter()
                {
                    Name = value,
                    Setter = GetSetterFor(property)
                });
                index++;
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

            var setter = headers[index].Setter;
            Debug.Log($"setter: {setter}");
            setter?.Invoke(currentObject, value);
            index++;

            Action<object, string> GetSetterFor(PropertyInfo propertyInfo)
            {
                if (propertyInfo == null)
                {
                    Debug.Log($"propertyinfo is null, returning");
                    return null;
                }
                var propertyType = propertyInfo.PropertyType;
                if (propertyType == typeof(int))
                {
                    Debug.Log($"returning int setter");
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
                if (propertyType == typeof(float))
                {
                    Debug.Log($"returning float setter");
                    return (obj, val) =>
                    {
                        if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
                        {
                            propertyInfo.SetValue(obj, floatValue);
                        }
                        else
                        {
                            throw new Exception($"Types mismatch, value {val} can not be casted to float");
                        }
                    };
                }
                if (propertyType == typeof(string))
                {
                    Debug.Log($"returning string setter");
                    return propertyInfo.SetValue;
                }
                if (propertyType == typeof(bool))
                {
                    Debug.Log($"returning bool setter");
                    return (obj, val) => propertyInfo.SetValue(obj, val.ToLower() == "true");
                }
                if (propertyType == typeof(List<string>))
                {
                    Debug.Log($"returning list<string> setter");
                    return (obj, val) =>
                    {
                        var list = propertyInfo.GetValue(obj) as List<string>;
                        if (list == null)
                        {
                            list = new List<string>() { val };
                            propertyInfo.SetValue(obj, list);
                        }
                        else
                        {
                            list.Add(val);
                        }
                    };
                }
                if (propertyType.IsEnum)
                {
                    Debug.Log($"returning enum setter");
                    return (obj, val) =>
                    {
                        if (Enum.TryParse(propertyType, val, out var parsed))
                        {
                            propertyInfo.SetValue(obj, parsed);
                        }
                        else
                        {
                            throw new Exception($"Can not set enum value {val} to enum of type {propertyType}. " +
                                $"Variants are: {string.Join(",", Enum.GetValues(propertyType))}");
                        }
                    };
                }

                throw new Exception($"Type {propertyType} is not supported for .csv fields currently");
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

                    index = 0;
                    state = CsvParsingState.Values;
                    headers = headersTemp.ToArray();
                    return;
            }

            Debug.Log($"record ready! {currentObject}");
            onRecordReady?.Invoke(currentObject);
            if (!recycleRecord)
            {
                currentObject = (ICsvRecord)Activator.CreateInstance(recordType);
            }
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