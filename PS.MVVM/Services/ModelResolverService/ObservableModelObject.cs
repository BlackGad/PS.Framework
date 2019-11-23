using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Data;
using PS.Extensions;
using PropertyChangedEventArgs = PS.ComponentModel.PropertyChangedEventArgs;

namespace PS.MVVM.Services
{
    internal class ObservableModelObject : INotifyPropertyChanged,
                                           IObservableModelObject
    {
        private ObjectsStorage<object, object> _metadata;
        private object _value;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IObservableModelObject Members

        public event EventHandler<PropertyChangedEventArgs> ValueChanged;

        public IDictionary<object, object> GetItemMetadata()
        {
            return _metadata ?? (_metadata = new ObjectsStorage<object, object>());
        }

        public void Reset()
        {
            Set(null, null);
        }

        public void Set(object value, Action<object, IDictionary<object, object>> activationAction)
        {
            if (_value.AreEqual(value)) return;

            _metadata = null;

            var newValue = value;
            var oldValue = _value;

            _value = value;
            activationAction?.Invoke(_value, GetItemMetadata());

            OnPropertyChanged(nameof(Value));
            ValueChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value), oldValue, newValue));
        }

        public object Value
        {
            get { return _value; }
            set { Set(value, null); }
        }

        #endregion

        #region Members

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}