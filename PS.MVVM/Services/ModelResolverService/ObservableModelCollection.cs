using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using PS.Data;

namespace PS.MVVM.Services
{
    internal class ObservableModelCollection : ObservableCollection<object>,
                                               IObservableModelCollection
    {
        private readonly ConditionalWeakTable<object, ObjectsStorage<object, object>> _metadata;

        public ObservableModelCollection()
        {
            _metadata = new ConditionalWeakTable<object, ObjectsStorage<object, object>>();
        }

        public void Add(object item, Action<object, IDictionary<object, object>> activationAction)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            var metadata = _metadata.GetOrCreateValue(item);
            activationAction?.Invoke(item, metadata);
            Add(item);
        }

        public IDictionary<object, object> GetItemMetadata(object item)
        {
            //If item in collection get existing metadata or create new
            if (Contains(item))
            {
                return _metadata.GetOrCreateValue(item);
            }

            //Item is already not in collection, try to get existing metadata table from cached
            if (_metadata.TryGetValue(item, out var result)) return result;

            throw new ArgumentException("Item is not in collection");
        }

        public IEnumerable Query(Func<object, IReadOnlyDictionary<object, object>, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            foreach (var item in this)
            {
                if (predicate(item, (IReadOnlyDictionary<object, object>)GetItemMetadata(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
