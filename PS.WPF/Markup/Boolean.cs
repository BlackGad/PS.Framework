using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class Boolean : BaseBoxMarkupExtension<bool>
    {
        #region Constructors

        public Boolean()
        {
        }

        public Boolean(bool value)
            : base(value)
        {
        }

        #endregion
    }
}