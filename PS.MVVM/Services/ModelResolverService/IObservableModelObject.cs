using System;
using System.Collections.Generic;
using PS.ComponentModel;

namespace PS.MVVM.Services
{
    public interface IObservableModelObject
    {
        object Value { get; set; }

        event EventHandler<PropertyChangedEventArgs> ValueChanged;

        IDictionary<object, object> GetItemMetadata();

        void Reset();

        void Set(object value, Action<object, IDictionary<object, object>> activationAction = null);
    }
}
