using System;

namespace PS.WPF.Theming
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class PostfixAttribute : Attribute
    {
        public PostfixAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
