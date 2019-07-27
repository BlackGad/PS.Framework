using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(System.Int16))]
    public class Int16 : BaseIntegerMarkupExtension<System.Int16>
    {
        #region Constructors

        public Int16()
        {
        }

        public Int16(System.Int16 value)
            : base(value)
        {
        }

        #endregion
    }
}