using System;
using System.Windows;
using System.Windows.Threading;
using PS.IoC.Attributes;
using PS.MVVM.Services;
using PS.WPF.Extensions;

namespace PS.Commander.Models.BroadcastService
{
    [DependencyRegisterAsInterface(typeof(IBroadcastService))]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    internal class BroadcastService : MVVM.Services.BroadcastService
    {
        private readonly Dispatcher _dispatcher;

        public BroadcastService()
        {
            _dispatcher = Application.Current.Dispatcher;
        }

        protected override void CallDelegate<T>(Delegate @delegate, T args)
        {
            _dispatcher.Postpone(() => @delegate.DynamicInvoke(args));
        }
    }
}
