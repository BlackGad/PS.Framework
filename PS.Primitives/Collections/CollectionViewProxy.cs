using System;
using System.Diagnostics;

namespace PS.Collections
{
    internal sealed class CollectionViewProxy<TSource, TTarget>
    {
        private readonly CollectionView<TSource, TTarget> _list;

        public CollectionViewProxy(CollectionView<TSource, TTarget> list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TTarget[] Items
        {
            get
            {
                var array = new TTarget[_list.Count];
                _list.CopyTo(array, 0);
                return array;
            }
        }
    }
}
