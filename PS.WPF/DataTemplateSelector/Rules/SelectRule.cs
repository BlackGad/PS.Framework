namespace PS.WPF.DataTemplateSelector.Rules
{
    public abstract class SelectRule
    {
        public System.Windows.DataTemplate Template { get; set; }

        public abstract bool IsValid(object item);
    }
}
