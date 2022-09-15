using System;

namespace PS
{
    public class ExceptionEventArgs : System.EventArgs
    {
        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}