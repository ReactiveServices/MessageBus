using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class DependencyInjectionElement : ConfigurationElement
    {
        [ConfigurationProperty("AbstractType", DefaultValue = null, IsRequired = false, IsKey = true)]
        public string AbstractType
        {
            get { return (string)this["AbstractType"]; }
            set { this["AbstractType"] = value; }
        }
        [ConfigurationProperty("ConcreteType", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string ConcreteType
        {
            get { return (string)this["ConcreteType"]; }
            set { this["ConcreteType"] = value; }
        }
        [ConfigurationProperty("Lifestyle", DefaultValue = null, IsRequired = false, IsKey = false)]
        public string Lifestyle
        {
            get { return (string)this["Lifestyle"]; }
            set { this["Lifestyle"] = value; }
        }
    }
}
