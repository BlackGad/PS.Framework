using System.Collections;

namespace PS.Patterns.Aware
{
    public interface IChildrenAware
    {
        #region Properties

        IList Children { get; }

        #endregion
    }
}