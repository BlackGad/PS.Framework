using System;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class TypeConverters
    {
        #region Constants

        public static readonly RelayValueConverter DisplayName;
        public static readonly RelayValueConverter EnumValues;

        #endregion

        #region Constructors

        static TypeConverters()
        {
            DisplayName = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (value is Type type) return type.GetDisplayName();
                return null;
            });

            EnumValues = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                if (value is Type type && type.IsEnum)
                {
                    return type.GetEnumValues();
                }

                return null;
            });
        }

        #endregion
    }
}