using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public abstract class CommandServiceComponent : INotifyPropertyChanged,
                                                    IIDAware<object>,
                                                    ITitleAware,
                                                    IDescriptionAware,
                                                    IOrderAware,
                                                    IVisibilityAware
    {
        private string _description;
        private object _id;
        private bool _isVisible;
        private int _order;
        private string _title;

        protected CommandServiceComponent()
        {
            Id = Guid.NewGuid().ToString("N");
            IsVisible = true;
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description.AreEqual(value)) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        public object Id
        {
            get { return _id; }
            set
            {
                if (_id.AreEqual(value)) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Order
        {
            get { return _order; }
            set
            {
                if (_order.AreEqual(value)) return;
                _order = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title.AreEqual(value)) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible.AreEqual(value)) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
