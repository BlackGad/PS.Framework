﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using PS.Extensions;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class NodeGeometry : INodeGeometry
    {
        private double _centerX;
        private double _centerY;

        #region INodeGeometry Members

        public double CenterX
        {
            get { return _centerX; }
            set
            {
                if (_centerX.AreEqual(value)) return;
                _centerX = value;
                OnPropertyChanged();
            }
        }

        public double CenterY
        {
            get { return _centerY; }
            set
            {
                if (_centerY.AreEqual(value)) return;
                _centerY = value;
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