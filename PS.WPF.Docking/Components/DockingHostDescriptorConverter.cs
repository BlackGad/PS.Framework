using System;
using System.ComponentModel;
using System.Globalization;

namespace PS.WPF.Docking.Components
{
    public class DockingHostDescriptorConverter : TypeConverter
    {
        #region Override members

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return new DockingHostDescriptor();
            }

            if (value is string stringValue)
            {
                return new DockingHostDescriptor(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return value is DockingHostDescriptor descriptor && destinationType == typeof(string)
                ? descriptor.Id
                : base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}