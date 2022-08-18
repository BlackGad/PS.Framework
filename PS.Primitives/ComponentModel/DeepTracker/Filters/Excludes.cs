using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public static class Excludes
    {
        public static readonly ExcludeSourceType KnownSourceTypes =
            new ExcludeSourceType(true,
                                  typeof(Type),
                                  typeof(Assembly),
                                  typeof(AppDomain),
                                  //typeof(Dispatcher),
                                  typeof(Thread),
                                  typeof(Task),
                                  typeof(string));
    }
}
