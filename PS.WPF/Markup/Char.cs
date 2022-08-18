using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(char))]
    public class Char : BaseIntegerMarkupExtension<char>
    {
        public Char()
        {
        }

        public Char(char value)
            : base(value)
        {
        }
    }
}
