using System;
using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
            result = ParseSpecialFolders(result);
            result = ParseEnvironmentVariables(result);
            connectionStringSettings.ConnectionString = result;
            return connectionStringSettings;
        }

        private string ParseSpecialFolders(string connectionString)
        {
            return connectionString.Replace("${specialfolder:folder=ApplicationData}",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        private string ParseEnvironmentVariables(string connectionString)
        {
            return connectionString.Replace("${env_var:RABBITMQ_HOSTNAME}", //TODO - MAKE PROPER IMPLEMENTATION
                EnvironmentVariable("RABBITMQ_HOSTNAME"));
        }

        private string EnvironmentVariable(string variable)
        {
            var result = Environment.GetEnvironmentVariable(variable);
            return result;
            //const string keyName = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment\";
            //var result = (string)Registry.LocalMachine.OpenSubKey(keyName).GetValue(variable, "");
            //return result;
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
