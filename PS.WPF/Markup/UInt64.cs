using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.UInt64))]
    public class UInt64 : BaseIntegerMarkupExtension<System.UInt64>
    {
        public UInt64()
        {
        }

        public UInt64(System.UInt64 value)
            : base(value)
        {
        }
    }
}
