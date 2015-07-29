using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NLog;

namespace ReactiveServices.Configuration.TypeResolution
{
    public abstract class SymbolResolver
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected static readonly List<Assembly> ResolvedAssemblies = new List<Assembly>();

        protected static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return ResolveAssembly(args.Name);
        }

        protected static Assembly ResolveAssembly(string desiredAssemblyFullName)
        {
            // Try load assembly by full name
            var assembly = LoadAssemblyByFullName(desiredAssemblyFullName);
            try
            {
                if (assembly != null)
                    return assembly;

                // Decompose desired assembly full name
                var desiredAssemblyParts = desiredAssemblyFullName.Split(',');

                if (desiredAssemblyParts.Length != 4)
                    throw new AssemblyResolveException(desiredAssemblyFullName);

                var desiredAssemblySingleName = desiredAssemblyParts[0];
                var desiredAssemblyVersion = desiredAssemblyParts[1];
                var desiredAssemblyCulture = desiredAssemblyParts[2];
                var desiredAssemblyPublicKey = desiredAssemblyParts[3];
                var desiredAssemblyFileName = String.Format("{0}.dll", desiredAssemblySingleName);

                // Try to load assembly name that matches the desired assembly single name (as dll)
                var loadedAssemblyName = LoadAssemblyName(desiredAssemblyFileName);

                if (loadedAssemblyName == null)
                {
                    // Try to load assembly name that matches the desired assembly single name (as exe)
                    desiredAssemblyFileName = String.Format("{0}.exe", desiredAssemblySingleName);
                    loadedAssemblyName = LoadAssemblyName(desiredAssemblyFileName);
                }

                // If could not find an assembly with the desired file name, throw
                if (loadedAssemblyName == null)
                    throw new AssemblyResolveException(desiredAssemblyFullName);

                // Decompose loaded assembly full name
                var loadedAssemblyFullName = loadedAssemblyName.FullName;
                var loadedAssemblyParts = loadedAssemblyFullName.Split(',');
                var loadedAssemblyVersion = loadedAssemblyParts[1];
                var loadedAssemblyCulture = loadedAssemblyParts[2];
                var loadedAssemblyPublicKey = loadedAssemblyParts[3];

                // If there is no autoversion wild card, throw
                var wildcardPosition = desiredAssemblyVersion.IndexOf('*');
                if (wildcardPosition == -1)
                    throw new AssemblyResolveException(desiredAssemblyFullName);

                // If the stable versions does not match, throw
                var assemblyStableVersion = loadedAssemblyVersion.Substring(0, wildcardPosition);
                var desiredStableVersion = desiredAssemblyVersion.Substring(0, wildcardPosition);
                if (assemblyStableVersion != desiredStableVersion)
                    throw new AssemblyResolveException(desiredAssemblyFullName);

                // Load the assembly by file name only
                assembly = LoadAssemblyByFileName(desiredAssemblyFileName);

                // If the assembly with the same stable version does not have matching cultures and public keys, throw
                if ((assembly == null) || (desiredAssemblyCulture != loadedAssemblyCulture) ||
                    (desiredAssemblyPublicKey != loadedAssemblyPublicKey))
                    throw new AssemblyResolveException(desiredAssemblyFullName);

                return assembly;
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error resolving assembly '{0}'!", desiredAssemblyFullName), e);
                throw;
            }
            finally
            {
                if (assembly != null)
                    ResolvedAssemblies.Add(assembly);
            }
        }

        private static AssemblyName LoadAssemblyName(string desiredAssemblyFileName)
        {
            try
            {
                return AssemblyName.GetAssemblyName(desiredAssemblyFileName);
            }
            catch (IOException)
            {
                return null;
            }
        }

        private static Assembly LoadAssemblyByFullName(string desiredAssemblyFullName)
        {
            try
            {
                return Assembly.LoadFrom(desiredAssemblyFullName);
            }
            catch
            {
                return null;
            }
        }

        private static Assembly LoadAssemblyByFileName(string desiredAssemblyFileName)
        {
            try
            {
                return Assembly.LoadFrom(desiredAssemblyFileName);
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
}
