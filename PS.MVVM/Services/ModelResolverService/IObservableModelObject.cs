using System;

namespace PS.MVVM.Services
{
    public interface IObservableModelObject
    {
        #region Properties

        object Value { get; set; }

        #endregion

        #region Events

        event EventHandler<ComponentModel.PropertyChangedEventArgs> ValueChanged;

        #endregion
    }
}