using System;
using PS.Data;

namespace PS.MVVM.Services
{
    public static class GlobalServices
    {
        #region Constants

        private static readonly ObjectsStorage<Type, object> GlobalServicesStorage;

        #endregion

        #region Static members

        public static TResolverService Get<TResolverService>()
            where TResolverService : class
        {
            GlobalServicesStorage.TryGetValue(typeof(TResolverService), out var result);
            return result as TResolverService;
        }

        public static void Register<TResolverService>(TResolverService service)
            where TResolverService : class
        {
            GlobalServicesStorage[typeof(TResolverService)] = service;
        }

        #endregion

        #region Constructors

        static GlobalServices()
        {
            GlobalServicesStorage = new ObjectsStorage<Type, object>();
        }

        #endregion
    }
}