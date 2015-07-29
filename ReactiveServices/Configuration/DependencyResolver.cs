using ReactiveServices.Configuration.ConfigurationFiles;
using System;
using PostSharp.Patterns.Diagnostics;

namespace ReactiveServices.Configuration
{
    public class DependencyResolver
    {
        private static readonly object SyncRoot = new object();
        private static DependencyContainer Container { get; set; }

        /// <summary>
        /// Inform if the dependency resolver was already initialized
        /// </summary>
        public static bool IsInitialized { get; private set; }

        static DependencyResolver()
        {
            Container = new DependencyContainer();
        }

        /// <summary>
        /// Reset the dependency resolver and discard any dependency injection performed on initialize.
        /// </summary>
        public static void Reset()
        {
            IsInitialized = false;
            Container.Dispose();
            Container = new DependencyContainer();
        }

        /// <summary>
        /// Initialize the dependency injector.
        /// Load the reference assemblies and dependency injection mapping from the configuration file, if necessary.
        /// Register the dependency injection mapping loaded from the configuration file.
        /// Execute the custom dependency registration action, if any.
        /// Verify if the dependencies are valid.
        /// This method can be called only once by process, just after startup.
        /// </summary>
        /// <param name="dependenciesRegistrator">An action used to perform custom dependency injections</param>
        [Log]
        [LogException]
        public static void Initialize(Action<DependencyContainer> dependenciesRegistrator = null)
        {
            lock (SyncRoot)
            {
                if (IsInitialized)
                    throw new InvalidOperationException(
                        "The dependencies for this module are already resolved." +
                        "Please call DependencyResolver.Reset() before trying to Initialize the dependencies in a different way."
                    );

                if (!DependenciesLoaded)
                    LoadDependencies();

                RegisterDependencies(dependenciesRegistrator);

                //Container.Verify(); Attention: Verify will instanciate all registered objects, but will not dispose them

                IsInitialized = true;
            }
        }

        private static void RegisterDependencies(Action<DependencyContainer> dependenciesRegistrator)
        {
            // Register dependencies according to the Dependencies.config file.
            foreach (var dependencyInjectionMapping in Dependencies.DependencyInjectionMappings)
                Container.Register(
                    dependencyInjectionMapping.AbstractType,
                    dependencyInjectionMapping.ConcreteType,
                    dependencyInjectionMapping.Lifestyle
                );

            // Register custom dependencies using a given custom dependencies registrator
            if (dependenciesRegistrator != null)
                dependenciesRegistrator.Invoke(Container);
        }

        /// <summary>
        /// Load reference assemblies and dependency injection mapping from the configuration file
        /// </summary>
        public static void LoadDependencies()
        {
            Dependencies.Load();
        }
        
        /// <summary>
        /// Inform if the reference assemblies and dependency injection mapping was already loaded from the configuraiton file
        /// </summary>
        public static bool DependenciesLoaded
        {
            get { return Dependencies.Loaded; }
        }

        /// <summary>
        /// Return the concrete type registered for a given abstract type.
        /// Any dependencies of this type that are also registered will be instanciated as well.
        /// </summary>
        /// <typeparam name="TAbstractType">Abstract type to be instanciated according to the dependency injection registration</typeparam>
        /// <returns>Concrete type registered for the given abstract type</returns>
        public static TAbstractType Get<TAbstractType>() where TAbstractType : class
        {
            return Container.GetInstance<TAbstractType>();
        }

        /// <summary>
        /// Return the concrete type registered for a given abstract type and cast it to another return type.
        /// Any dependencies of this type that are also registered will be instanciated as well.
        /// </summary>
        /// <typeparam name="TReturnType">The type to whith the concrete type will be cast to</typeparam>
        /// <param name="abstractType">Abstract type to be instanciated according to the dependency injection registration</param>
        /// <returns>Concrete type registered for the given abstract type</returns>
        public static TReturnType Get<TReturnType>(Type abstractType)
        {
            return Container.GetInstance<TReturnType>(abstractType);
        }
    }
}
