using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class Boolean : BaseBoxMarkupExtension<bool>
    {
        public Boolean()
        {
        }

        public Boolean(bool value)
            : base(value)
        {
        }
    }
}
