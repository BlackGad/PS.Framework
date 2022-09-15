using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.Int64))]
    public class Int64 : BaseIntegerMarkupExtension<System.Int64>
    {
        public Int64()
        {
        }

        public Int64(System.Int64 value)
            : base(value)
        {
        }
    }
}
