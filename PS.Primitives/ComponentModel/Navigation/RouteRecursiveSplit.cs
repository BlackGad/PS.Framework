using System;

namespace PS.ComponentModel.Navigation
{
    public class RouteRecursiveSplit
    {
        public RouteRecursiveSplit(Route prefix, Route recursive, Route postfix)
        {
            Recursive = recursive ?? throw new ArgumentNullException(nameof(recursive));
            Postfix = postfix ?? throw new ArgumentNullException(nameof(postfix));
            Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }

        public Route Postfix { get; private set; }

        public Route Prefix { get; private set; }

        public Route Recursive { get; private set; }
    }
}
