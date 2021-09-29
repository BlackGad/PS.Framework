using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;
using PS.Patterns.Command;

namespace PS.WPF.Patterns.Command
{
    public class RelayUICommand : RelayUICommand<object>
    {
        #region Constructors

        public RelayUICommand(Action executeAction = null, Func<bool> canExecutePredicate = null)
            :
            base(o => executeAction?.Invoke(), o => canExecutePredicate?.Invoke() ?? true)
        {
        }

        #endregion
    }

    public class RelayUICommand<T> : RelayCommand<T>,
                                     IUICommand
    {
        private object _color;
        private string _description;
        private string _group;
        private object _icon;
        private bool _isVisible;
        private int _order;
        private string _title;

        #region Constructors

        public RelayUICommand(Action<T> executeAction = null, Func<T, bool> canExecutePredicate = null)
            :
            base(executeAction, canExecutePredicate)
        {
            IsVisible = true;
        }

        #endregion

        #region IUICommand Members

        public event PropertyChangedEventHandler PropertyChanged;

        public object Color
        {
            get { return _color; }
            set
            {
                if (_color.AreEqual(value)) return;

                _color = value;
                OnPropertyChangedAuto();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.Equals(_description, value)) return;

                _description = value;
                OnPropertyChangedAuto();
            }
        }

        public string Group
        {
            get { return _group; }
            set
            {
                if (_group.AreEqual(value)) return;
                _group = value;
                OnPropertyChangedAuto();
            }
        }

        public object Icon
        {
            get { return _icon; }
            set
            {
                if (_icon.AreEqual(value)) return;
                _icon = value;
                OnPropertyChangedAuto();
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible.AreEqual(value)) return;

                _isVisible = value;
                OnPropertyChangedAuto();
            }
        }

        public int Order
        {
            get { return _order; }
            set
            {
                if (_order.AreEqual(value)) return;
                _order = value;
                OnPropertyChangedAuto();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.Equals(_title, value)) return;

                _title = value;
                OnPropertyChangedAuto();
            }
        }

        #endregion

        #region Members

        protected void OnPropertyChanged(string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChangedAuto([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}