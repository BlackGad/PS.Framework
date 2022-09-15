using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.Int32))]
    public class Int32 : BaseIntegerMarkupExtension<System.Int32>
    {
        public Int32()
        {
        }

        public Int32(System.Int32 value)
            : base(value)
        {
        }
    }
}
