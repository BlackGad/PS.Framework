using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.UInt16))]
    public class UInt16 : BaseIntegerMarkupExtension<System.UInt16>
    {
        #region Constructors

        public UInt16()
        {
        }

        public UInt16(System.UInt16 value)
            : base(value)
        {
        }

        #endregion
    }
}