using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(sbyte))]
    public class SByte : BaseIntegerMarkupExtension<sbyte>
    {
        public SByte()
        {
        }

        public SByte(sbyte value)
            : base(value)
        {
        }
    }
}
