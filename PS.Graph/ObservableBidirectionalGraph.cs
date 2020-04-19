using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PS.Graph
{
    [Serializable]
    [DebuggerDisplay("VertexCount = {VertexCount}, EdgeCount = {EdgeCount}")]
    public class ObservableBidirectionalGraph<TVertex, TEdge> : BidirectionalGraph<TVertex, TEdge>,
                                                                INotifyPropertyChanged
        where TEdge : IEdge<TVertex>
    {
        private readonly ObservableCollection<TEdge> _edges;
        private readonly ObservableCollection<TVertex> _vertices;

        #region Constructors

        public ObservableBidirectionalGraph()
        {
            _vertices = new ObservableCollection<TVertex>();
            _edges = new ObservableCollection<TEdge>();
        }

        #endregion

        #region Properties

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
            base.OnCleared(obsoleteVertices, obsoleteEdges);
            OnPropertyChanged(nameof(VertexCount));
            OnPropertyChanged(nameof(EdgeCount));
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