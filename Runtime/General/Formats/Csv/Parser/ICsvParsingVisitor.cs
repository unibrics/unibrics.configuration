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
        private readonly List<FieldSetter> headersTemp = new();

        private readonly List<PropertyInfo> properties;

        private FieldSetter[] headers;

        private bool isParsingHeader = true;

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
            if (isParsingHeader)
            {
                var property = properties.FirstOrDefault(pr =>
                    string.Equals(pr.Name, value, StringComparison.CurrentCultureIgnoreCase));

                headersTemp.Add(new FieldSetter()
                {
                    Name = value,
                    Setter = GetSetterFor(property)
                });
                return;
            }

            Debug.Log($"cell : {headers[index]}={value}");
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
            Debug.Log($"line end! {currentObject}");
            if (isParsingHeader)
            {
                isParsingHeader = false;
                headers = headersTemp.ToArray();
                return;
            }
            
            onRecordReady?.Invoke(currentObject);
            currentObject = (ICsvRecord)Activator.CreateInstance(recordType);
            index = 0;
        }
        
        public void OnFileEnd()
        {
            Debug.Log($"file end");
        }
        
        private struct FieldSetter
        {
            public string Name { get; set; }
            
            public Action<object, string> Setter { get; set; }
        }
    }
}