using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactiveServices.Configuration.TypeResolution
{
    public class TypeResolveException : TypeLoadException
    {
        public TypeResolveException(string typeName)
            : base(String.Format("Could not resolve the type '{0}'", typeName)) { }
        public TypeResolveException(string typeName,IEnumerable<string> resolvedAssemblyNames)
            : base(String.Format("Could not resolve the type '{0}'. Resolved assemblies: {1}", typeName, resolvedAssemblyNames.Aggregate((a, b) => String.Format("{0},{1}", a, b)))) { }
    }
}