using System;
using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationFiles
{
    public class ConnectionStrings
    {
        public ConnectionStringSettings MessageBus
        {
            get
            {
                return Parse(this["MessageBus"]);
            }
        }

        private ConnectionStringSettings Parse(ConnectionStringSettings connectionStringSettings)
        {
            var result = connectionStringSettings.ConnectionString;
            result = ExpandEnvironmentVariables(result);
            connectionStringSettings.ConnectionString = result;
            return connectionStringSettings;
        }

        private string ExpandEnvironmentVariables(string connectionString)
        {
            var result = Environment.ExpandEnvironmentVariables(connectionString);
            return result;
        }

        public ConnectionStringSettings SharedMemory
        {
            get
            {
                return Parse(this["SharedMemory"]);
            }
        }

        public ConnectionStringSettings this[string key]
        {
            get
            {
                return Settings.SettingsConfigFile.ConnectionStrings.ConnectionStrings[key];
            }
        }
    }
}
