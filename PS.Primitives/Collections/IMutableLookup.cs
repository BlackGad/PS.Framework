using System.Linq;

namespace PS.Collections
{
    public interface IMutableLookup<TKey, TValue> : ILookup<TKey, TValue>
    {
        void Add(TKey key, TValue value);

        void Clear();

        bool Remove(TKey key, TValue value);

        bool Remove(TKey key);
    }
}
