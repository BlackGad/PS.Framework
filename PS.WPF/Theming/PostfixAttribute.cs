using System;

namespace PS.WPF.Theming
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class PostfixAttribute : Attribute
    {
        #region Constructors

        public PostfixAttribute(string value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public string Value { get; }

        #endregion
    }
}