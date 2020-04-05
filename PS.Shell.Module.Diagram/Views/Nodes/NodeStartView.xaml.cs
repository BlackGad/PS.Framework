﻿using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Diagram.ViewModels.Nodes;

namespace PS.Shell.Module.Diagram.Views.Nodes
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<NodeStartViewModel>))]
    public partial class NodeStartView : IView<NodeStartViewModel>
    {
        #region Constructors

        public NodeStartView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<NodeStartViewModel> Members

        public NodeStartViewModel ViewModel
        {
            get { return DataContext as NodeStartViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}