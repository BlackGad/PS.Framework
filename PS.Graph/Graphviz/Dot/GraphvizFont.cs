using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Graphviz.Dot
{
    public sealed class GraphvizFont
    {
        #region Constructors

        public GraphvizFont(string name, float sizeInPoints)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Requires(sizeInPoints > 0);

            Name = name;
            SizeInPoints = sizeInPoints;
        }

        #endregion

        #region Properties

        public string Name { get; }
        public float SizeInPoints { get; }

        #endregion
    }
}