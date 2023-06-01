namespace Unibrics.Configuration.General
{
    using System;

    public class ConfigMeta
    {
        public string Key { get; set; }
        
        public Type InterfaceType { get; set; }
        
        public Type ImplementationType { get; set; }
        
        public bool LocalOnly { get; set; }

        public bool IsMultiConfig { get; set; }
    }

}