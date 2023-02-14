namespace Unibrics.Configuration.General
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Interface), MeansImplicitUse]
    public class ConfigAttribute : Attribute
    {
        public string Key { get; }
        
        public Type ImplementedBy { get; }
        
        public bool LocalOnly { get; set; }

        public ConfigAttribute(string key, Type implementedBy)
        {
            Key = key;
            ImplementedBy = implementedBy;
        }
    }
}