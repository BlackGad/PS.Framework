using System.Collections;
using System.Collections.Generic;

namespace PS.Patterns.Aware
{
    public interface IChildrenAware
    {
        #region Properties

        IList Children { get; }

        #endregion
    }

    public interface IChildrenAware<T>
    {
        #region Properties

        IList<T> Children { get; }

        #endregion
    }
}