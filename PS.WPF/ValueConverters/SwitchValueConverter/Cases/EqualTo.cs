using PS.Extensions;

namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public class EqualTo : SwitchCase
    {
        #region Properties

        public object Value { get; set; }

        #endregion

        #region Override members

        public override bool IsValid(object item)
        {
            return Value.AreEqual(item);
        }

        #endregion
    }
}