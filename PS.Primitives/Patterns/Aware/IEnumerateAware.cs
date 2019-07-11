using System.Collections.Generic;

namespace PS.Patterns.Aware
{
    public interface IEnumerateAware<out TValue>
    {
        #region Members

        IEnumerable<TValue> Enumerate();

        #endregion
    }
}