namespace PS.ComponentModel
{
    public class PropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        public PropertyChangedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object NewValue { get; }

        public object OldValue { get; }
    }
}
