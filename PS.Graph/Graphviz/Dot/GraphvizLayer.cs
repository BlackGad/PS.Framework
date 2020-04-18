using System;
using System.Diagnostics.Contracts;

namespace PS.Graph.Graphviz.Dot
{
    public class GraphvizLayer
    {
        private string _name;

        #region Constructors

        public GraphvizLayer(string name)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));

            _name = name;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set
            {
                Contract.Requires(!String.IsNullOrEmpty(value));
                _name = value;
            }
        }

        #endregion
    }
}