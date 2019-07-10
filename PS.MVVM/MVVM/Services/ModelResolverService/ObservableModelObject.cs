using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;

namespace PS.MVVM.Services
{
    internal class ObservableModelObject : INotifyPropertyChanged,
                                           IObservableModelObject
    {
        private object _value;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IObservableModelObject Members

        public object Value
        {
            get { return _value; }
            set
            {
                if (_value.AreEqual(value)) return;

                var newValue = value;
                var oldValue = _value;

                _value = value;

                OnPropertyChanged();
                ValueChanged?.Invoke(this, new ComponentModel.PropertyChangedEventArgs(nameof(Value), oldValue, newValue));
            }
        }

        public event EventHandler<ComponentModel.PropertyChangedEventArgs> ValueChanged;

        #endregion

        #region Members

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}