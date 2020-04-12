﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class NodeVisual : INodeVisual
    {
        private bool _isSelected;

        #region INodeVisual Members

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected.AreEqual(value)) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Members

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}