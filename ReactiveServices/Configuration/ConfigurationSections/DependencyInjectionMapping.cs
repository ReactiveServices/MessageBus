using System;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    public class DependencyInjectionMapping
    {
        public Type AbstractType { get; set; }
        public Type ConcreteType { get; set; }
        public Lifestyle Lifestyle { get; set; }
    }
}
