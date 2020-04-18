using PS.Graph.Graphviz.Dot;

namespace PS.Graph.Graphviz
{
    public interface IDotEngine
    {
        #region Members

        string Run(
            GraphvizImageType imageType,
            string dot,
            string outputFileName);

        #endregion
    }
}