using System;

namespace ReactiveServices.Configuration.TypeResolution
{
    public class AssemblyResolveException : TypeLoadException
    {
        public AssemblyResolveException(string assemblyName)
            : base(String.Format("Could resolve the assembly '{0}'", assemblyName)) { }
    }
}