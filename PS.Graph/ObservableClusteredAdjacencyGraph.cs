using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}, ClusterCount = {ClusterCount}")]
    public class ObservableClusteredAdjacencyGraph<TVertex, TEdge> : ClusteredAdjacencyGraph<TVertex, TEdge>,
                                                                     INotifyPropertyChanged
        where TEdge : IEdge<TVertex>
    {
        private readonly ObservableCollection<IClusteredGraph<TVertex, TEdge>> _clusters;
        private readonly ObservableCollection<TEdge> _edges;
        private readonly ObservableCollection<TVertex> _vertices;

        #region Constructors

        public ObservableClusteredAdjacencyGraph()
        {
            _vertices = new ObservableCollection<TVertex>();
            _edges = new ObservableCollection<TEdge>();
            _clusters = new ObservableCollection<IClusteredGraph<TVertex, TEdge>>();
        }

        #endregion

        #region Properties

        public override IEnumerable<IClusteredGraph<TVertex, TEdge>> Clusters
        {
            get { return _clusters; }
        }

        public override IEnumerable<TEdge> Edges
        {
            get { return _edges; }
        }

        public override IEnumerable<TVertex> Vertices
        {
            get { return _vertices; }
        }

        #endregion

        #region Override members

        protected override void OnClusterAdded(IClusteredGraph<TVertex, TEdge> args)
        {
            _clusters.Add(args);
            base.OnClusterAdded(args);
            OnPropertyChanged(nameof(ClusterCount));
        }

        protected override void OnClusterRemoved(IClusteredGraph<TVertex, TEdge> args)
        {
            _clusters.Remove(args);
            base.OnClusterRemoved(args);
            OnPropertyChanged(nameof(ClusterCount));
        }

        protected override void OnVertexAdded(TVertex args)
        {
            _vertices.Add(args);
            base.OnVertexAdded(args);
            OnPropertyChanged(nameof(VertexCount));
        }

        protected override void OnVertexRemoved(TVertex args)
        {
            _vertices.Remove(args);
            base.OnVertexRemoved(args);
            OnPropertyChanged(nameof(VertexCount));
        }

        protected override void OnEdgeAdded(TEdge args)
        {
            _edges.Add(args);
            base.OnEdgeAdded(args);
            OnPropertyChanged(nameof(EdgeCount));
        }

        protected override void OnEdgeRemoved(TEdge args)
        {
            _edges.Remove(args);
            base.OnEdgeRemoved(args);
            OnPropertyChanged(nameof(EdgeCount));
        }

        protected override void OnCleared(IReadOnlyList<TVertex> obsoleteVertices, IReadOnlyList<TEdge> obsoleteEdges)
        {
            _vertices.Clear();
            _edges.Clear();
            _clusters.Clear();
            base.OnCleared(obsoleteVertices, obsoleteEdges);
            OnPropertyChanged(nameof(VertexCount));
            OnPropertyChanged(nameof(EdgeCount));
            OnPropertyChanged(nameof(ClusterCount));
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Members

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}