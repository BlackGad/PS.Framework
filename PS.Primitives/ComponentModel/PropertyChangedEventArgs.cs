namespace PS.ComponentModel
{
    public class PropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        #region Constructors

        public PropertyChangedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        public object NewValue { get; }
        public object OldValue { get; }

        #endregion
    }
}