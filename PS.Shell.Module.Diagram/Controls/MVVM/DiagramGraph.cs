using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PS.Extensions;
using PS.Graph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class DiagramGraph : IDiagramGraph
    {
        private readonly ObservableCollection<IConnector> _connectors;
        private readonly Dictionary<string, List<IConnector>> _inEdges;
        private readonly ObservableCollection<INode> _nodes;
        private readonly Dictionary<string, List<IConnector>> _outEdges;

        #region Constructors

        public DiagramGraph()
        {
            _nodes = new ObservableCollection<INode>();
            _connectors = new ObservableCollection<IConnector>();
            _inEdges = new Dictionary<string, List<IConnector>>();
            _outEdges = new Dictionary<string, List<IConnector>>();
        }

        #endregion

        #region IDiagramGraph Members

        bool IGraph.IsDirected
        {
            get { return true; }
        }

        bool IGraph.AllowParallelEdges
        {
            get { return true; }
        }

        bool IImplicitVertexSet<INode>.ContainsVertex(INode vertex)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));
            return _nodes.Contains(vertex);
        }

        bool IImplicitGraph<INode, IConnector>.IsOutEdgesEmpty(INode v)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<INode, IConnector>.OutDegree(INode v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConnector> OutEdges(INode v)
        {
            if (!_nodes.Contains(v)) throw new ArgumentException("Node does not belong to this graph");
            return _outEdges[v.Id];
        }

        bool IImplicitGraph<INode, IConnector>.TryGetOutEdges(INode v, out IEnumerable<IConnector> edges)
        {
            throw new NotImplementedException();
        }

        IConnector IImplicitGraph<INode, IConnector>.OutEdge(INode v, int index)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnector>.ContainsEdge(INode source, INode target)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnector>.TryGetEdges(INode source, INode target, out IEnumerable<IConnector> edges)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnector>.TryGetEdge(INode source, INode target, out IConnector edge)
        {
            throw new NotImplementedException();
        }

        bool IVertexSet<INode>.IsVerticesEmpty
        {
            get { return !_nodes.Any(); }
        }

        public int VertexCount
        {
            get { return _nodes.Count; }
        }

        public IEnumerable<INode> Vertices
        {
            get { return _nodes; }
        }

        bool IEdgeSet<INode, IConnector>.ContainsEdge(IConnector edge)
        {
            return _connectors.Contains(edge);
        }

        bool IEdgeSet<INode, IConnector>.IsEdgesEmpty
        {
            get { return !_connectors.Any(); }
        }

        public int EdgeCount
        {
            get { return _connectors.Count; }
        }

        public IEnumerable<IConnector> Edges
        {
            get { return _connectors; }
        }

        bool IBidirectionalIncidenceGraph<INode, IConnector>.IsInEdgesEmpty(INode v)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<INode, IConnector>.InDegree(INode v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConnector> InEdges(INode v)
        {
            if (!_nodes.Contains(v)) throw new ArgumentException("Node does not belong to this graph");
            return _inEdges[v.Id];
        }

        bool IBidirectionalIncidenceGraph<INode, IConnector>.TryGetInEdges(INode v, out IEnumerable<IConnector> edges)
        {
            throw new NotImplementedException();
        }

        IConnector IBidirectionalIncidenceGraph<INode, IConnector>.InEdge(INode v, int index)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<INode, IConnector>.Degree(INode v)
        {
            throw new NotImplementedException();
        }

        public INode Add(string id, object viewModel)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (_nodes.Any(n => n.Id.AreEqual(id))) throw new InvalidOperationException("Node with same id is already exist");

            var node = new Node(id, viewModel);
            _nodes.Add(node);
            _inEdges.Add(id, new List<IConnector>());
            _outEdges.Add(id, new List<IConnector>());
            return node;
        }

        public void Delete(IDiagramComponent component)
        {
            if (component is INode node)
            {
                if (!_nodes.Contains(node)) throw new ArgumentException("Node does not belong to this graph");

                var connectedEdges = InEdges(node).Union(OutEdges(node));
                connectedEdges.ForEach(Delete);

                _nodes.Remove(node);
                _inEdges.Remove(node.Id);
                _outEdges.Remove(node.Id);
            }

            if (component is IConnector connector)
            {
                if (!_connectors.Contains(connector)) throw new ArgumentException("Connector does not belong to this graph");

                _inEdges[connector.SourceId].Remove(connector);
                _outEdges[connector.SourceId].Remove(connector);
                _connectors.Remove(connector);
            }
        }

        public IConnector Connect(INode source, INode target, string sourceId = null, string targetId = null)
        {
            if (!_nodes.Contains(source)) throw new ArgumentException("Source node does not belong to this graph");
            if (!_nodes.Contains(target)) throw new ArgumentException("Target node does not belong to this graph");

            var connector = new Connector(Guid.NewGuid().ToString("N"),
                                          source,
                                          target,
                                          sourceId,
                                          targetId);
            _connectors.Add(connector);
            _inEdges[connector.SourceId].Add(connector);
            _outEdges[connector.SourceId].Add(connector);

            return connector;
        }

        #endregion
    }
}