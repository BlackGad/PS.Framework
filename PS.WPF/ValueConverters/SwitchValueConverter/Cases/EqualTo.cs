using PS.Extensions;

namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public class EqualTo : SwitchCase
    {
        public object Value { get; set; }

        public override bool IsValid(object item)
        {
            return Value.AreEqual(item);
        }
    }
}
