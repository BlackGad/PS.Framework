﻿using System.Windows;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<TextBoxViewModel>))]
    public partial class TextBoxView : IView<TextBoxViewModel>
    {
        #region Constructors

        public TextBoxView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<TextBoxViewModel> Members

        public TextBoxViewModel ViewModel
        {
            get { return DataContext as TextBoxViewModel; }
        }

        #endregion

        #region Event handlers

        private void BeginEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.BeginEdit();
            ViewModel.Log.Add("Begin edit called");
        }

        private void CancelEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.CancelEdit();
            ViewModel.Log.Add("Cancel edit called");
        }

        private void EndEdit_OnClick(object sender, RoutedEventArgs e)
        {
            Control.EndEdit();
            ViewModel.Log.Add("End edit called");
        }

        #endregion
    }
}