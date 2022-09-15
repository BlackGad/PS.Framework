using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;

namespace PS
{
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (field.AreEqual(value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
