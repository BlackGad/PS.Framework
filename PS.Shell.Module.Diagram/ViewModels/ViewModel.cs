using PS.Shell.Module.Diagram.Controls.MVVM;
using PS.Shell.Module.Diagram.ViewModels.Nodes;
using PS.Shell.Module.Diagram.Views.Nodes;

namespace PS.Shell.Module.Diagram.ViewModels
{
    internal class ViewModel
    {
        #region Constructors

        public ViewModel()
        {
            var nodeViewModel1 = new NodeStartViewModel();
            var nodeViewModel2 = new NodeStartViewModel();

            var node1 = Graph.Add(nodeViewModel1.Id, nodeViewModel1);
            var node2 = Graph.Add(nodeViewModel2.Id, nodeViewModel2);

            node1.Geometry.CenterX = 1;

            var connection = Graph.Connect(node1, null, node2, null);

            Graph.Delete(connection);
            Graph.Delete(node1);
        }

        #endregion

        #region Properties

        public IDiagramGraph Graph { get; set; }

        #endregion
    }
}