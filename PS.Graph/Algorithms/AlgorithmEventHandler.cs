using System;

namespace PS.Graph.Algorithms
{
    public delegate void AlgorithmEventHandler<in TGraph>(
        IAlgorithm<TGraph> sender,
        EventArgs e);
}