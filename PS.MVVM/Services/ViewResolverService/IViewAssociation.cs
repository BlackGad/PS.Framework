﻿using System;
using System.Collections.Generic;

namespace PS.MVVM.Services
{
    public interface IViewAssociation
    {
        Type ConsumerServiceType { get; }

        int Depth { get; }

        IReadOnlyDictionary<object, object> Metadata { get; }

        object Payload { get; }

        object Region { get; }

        Type ViewModelType { get; }
    }
}
