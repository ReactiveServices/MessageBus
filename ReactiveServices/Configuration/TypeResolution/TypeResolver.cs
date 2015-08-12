using System;
using System.Linq;
using System.Reflection;
using NLog;
using PostSharp.Patterns.Diagnostics;

namespace ReactiveServices.Configuration.TypeResolution
{
    public sealed class TypeResolver : SymbolResolver
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [Log]
        [LogException]
        public static Type Resolve(string typeName)
        {
            try
            {
                Type resolvedType;
                // If the typeName is not an assembly qualified name, load it normally
                if (!IsAssemblyQualifiedName(typeName))
                {
                    resolvedType = Type.GetType(typeName);
                    if (resolvedType != null)
                        return resolvedType;
                }

                // If the typeName is an assembly qualified name, load it with custom assembly resolution
                AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
                try
                {
                    // Try to load type with the given assembly qualified name
                    resolvedType = Type.GetType(typeName);
                    // Try to load type from previously resolved assemblies
                    if (resolvedType == null)
                    {
                        foreach (var resolvedAssembly in ResolvedAssemblies)
                        {
                            resolvedType = resolvedAssembly.GetType(typeName);
                            if (resolvedType != null)
                                break;
                        }
                    }
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
                }

                if (resolvedType != null)
                    return resolvedType;

                var resolvedAssemblies = ResolvedAssemblies.Select(a => a.GetName().Name);

                throw new TypeResolveException(typeName, resolvedAssemblies);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error resolving type '{0}'!", typeName);
                throw;
            }
        }

        private static bool IsAssemblyQualifiedName(string typeName)
        {
            return typeName.Contains(",");
        }
    }
}