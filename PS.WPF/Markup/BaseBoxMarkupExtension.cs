using System;
using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [ContentProperty("Value")]
    public abstract class BaseBoxMarkupExtension<T> : MarkupExtension
    {
        protected BaseBoxMarkupExtension()
        {
        }

        protected BaseBoxMarkupExtension(T value)
        {
            Value = value;
        }

        [ConstructorArgument("value")]
        public T Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }
    }
}
