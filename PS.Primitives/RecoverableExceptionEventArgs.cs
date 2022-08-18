using System;

namespace PS
{
    public class RecoverableExceptionEventArgs : ExceptionEventArgs
    {
        public RecoverableExceptionEventArgs(Exception exception, bool handled) : base(exception)
        {
            Handled = handled;
        }

        public bool Handled { get; set; }
    }
}
