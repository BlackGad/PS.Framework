using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public class EqualTo : SwitchCase
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                                        typeof(object),
                                        typeof(EqualTo),
                                        new FrameworkPropertyMetadata(default(object)));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public override bool IsValid(object item)
        {
            return Value.AreEqual(item);
        }
    }
}
