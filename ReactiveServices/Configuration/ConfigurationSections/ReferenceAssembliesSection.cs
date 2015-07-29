using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class ReferenceAssembliesSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = false, IsKey = false, IsDefaultCollection = true)]
        public AssemblyElementCollection Items
        {
            get
            {
                return ((AssemblyElementCollection)(base[""]));
            }
            set
            {
                base[""] = value; 
            }
        }
    }
}
