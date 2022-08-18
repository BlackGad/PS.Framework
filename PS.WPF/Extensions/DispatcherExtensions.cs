using System;
using System.Threading;
using System.Windows.Threading;

namespace PS.WPF.Extensions
{
    public static class DispatcherExtensions
    {
        public static void Postpone(this Dispatcher dispatcher, DispatcherPriority priority, Action action)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            if (dispatcher.HasShutdownStarted) throw new ObjectDisposedException(nameof(dispatcher));
            dispatcher.BeginInvoke(action, priority);
        }

        public static void Postpone(this Dispatcher dispatcher, Action action)
        {
            Postpone(dispatcher, DispatcherPriority.Normal, action);
        }

        public static void SafeCall(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            if (dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                if (dispatcher.HasShutdownStarted) throw new ObjectDisposedException(nameof(dispatcher));
                dispatcher.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static T SafeGet<T>(this Dispatcher dispatcher, Func<T> func)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));
            if (dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
            {
                if (dispatcher.HasShutdownStarted) throw new ObjectDisposedException(nameof(dispatcher));
                return dispatcher.Invoke(func);
            }

            return func();
        }
    }
}
