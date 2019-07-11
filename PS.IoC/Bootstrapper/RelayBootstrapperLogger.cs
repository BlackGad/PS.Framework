using System;

namespace PS.IoC
{
    public class RelayBootstrapperLogger : IBootstrapperLogger
    {
        #region Properties

        public Action<string> DebugAction { get; set; }
        public Action<string> ErrorAction { get; set; }
        public Action<string> FatalAction { get; set; }
        public Action<string> InfoAction { get; set; }
        public Action<string> TraceAction { get; set; }
        public Action<string> WarnAction { get; set; }

        #endregion

        #region IBootstrapperLogger Members

        public void Debug(string message)
        {
            DebugAction?.Invoke(message);
        }

        public void Error(string message)
        {
            ErrorAction?.Invoke(message);
        }

        public void Fatal(string message)
        {
            FatalAction?.Invoke(message);
        }

        public void Info(string message)
        {
            InfoAction?.Invoke(message);
        }

        public void Trace(string message)
        {
            TraceAction?.Invoke(message);
        }

        public void Warn(string message)
        {
            WarnAction?.Invoke(message);
        }

        #endregion
    }
}