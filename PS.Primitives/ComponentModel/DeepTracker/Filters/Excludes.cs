using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PS.ComponentModel.DeepTracker.Filters
{
    public static class Excludes
    {
        #region Property definitions

        public static readonly ExcludePropertyType KnownPropertyTypes =
            new ExcludePropertyType(typeof(Type),
                                    typeof(Assembly),
                                    typeof(AppDomain),
                                    typeof(Thread),
                                    typeof(Task));

        #endregion
    }
}