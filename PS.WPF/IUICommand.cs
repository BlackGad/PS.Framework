﻿using System.ComponentModel;
using System.Windows.Input;
using PS.Patterns.Aware;

namespace PS.WPF
{
    public interface IUICommand : ICommand,
                                  IDescriptionAware,
                                  IGroupAware,
                                  IOrderAware,
                                  ITitleAware,
                                  IIconAware,
                                  IColorAware,
                                  IVisibilityAware,
                                  INotifyPropertyChanged
    {
        #region Members

        void RaiseCanExecuteChanged();

        #endregion
    }
}