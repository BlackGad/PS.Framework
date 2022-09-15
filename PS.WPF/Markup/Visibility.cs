using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.Windows.Visibility))]
    public class Visibility : BaseBoxMarkupExtension<System.Windows.Visibility>
    {
        public Visibility()
        {
        }

        public Visibility(System.Windows.Visibility value)
            : base(value)
        {
        }
    }
}
