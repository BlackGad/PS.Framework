namespace PS.Graph.Algorithms
{
    public interface IVertexColorizerAlgorithm<in TVertex>
    {
        #region Members

        GraphColor GetVertexColor(TVertex v);

        #endregion
    }
}