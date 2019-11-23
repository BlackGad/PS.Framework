using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PS.Extensions;

namespace PS.Data
{
    public class ObjectsStorage<TKey, TObject> : IReadOnlyDictionary<TKey, TObject>,
                                                 IDictionary<TKey, TObject>,
                                                 IDisposable
    {
        private readonly Func<TKey, TObject> _factory;
        private readonly ConcurrentDictionary<TKey, Lazy<TObject>> _storage;

        #region Constructors

        public ObjectsStorage()
            : this(null)
        {
        }

        public ObjectsStorage(Func<TKey, TObject> factory)
        {
            _factory = factory ?? (key => Activator.CreateInstance<TObject>());
            _storage = new ConcurrentDictionary<TKey, Lazy<TObject>>();
        }

        #endregion

        #region IDictionary<TKey,TObject> Members

        void IDictionary<TKey, TObject>.Add(TKey key, TObject value)
        {
            this[key] = value;
        }

        bool IDictionary<TKey, TObject>.Remove(TKey key)
        {
            return !Remove(key).AreEqual(default(TObject));
        }

        ICollection<TObject> IDictionary<TKey, TObject>.Values
        {
            get { return _storage.Values.Select(v => v.Value).ToArray(); }
        }

        ICollection<TKey> IDictionary<TKey, TObject>.Keys
        {
            get { return _storage.Keys; }
        }

        bool ICollection<KeyValuePair<TKey, TObject>>.Remove(KeyValuePair<TKey, TObject> item)
        {
            return !Remove(item.Key).AreEqual(default(TObject));
        }

        bool ICollection<KeyValuePair<TKey, TObject>>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<KeyValuePair<TKey, TObject>>.Add(KeyValuePair<TKey, TObject> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            _storage.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TObject>>.Contains(KeyValuePair<TKey, TObject> item)
        {
            return ((IDictionary<TKey, TObject>)_storage).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TObject>>.CopyTo(KeyValuePair<TKey, TObject>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TObject>)_storage).CopyTo(array, arrayIndex);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var value in this)
            {
                if (value.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        #endregion

        #region IReadOnlyDictionary<TKey,TObject> Members

        public IEnumerator<KeyValuePair<TKey, TObject>> GetEnumerator()
        {
            return _storage.Select(p => new KeyValuePair<TKey, TObject>(p.Key, p.Value.Value))
                           .ToList()
                           .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(TKey key)
        {
            return _storage.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TObject value)
        {
            var result = _storage.TryGetValue(key, out var lazyValue);
            if (result && lazyValue != null)
            {
                value = lazyValue.Value;
            }
            else
            {
                value = default;
            }

            return result;
        }

        public TObject this[TKey key]
        {
            get { return _storage.GetOrAdd(key, k => new Lazy<TObject>(() => Create(k), true)).Value; }
            set { _storage.AddOrUpdate(key, k => new Lazy<TObject>(() => value, true), (k, existing) => new Lazy<TObject>(() => value, true)); }
        }

        public IEnumerable<TKey> Keys
        {
            get { return _storage.Keys; }
        }

        public IEnumerable<TObject> Values
        {
            get { return _storage.Values.Select(v => v.Value).ToArray(); }
        }

        public int Count
        {
            get { return _storage.Count; }
        }

        #endregion

        #region Members

        public TObject Create(TKey key)
        {
            return _factory(key);
        }

        public TObject Remove(TKey key)
        {
            if (_storage.TryRemove(key, out var obj)) return obj.Value;
            return default;
        }

        #endregion
    }
}