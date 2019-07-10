using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PS.Extensions;

namespace PS.Threading
{
    public static class TaskHelper
    {
        #region Constants

        private static readonly IReadOnlyList<MethodInfo> StaticMethods;

        #endregion

        #region Static members

        public static Task FromException(Exception exception)
        {
            var internalMethod = StaticMethods.First(m => m.Name.AreEqual(nameof(FromException)) && !m.IsGenericMethod);
            return (Task)internalMethod.Invoke(null, new object[] { exception });
        }

        public static Task<TResult> FromException<TResult>(Exception exception)
        {
            var internalMethod = StaticMethods.First(m => m.Name.AreEqual(nameof(FromException)) && m.IsGenericMethod);
            var genericMethod = internalMethod.MakeGenericMethod(typeof(TResult));
            return (Task<TResult>)genericMethod.Invoke(null, new object[] { exception });
        }

        #endregion

        #region Constructors

        static TaskHelper()
        {
            var taskType = typeof(Task);
            StaticMethods = taskType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                                    .Enumerate()
                                    .ToArray();
        }

        #endregion
    }
}