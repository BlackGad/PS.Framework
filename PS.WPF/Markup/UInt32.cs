using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.UInt32))]
    public class UInt32 : BaseIntegerMarkupExtension<System.UInt32>
    {
        public UInt32()
        {
        }

        public UInt32(System.UInt32 value)
            : base(value)
        {
        }
    }
}
