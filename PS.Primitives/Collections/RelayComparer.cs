using System;
using System.Collections;

namespace PS.Collections
{
    public class RelayComparer : IComparer
    {
        private readonly Func<object, object, int> _comparer;

        public RelayComparer(Func<object, object, int> comparer = null)
        {
            _comparer = comparer ?? ((x, y) => 0);
        }

        public int Compare(object x, object y)
        {
            return _comparer(x, y);
        }
    }
}
