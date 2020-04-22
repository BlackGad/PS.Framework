using System;
using PS.Extensions;
using PS.Graph;
using PS.Patterns.Aware;

namespace PS.Shell.Module.Diagram.Test
{
    public class MyEdge : IIDAware,
                          IEdge<MyVertex>,
                          IEquatable<MyEdge>
    {
        #region Constructors

        public MyEdge(MyVertex source, MyVertex target)
        {
            Id = Guid.NewGuid().ToString("N");
            Source = source;
            Target = target;
        }

        #endregion

        #region Override members

        public override string ToString()
        {
            return $"{Source} -> {Target}";
        }

        #endregion

        #region IEdge<Node> Members

        public MyVertex Source { get; }
        public MyVertex Target { get; }

        #endregion

        #region IEquatable<MyEdge> Members

        public bool Equals(MyEdge other)
        {
            if (other == null) return false;

            if (!Id.AreEqual(other.Id)) return false;
            if (!Source.AreEqual(other.Source)) return false;
            if (!Target.AreEqual(other.Target)) return false;

            return true;
        }

        #endregion

        #region IIDAware Members

        public string Id { get; set; }

        #endregion
    }
}