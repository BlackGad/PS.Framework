using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Command;
using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.Shell.Module.Diagram.ViewModels.Nodes;

namespace PS.Shell.Module.Diagram.ViewModels
{
    [DependencyRegisterAsSelf]
    public class EditorViewModel : DependencyObject,
                                   IViewModel
    {
        #region Property definitions

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register(nameof(Graph),
                                        typeof(IDiagramGraph),
                                        typeof(EditorViewModel),
                                        new FrameworkPropertyMetadata(default(IDiagramGraph)));

        #endregion

        #region Constructors

        public EditorViewModel()
        {
            AddCommand = new RelayCommand(Add);
            RemoveCommand = new RelayCommand(Remove);
            Graph = new DiagramGraph();
            Graph.Add(Guid.NewGuid().ToString("N"), new NodeStartViewModel());
            Graph.Add(Guid.NewGuid().ToString("N"), new NodeEndViewModel());
        }

        private void Remove()
        {
            var node = Graph.Vertices.LastOrDefault();
            Graph.Delete(node);
        }

        #endregion

        #region Properties

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public IDiagramGraph Graph
        {
            get { return (IDiagramGraph)GetValue(GraphProperty); }
            set { SetValue(GraphProperty, value); }
        }

        #endregion

        #region Members

        private void Add()
        {
            Graph.Add(Guid.NewGuid().ToString("N"), new NodeEndViewModel());
        }

        #endregion
    }
}