namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public abstract class SwitchCase
    {
        public object Result { get; set; }

        public abstract bool IsValid(object item);
    }
}
