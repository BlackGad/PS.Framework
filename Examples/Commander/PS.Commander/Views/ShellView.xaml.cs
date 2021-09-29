﻿using System.Windows;
using PS.Commander.ViewModels;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Commander.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ShellViewModel>))]
    public partial class ShellView : IView<ShellViewModel>
    {
        #region Constructors

        public ShellView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ShellViewModel> Members

        public ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }

        #endregion

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotificationException("Hi!", "With title");
        }
    }
}