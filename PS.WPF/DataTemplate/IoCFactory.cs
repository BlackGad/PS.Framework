using System;

namespace PS.WPF.DataTemplate
{
    public static class IoCFactory
    {
        #region Static members

        public static object GetInstance(Type serviceType, string key)
        {
            var factoryFunc = _factoryFunc;
            if (factoryFunc == null) throw new InvalidOperationException("Prism factory not registered");
            return factoryFunc(serviceType, key);
        }

        public static T GetInstance<T>(Type serviceType, string key)
        {
            return (T)GetInstance(serviceType, key);
        }

        public static void Register(Func<Type, string, object> factoryFunc)
        {
            _factoryFunc = factoryFunc ?? throw new ArgumentNullException(nameof(factoryFunc));
        }

        #endregion

        private static Func<Type, string, object> _factoryFunc;
    }
}