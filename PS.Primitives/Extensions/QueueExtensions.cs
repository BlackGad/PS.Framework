using System.Collections.Generic;
using System.Linq;

namespace PS.Extensions
{
    public static class QueueExtensions
    {
        #region Static members

        public static bool TryDequeue<T>(this Queue<T> queue, out T value)
        {
            value = default;
            if (queue.Any())
            {
                value = queue.Dequeue();
                return true;
            }

            return false;
        }

        #endregion
    }
}