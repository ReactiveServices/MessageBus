using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class AssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("AssemblyName", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string AssemblyName
        {
            get { return (string) this["AssemblyName"]; }
            set { this["AssemblyName"] = value; }
        }
    }
}
