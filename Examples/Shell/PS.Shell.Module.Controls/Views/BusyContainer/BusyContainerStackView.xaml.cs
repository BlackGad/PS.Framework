﻿using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels.BusyContainer;

namespace PS.Shell.Module.Controls.Views.BusyContainer
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<BusyContainerStackViewModel>))]
    public partial class BusyContainerStackView : IView<BusyContainerStackViewModel>
    {
        public BusyContainerStackView()
        {
            InitializeComponent();
        }

        public BusyContainerStackViewModel ViewModel
        {
            get { return DataContext as BusyContainerStackViewModel; }
        }
    }
}
