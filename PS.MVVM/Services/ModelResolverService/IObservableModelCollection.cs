using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PS.MVVM.Services
{
    public interface IObservableModelCollection : IList,
                                                  INotifyCollectionChanged
    {
        void Add(object item, Action<object, IDictionary<object, object>> activationAction);

        IDictionary<object, object> GetItemMetadata(object item);

        IEnumerable Query(Func<object, IReadOnlyDictionary<object, object>, bool> predicate);
    }
}
