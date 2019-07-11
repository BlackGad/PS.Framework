using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(decimal))]
    public class Decimal : BaseIntegerMarkupExtension<decimal>
    {
        #region Constructors

        public Decimal()
        {
        }

        public Decimal(decimal value)
            : base(value)
        {
        }

        #endregion
    }
}