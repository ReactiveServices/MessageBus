using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class ReactiveServicesSection : ConfigurationSection
    {
        [ConfigurationProperty("ShowConsoleWindow", IsRequired = false)]
        public bool ShowConsoleWindow
        {
            get
            {
                return ((bool)(base["ShowConsoleWindow"]));
            }
            set
            {
                base["ShowConsoleWindow"] = value; 
            }
        }
    }
}
