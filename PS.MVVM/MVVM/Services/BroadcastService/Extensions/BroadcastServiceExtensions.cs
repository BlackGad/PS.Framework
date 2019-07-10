using System;
using System.Threading.Tasks;

namespace PS.MVVM.Services.Extensions
{
    public static class BroadcastServiceExtensions
    {
        #region Static members

        public static Task Broadcast<T>(this IBroadcastService service, T args)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Broadcast(typeof(T), args);
        }

        public static Task Broadcast<T, TArgument>(this IBroadcastService service, TArgument args)
            where T : TArgument
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Broadcast(typeof(T), args);
        }

        public static bool Subscribe<T>(this IBroadcastService service, Action<T> subscribeAction)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Subscribe(typeof(T), subscribeAction);
        }

        public static bool Subscribe<T, TArgument>(this IBroadcastService service, Action<TArgument> subscribeAction)
            where T : TArgument
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Subscribe(typeof(T), subscribeAction);
        }

        public static bool Unsubscribe<T>(this IBroadcastService service, Action<T> unsubscribeAction)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Unsubscribe(typeof(T), unsubscribeAction);
        }

        public static bool Unsubscribe<T, TArgument>(this IBroadcastService service, Action<TArgument> unsubscribeAction)
            where T : TArgument
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            return service.Unsubscribe(typeof(T), unsubscribeAction);
        }

        #endregion
    }
}