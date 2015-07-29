using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    [ConfigurationCollection(typeof(AssemblyElement), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        internal const string ItemPropertyName = "ReferenceAssembly";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return ItemPropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return (elementName == ItemPropertyName);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).AssemblyName;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }  
}
