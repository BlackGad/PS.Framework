using System.Collections.Generic;
using System.Linq;

namespace PS.Extensions
{
    public static class StackExtensions
    {
        #region Static members

        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            value = default;
            if (stack.Any())
            {
                value = stack.Peek();
                return true;
            }

            return false;
        }

        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            value = default;
            if (stack.Any())
            {
                value = stack.Pop();
                return true;
            }

            return false;
        }

        #endregion
    }
}