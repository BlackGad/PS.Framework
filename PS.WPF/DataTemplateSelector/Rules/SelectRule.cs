namespace PS.WPF.DataTemplateSelector.Rules
{
    public abstract class SelectRule
    {
        #region Properties

        public System.Windows.DataTemplate Template { get; set; }

        #endregion

        #region Members

        public abstract bool IsValid(object item);

        #endregion
    }
}