using ReactiveServices.Configuration.ConfigurationSections;
using ReactiveServices.Configuration.TypeResolution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NLog;
using PostSharp.Patterns.Diagnostics;

namespace ReactiveServices.Configuration.ConfigurationFiles
{
    public static class Dependencies
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const string DependenciesConfigFileName = "Dependencies.config";
        private static System.Configuration.Configuration _dependenciesConfigFile;
        private static System.Configuration.Configuration DependenciesConfigFile
        {
            get
            {
                if (_dependenciesConfigFile == null)
                {
                    var dependenciesConfigFile = new ExeConfigurationFileMap
                    {
                        ExeConfigFilename = DependenciesConfigFileName
                    };
                    _dependenciesConfigFile = ConfigurationManager.OpenMappedExeConfiguration(dependenciesConfigFile, ConfigurationUserLevel.None);
                }
                return _dependenciesConfigFile;
            }
        }

        internal static IEnumerable<DependencyInjectionMapping> DependencyInjectionMappings { get; private set; }

        /// <summary>
        /// Load reference assemblies and dependency injection mapping from the configuration file
        /// </summary>
        public static void Load()
        {
            LoadReferenceAssemblies();
            LoadDependencyInjectionMappings();
        }

        /// <summary>
        /// Inform if the reference assemblies and dependency injection mapping was already loaded from the configuraiton file
        /// </summary>
        public static bool Loaded
        {
            get { return DependencyInjectionMappings != null && DependencyInjectionMappings.Any(); }
        }

        [Log]
        [LogException]
        private static void LoadDependencyInjectionMappings()
        {
            var dependencyInjectionConfigSection = DependenciesConfigFile.GetSection("DependencyInjections") as DependencyInjectionsSection;

            if (dependencyInjectionConfigSection == null)
                throw new ConfigurationErrorsException(String.Format("DependencyInjections section not found in the {0} file!", DependenciesConfigFileName));

            DependencyInjectionMappings = dependencyInjectionConfigSection.Items.Select(die =>
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(die.AbstractType))
                        die.AbstractType = die.ConcreteType;

                    var dim = new DependencyInjectionMapping
                    {
                        AbstractType = TypeResolver.Resolve(die.AbstractType),
                        ConcreteType = TypeResolver.Resolve(die.ConcreteType)
                    };

                    if (String.IsNullOrWhiteSpace(die.Lifestyle))
                        dim.Lifestyle = Lifestyle.Transient;
                    else
                    {
                        Lifestyle lifestyle;
                        if (Enum.TryParse(die.Lifestyle, true, out lifestyle))
                            dim.Lifestyle = lifestyle;
                        else
                            throw new SettingsPropertyWrongTypeException(
                                String.Format("Invalid value for dependency injection lifestyle: {0}", lifestyle));
                    }
                    return dim;
                }
                catch (Exception e)
                {
                    Log.Error("Could not resolve dependencies", e);
                    throw;
                }
            });
        }

        [Log]
        [LogException]
        private static void LoadReferenceAssemblies()
        {
            var referenceAssembliesConfigSection = DependenciesConfigFile.GetSection("ReferenceAssemblies") as ReferenceAssembliesSection;

            if (referenceAssembliesConfigSection == null)
                throw new ConfigurationErrorsException(String.Format("ReferenceAssemblies section not found in the {0} file!", DependenciesConfigFileName));

            foreach (AssemblyElement assembly in referenceAssembliesConfigSection.Items)
            {
                AssemblyResolver.Resolve(assembly.AssemblyName);
            }
        }
    }
}
