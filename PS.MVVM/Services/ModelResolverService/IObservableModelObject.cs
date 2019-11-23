using System;
using System.Collections.Generic;
using PS.ComponentModel;

namespace PS.MVVM.Services
{
    public interface IObservableModelObject
    {
        #region Properties

        object Value { get; set; }

        #endregion

        #region Events

        event EventHandler<PropertyChangedEventArgs> ValueChanged;

        #endregion

        #region Members

        IDictionary<object, object> GetItemMetadata();
        void Reset();

        void Set(object value, Action<object, IDictionary<object, object>> activationAction = null);

        #endregion
    }
}