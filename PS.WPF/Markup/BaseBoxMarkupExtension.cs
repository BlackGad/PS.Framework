using System;
using System.Windows.Markup;

namespace PS.WPF.Markup
{
    [ContentProperty("Value")]
    public abstract class BaseBoxMarkupExtension<T> : MarkupExtension
    {
        #region Constructors

        protected BaseBoxMarkupExtension()
        {
        }

        protected BaseBoxMarkupExtension(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        [ConstructorArgument("value")]
        public T Value { get; set; }

        #endregion

        #region Override members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Value;
        }

        #endregion
    }
}