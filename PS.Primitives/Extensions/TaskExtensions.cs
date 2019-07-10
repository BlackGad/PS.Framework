using System;
using System.Threading.Tasks;

namespace PS.Extensions
{
    public static class TaskExtensions
    {
        #region Static members

        public static Task<AggregateException> SuppressError(this Task task)
        {
            return task.ContinueWith(t => t.Exception, TaskContinuationOptions.OnlyOnFaulted);
        }

        #endregion
    }
}