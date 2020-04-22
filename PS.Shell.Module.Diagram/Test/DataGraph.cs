using System.Collections.Generic;
using PS.Graph;

namespace PS.Shell.Module.Diagram.Test
{
    internal class DataGraph : ClusteredBidirectionalGraph<MyVertex, MyEdge>
    {
        public DataGraph()
        {
            //TODO: Create VisualGraph for diagram where clusters collapsed into vertices
            //TODO: Vertices must have additional info to fill pin information when it connected inside cluster
        }
        protected override void OnCleared(IReadOnlyList<MyVertex> obsoleteVertices, IReadOnlyList<MyEdge> obsoleteEdges)
        {
            base.OnCleared(obsoleteVertices, obsoleteEdges);
        }

        protected override void OnEdgeAdded(MyEdge args)
        {
            base.OnEdgeAdded(args);
        }

        protected override void OnEdgeRemoved(MyEdge args)
        {
            base.OnEdgeRemoved(args);
        }

        protected override void OnVertexAdded(MyVertex args)
        {
            base.OnVertexAdded(args);
        }

        protected override void OnVertexRemoved(MyVertex args)
        {
            base.OnVertexRemoved(args);
        }

        protected override void OnClusterAdded(IClusteredGraph<MyVertex, MyEdge> args)
        {
            base.OnClusterAdded(args);
        }

        protected override void OnClusterRemoved(IClusteredGraph<MyVertex, MyEdge> args)
        {
            base.OnClusterRemoved(args);
        }
    }

    //TODO: TemplateSelector for clusters as well
    public abstract class DataCluster : ClusteredBidirectionalGraph<MyVertex, MyEdge>
    {
        
    }
}