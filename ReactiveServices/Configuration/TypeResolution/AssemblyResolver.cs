using System.Reflection;

namespace ReactiveServices.Configuration.TypeResolution
{
    public sealed class AssemblyResolver : SymbolResolver
    {
        public static Assembly Resolve(string assemblyName)
        {
            return ResolveAssembly(assemblyName);
        }
    }
}