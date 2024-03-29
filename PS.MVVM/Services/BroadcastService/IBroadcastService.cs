﻿using System;
using System.Threading.Tasks;

namespace PS.MVVM.Services
{
    public interface IBroadcastService
    {
        Task Broadcast(Type eventType, object args);

        bool Subscribe(Type eventType, Delegate subscribeAction);

        bool Unsubscribe(Type eventType, Delegate unsubscribeAction);
    }
}
