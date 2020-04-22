using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using PS.Graph;
using PS.Graph.Graphviz;
using PS.IoC.Extensions;
using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.Shell.Module.Diagram.Models.ViewResolverService;
using PS.Shell.Module.Diagram.Test;
using PS.Shell.Module.Diagram.ViewModels;
using PS.Shell.Module.Diagram.ViewModels.Nodes;
using PS.Shell.Module.Diagram.Views;
using PS.Shell.Module.Diagram.Views.Nodes;
using PS.WPF.DataTemplate;

namespace PS.Shell.Module.Diagram
{
    public class DiagramModule : Autofac.Module
    {
        #region Override members

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.HandleActivation<IViewResolverService>(ViewResolverServiceActivation);
        }

        protected override void Load(ContainerBuilder builder)
        {
            var baseGraph = new Test.DataGraph();
            var vertex1 = new MyVertex("1");
            var vertex2 = new MyVertex("2");
            var vertex3 = new MyVertex("3");
            var vertex31 = new MyVertex("31");
            var vertex4 = new MyVertex("4");
            var vertex41 = new MyVertex("41");
            var vertex5 = new MyVertex("5");
            var vertex6 = new MyVertex("6");

            var edge12 = new MyEdge(vertex1, vertex2);
            var edge23 = new MyEdge(vertex2, vertex3);
            
            baseGraph.AddVerticesAndEdge(edge12);
            baseGraph.AddVerticesAndEdge(edge23);
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex2, vertex4));
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex3, vertex31));
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex4, vertex41));
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex31, vertex5));
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex41, vertex5));
            baseGraph.AddVerticesAndEdge(new MyEdge(vertex5, vertex6));

            var clusteredGraph = baseGraph;
            var cluster1 = clusteredGraph.AddCluster(null);
            cluster1.AddVertex(vertex1);
            var cluster2 = cluster1.AddCluster(null);
            cluster2.AddVertex(vertex41);
            cluster2.AddVerticesAndEdge(edge23);

            var cluster3 = clusteredGraph.AddCluster(null);
            cluster3.AddVertex(vertex1);

            var graphvizClusteredGraph = new GraphvizAlgorithm<MyVertex, MyEdge>(clusteredGraph);
            graphvizClusteredGraph.FormatVertex += GraphvizOnFormatVertex;
            var clusteredGraphVisual = graphvizClusteredGraph.Generate();

            var graphvizCluster1 = new GraphvizAlgorithm<MyVertex, MyEdge>(cluster1);
            graphvizCluster1.FormatVertex += GraphvizOnFormatVertex;
            var cluster1Visual = graphvizCluster1.Generate();

            var graphvizCluster2 = new GraphvizAlgorithm<MyVertex, MyEdge>(cluster2);
            graphvizCluster2.FormatVertex += GraphvizOnFormatVertex;
            var cluster2Visual = graphvizCluster2.Generate();


            builder.RegisterAssemblyTypesWithAttributes(ThisAssembly);
        }

        #endregion

        #region Event handlers

        private void GraphvizOnFormatVertex(object sender, FormatVertexEventArgs<MyVertex> args)
        {
            args.VertexFormatter.Label = args.Vertex.Name;
        }

        #endregion

        #region Members

        private void ViewResolverServiceActivation(ILifetimeScope scope, IViewResolverService service)
        {
            service.AssociateTemplate<EditorViewModel>(scope.Resolve<IDataTemplate<EditorView>>());

            service.Region(Regions.Diagram)
                   .AssociateTemplate<NodeStartViewModel>(scope.Resolve<IDataTemplate<NodeStartView>>())
                   .AssociateTemplate<NodeEndViewModel>(scope.Resolve<IDataTemplate<NodeEndView>>());
        }

        #endregion
    }
}