using System.Collections;
using System.Collections.Generic;

namespace PS.Patterns.Aware
{
    public interface IChildrenAware
    {
        IList Children { get; }
    }

    public interface IChildrenAware<T>
    {
        IList<T> Children { get; }
    }
}
