using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(byte))]
    public class Byte : BaseIntegerMarkupExtension<byte>
    {
        #region Constructors

        public Byte()
        {
        }

        public Byte(byte value)
            : base(value)
        {
        }

        #endregion
    }
}