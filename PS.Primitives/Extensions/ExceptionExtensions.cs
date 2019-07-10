using System;

namespace PS.Extensions
{
    public static class ExceptionExtensions
    {
        #region Static members

        public static string GetMessage(this Exception exception)
        {
            var current = exception;
            while (current.InnerException != null)
            {
                current = current.InnerException;
            }

            return current?.Message;
        }

        #endregion
    }
}