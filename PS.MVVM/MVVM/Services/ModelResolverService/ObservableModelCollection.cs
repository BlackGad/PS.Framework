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

        #region Constructors

        public ObservableModelCollection()
        {
            _metadata = new ConditionalWeakTable<object, ObjectsStorage<object, object>>();
        }

        #endregion

        #region IObservableModelCollection Members

        public IDictionary<object, object> GetItemMetadata(object item)
        {
            if (Contains(item))
            {
                return _metadata.GetOrCreateValue(item);
            }

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

        #endregion
    }
}