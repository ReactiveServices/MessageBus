using System;

namespace ReactiveServices.Configuration
{
    public enum Lifestyle
    {
        Singleton,
        Transient
    };

    public sealed class DependencyContainer : IDisposable
    {
        public DependencyContainer()
        {
            InternalContainer = new SimpleInjector.Container();
        }

        public SimpleInjector.Container InternalContainer { get; private set; }

        public void Register<TAbstractType>(Func<TAbstractType> instanceCreator, Lifestyle lifestyle) where TAbstractType : class
        {
            InternalContainer.Register(instanceCreator, ConvertTo(lifestyle));
        }

        public void Register<TAbstractType, TConcreteType>()
            where TAbstractType : class
            where TConcreteType : class, TAbstractType
        {
            InternalContainer.Register<TAbstractType, TConcreteType>();
        }

        public void Register<TAbstractType, TConcreteType>(Lifestyle lifestyle)
            where TAbstractType : class
            where TConcreteType : class, TAbstractType
        {
            InternalContainer.Register<TAbstractType, TConcreteType>(ConvertTo(lifestyle));
        }

        public void Register(Type abstractType, Type concreteType, Lifestyle lifestyle)
        {
            InternalContainer.Register(abstractType, concreteType, ConvertTo(lifestyle));
        }

        public void Register<TAbstractType>(Lifestyle lifestyle)
            where TAbstractType : class
        {
            InternalContainer.Register<TAbstractType>(ConvertTo(lifestyle));
        }

        public TAbstractType GetInstance<TAbstractType>() where TAbstractType : class
        {
            return InternalContainer.GetInstance<TAbstractType>();
        }

        public TAbstractType GetInstance<TAbstractType>(Type concreteType)
        {
            return (TAbstractType)InternalContainer.GetInstance(concreteType);
        }

        /// <remarks>
        /// Attention: Verify() will instanciate all registered objects, but will not dispose them
        /// </remarks>
        public void Verify()
        {
            InternalContainer.Verify();
        }

        private static SimpleInjector.Lifestyle ConvertTo(Lifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    return SimpleInjector.Lifestyle.Singleton;
                case Lifestyle.Transient:
                    return SimpleInjector.Lifestyle.Transient;
                default:
                    throw new NotImplementedException(String.Format("Register not implemented for {0} lifestyle", lifestyle));
            }
        }

        public void Dispose()
        {
            InternalContainer = null;
        }
    }
}
