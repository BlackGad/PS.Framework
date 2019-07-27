namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public abstract class SwitchCase
    {
        #region Properties

        public object Result { get; set; }

        #endregion

        #region Members

        public abstract bool IsValid(object item);

        #endregion
    }
}