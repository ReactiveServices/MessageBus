using System.Configuration;
using System.IO;
using ReactiveServices.Configuration.ConfigurationSections;

namespace ReactiveServices.Configuration.ConfigurationFiles
{
    public static class Settings
    {
        private const string SettingsConfigFileName = "Settings.config";
        private static System.Configuration.Configuration _settingsConfigFile;
        internal static System.Configuration.Configuration SettingsConfigFile
        {
            get
            {
                if (_settingsConfigFile == null)
                {
                    var dependenciesConfigFile = new ExeConfigurationFileMap
                    {
                        ExeConfigFilename = SettingsConfigFileName
                    };
                    _settingsConfigFile = ConfigurationManager.OpenMappedExeConfiguration(dependenciesConfigFile, ConfigurationUserLevel.None);
                }
                return _settingsConfigFile;
            }
        }

        /// <summary>
        /// Returns the connection strings registered in the Settings.config file
        /// </summary>
        public static readonly ConnectionStrings ConnectionStrings = new ConnectionStrings();

        public static ReactiveServicesSection ReactiveServices
        {
            get
            {
                return Section<ReactiveServicesSection>("ReactiveServices");
            }
        }

        /// <summary>
        /// Return a typed configuration section from the Settings.config file
        /// </summary>
        public static T Section<T>(string sectionName)
            where T : ConfigurationSection, new()
        {
            return (T)SettingsConfigFile.GetSection(sectionName) ?? new T();
        }
    }
}
