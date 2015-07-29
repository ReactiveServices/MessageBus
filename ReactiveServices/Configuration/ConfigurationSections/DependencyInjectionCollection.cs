using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace ReactiveServices.Configuration.ConfigurationSections
{
    [ConfigurationCollection(typeof(DependencyInjectionElement), CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
    public class DependencyInjectionCollection : ConfigurationElementCollection, IEnumerable<DependencyInjectionElement>
    {
        internal const string ItemPropertyName = "DependencyInjection";

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
            return ((DependencyInjectionElement)element).AbstractType;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DependencyInjectionElement();
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        public new IEnumerator<DependencyInjectionElement> GetEnumerator()
        {
            return new DependencyInjectionCollectionEnumerator(this);
        }

        public sealed class DependencyInjectionCollectionEnumerator : IEnumerator<DependencyInjectionElement>
        {
            public DependencyInjectionCollectionEnumerator(DependencyInjectionCollection collection)
            {
                Collection = collection;
            }

            private readonly DependencyInjectionCollection Collection;

            public DependencyInjectionElement Current
            {
                get { return (this as IEnumerator).Current as DependencyInjectionElement; }
            }

            public void Dispose()
            {
            }

            private int EnumeratorIndex = -1;

            object IEnumerator.Current
            {
                get { return Collection.BaseGet(EnumeratorIndex); }
            }

            public bool MoveNext()
            {
                if (EnumeratorIndex < Collection.Count-1)
                {
                    EnumeratorIndex++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                EnumeratorIndex = -1;
            }
        }
    }  
}
