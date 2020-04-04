using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace PS.Shell.Module.Diagram.Controls.MVVM
{
    public class DiagramGraph : IDiagramGraph
    {
        private readonly Dictionary<string, IConnection> _connections;
        private readonly Dictionary<string, INode> _nodes;

        #region Constructors

        public DiagramGraph()
        {
            _nodes = new Dictionary<string, INode>();
            _connections = new Dictionary<string, IConnection>();
        }

        #endregion

        #region IDiagramGraph Members

        bool IGraph<INode, IConnection>.IsDirected
        {
            get { return true; }
        }

        bool IGraph<INode, IConnection>.AllowParallelEdges
        {
            get { return true; }
        }

        bool IImplicitVertexSet<INode>.ContainsVertex(INode vertex)
        {
            if (vertex == null) throw new ArgumentNullException(nameof(vertex));
            return _nodes.Values.Contains(vertex);
        }

        bool IImplicitGraph<INode, IConnection>.IsOutEdgesEmpty(INode v)
        {
            throw new NotImplementedException();
        }

        int IImplicitGraph<INode, IConnection>.OutDegree(INode v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IConnection> IImplicitGraph<INode, IConnection>.OutEdges(INode v)
        {
            throw new NotImplementedException();
        }

        bool IImplicitGraph<INode, IConnection>.TryGetOutEdges(INode v, out IEnumerable<IConnection> edges)
        {
            throw new NotImplementedException();
        }

        IConnection IImplicitGraph<INode, IConnection>.OutEdge(INode v, int index)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnection>.ContainsEdge(INode source, INode target)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnection>.TryGetEdges(INode source, INode target, out IEnumerable<IConnection> edges)
        {
            throw new NotImplementedException();
        }

        bool IIncidenceGraph<INode, IConnection>.TryGetEdge(INode source, INode target, out IConnection edge)
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
            get { return _nodes.Values; }
        }

        bool IEdgeSet<INode, IConnection>.ContainsEdge(IConnection edge)
        {
            return _connections.Values.Contains(edge);
        }

        bool IEdgeSet<INode, IConnection>.IsEdgesEmpty
        {
            get { return !_connections.Any(); }
        }

        public int EdgeCount
        {
            get { return _connections.Count; }
        }

        public IEnumerable<IConnection> Edges
        {
            get { return _connections.Values; }
        }

        bool IBidirectionalIncidenceGraph<INode, IConnection>.IsInEdgesEmpty(INode v)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<INode, IConnection>.InDegree(INode v)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IConnection> IBidirectionalIncidenceGraph<INode, IConnection>.InEdges(INode v)
        {
            throw new NotImplementedException();
        }

        bool IBidirectionalIncidenceGraph<INode, IConnection>.TryGetInEdges(INode v, out IEnumerable<IConnection> edges)
        {
            throw new NotImplementedException();
        }

        IConnection IBidirectionalIncidenceGraph<INode, IConnection>.InEdge(INode v, int index)
        {
            throw new NotImplementedException();
        }

        int IBidirectionalIncidenceGraph<INode, IConnection>.Degree(INode v)
        {
            throw new NotImplementedException();
        }

        public INode Add(string id, object viewModel)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            if (_nodes.ContainsKey(id)) throw new InvalidOperationException("Node with same id is already exist");
            var node = new Node(id, viewModel);
            _nodes.Add(id, node);
            return node;
        }

        public IConnection Connect(INode source, string sourceId, INode target, string targetId)
        {
            throw new NotImplementedException();
        }

        public void Delete(IDiagramComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}