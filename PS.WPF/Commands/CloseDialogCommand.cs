using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using PS.Extensions;
using PS.WPF.Extensions;

namespace PS.WPF.Commands
{
    public class CloseDialogCommand : IUICommand
    {
        private object _color;
        private string _description;
        private bool? _dialogResult;
        private string _group;
        private object _icon;
        private bool _isVisible;
        private int _order;
        private string _title;

        #region Constructors

        public CloseDialogCommand()
        {
            Title = "Close";
            IsVisible = true;
        }

        #endregion

        #region Properties

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set
            {
                if (_dialogResult.AreEqual(value)) return;
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region IUICommand Members

        public event EventHandler CanExecuteChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public object Color
        {
            get { return _color; }
            set
            {
                if (_color.AreEqual(value)) return;

                _color = value;
                OnPropertyChanged();
            }
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

        public string Group
        {
            get { return _group; }
            set
            {
                if (_group.AreEqual(value)) return;
                _group = value;
                OnPropertyChanged();
            }
        }

        public object Icon
        {
            get { return _icon; }
            set
            {
                if (_icon.AreEqual(value)) return;
                _icon = value;
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

        public bool CanExecute(object parameter)
        {
            var element = parameter as FrameworkElement;
            if (element == null) return false;
            var window = Window.GetWindow(element);
            if (window == null) return false;

            return true;
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;

            var window = Window.GetWindow((FrameworkElement)parameter);
            if (window == null) return;

            if (window.IsModal()) window.DialogResult = DialogResult;
            window.Close();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Members

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}