using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class DependencyInjectionsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = false, IsKey = false, IsDefaultCollection = true)]
        public DependencyInjectionCollection Items
        {
            get
            {
                return ((DependencyInjectionCollection)(base[""]));
            }
            set
            {
                base[""] = value; 
            }
        }
    }
}
