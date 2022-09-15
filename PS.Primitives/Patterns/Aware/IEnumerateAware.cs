using System.Collections.Generic;

namespace PS.Patterns.Aware
{
    public interface IEnumerateAware<out TValue>
    {
        IEnumerable<TValue> Enumerate();
    }
}
