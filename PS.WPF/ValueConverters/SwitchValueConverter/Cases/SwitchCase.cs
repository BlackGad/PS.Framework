using System;
using System.Windows;

namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public abstract class SwitchCase : Freezable
    {
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register(nameof(Result),
                                        typeof(object),
                                        typeof(SwitchCase),
                                        new FrameworkPropertyMetadata(default(object)));

        public object Result
        {
            get { return GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotSupportedException();
        }

        public abstract bool IsValid(object item);
    }
}
