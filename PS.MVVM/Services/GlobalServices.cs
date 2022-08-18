using System;
using PS.Data;

namespace PS.MVVM.Services
{
    public static class GlobalServices
    {
        private static readonly ObjectsStorage<Type, object> GlobalServicesStorage;

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

        static GlobalServices()
        {
            GlobalServicesStorage = new ObjectsStorage<Type, object>();
        }
    }
}
