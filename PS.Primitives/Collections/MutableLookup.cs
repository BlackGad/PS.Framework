using System;
using System.Collections.Generic;
using System.Linq;
using PS.Data;
using PS.Extensions;

namespace PS.Collections
{
    public class MutableLookup<TKey, TValue> : IMutableLookup<TKey, TValue>
    {
        private readonly ObjectsStorage<TKey, List<TValue>> _contents;

        #region Constructors

        public MutableLookup(IEnumerable<IGrouping<TKey, TValue>> groups)
        {
            foreach (var group in groups.Enumerate())
            {
                foreach (var value in group)
                {
                    Add(group.Key, value);
                }
            }
        }

        public MutableLookup(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            foreach (var pair in pairs.Enumerate())
            {
                Add(pair.Key, pair.Value);
            }
        }

        public MutableLookup()
        {
            _contents = new ObjectsStorage<TKey, List<TValue>>();
        }

        #endregion

        #region Properties

        public IEnumerable<TKey> Keys
        {
            get { return _contents.Keys; }
        }

        #endregion

        #region IMutableLookup<TKey,TValue> Members

        public int Count
        {
            get { return _contents.Count; }
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get { return _contents[key]; }
        }

        public void Add(TKey key, TValue value)
        {
            _contents[key].Add(value);
        }

        public bool Remove(TKey key, TValue value)
        {
            var result = _contents[key].Remove(value);
            if (result && !_contents[key].Any()) _contents.Remove(key);
            return result;
        }

        public bool Remove(TKey key)
        {
            return _contents.Remove(key) != null;
        }

        public void Clear()
        {
            _contents.Clear();
        }

        public bool Contains(TKey key)
        {
            return _contents.ContainsKey(key);
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
        {
            return _contents.Select(p => new Grouping
            {
                Key = p.Key,
                Source = this
            }).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Members

        public void Add(TKey key, params TValue[] values)
        {
            _contents[key].AddRange(values);
        }

        public void Add(IEnumerable<Tuple<TKey, TValue>> items)
        {
            foreach (var item in items)
            {
                Add(item.Item1, item.Item2);
            }
        }

        #endregion

        #region Nested type: Grouping

        private class Grouping : IGrouping<TKey, TValue>
        {
            public MutableLookup<TKey, TValue> Source;

            #region IGrouping<TKey,TValue> Members

            public TKey Key { get; set; }

            public IEnumerator<TValue> GetEnumerator()
            {
                if (Source.Contains(Key))
                {
                    foreach (var item in Source[Key])
                    {
                        yield return item;
                    }
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion
    }
}