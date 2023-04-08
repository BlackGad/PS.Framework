using System;
using System.Runtime.Serialization;

namespace PS.MVVM.Components
{
    [Serializable]
    public class NotificationException : Exception
    {
        public NotificationException(string message, string title = null)
            : base(message)
        {
            Title = title;
        }

        public NotificationException(string message, Exception inner, string title = null)
            : base(message, inner)
        {
            Title = title;
        }

        protected NotificationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string Title { get; }
    }
}
