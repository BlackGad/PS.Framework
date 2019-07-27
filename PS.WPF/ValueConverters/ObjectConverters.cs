using System;

namespace PS.WPF.ValueConverters
{
    public static class ObjectConverters
    {
        #region Constants

        public static readonly RelayValueConverter AssignableFrom;

        public static readonly RelayValueConverter Exist;

        public static readonly RelayValueConverter Factory;

        #endregion

        #region Constructors

        static ObjectConverters()
        {
            Exist = new RelayValueConverter((value, targetType, parameter, culture) => value != null);

            AssignableFrom = new RelayValueConverter((value, targetType, parameter, culture) =>
            {
                var valueType = value?.GetType();
                var parameterType = parameter as Type;

                if (valueType == null || parameterType == null) return false;
                return parameterType.IsAssignableFrom(valueType);
            });

            Factory = new RelayValueConverter((value, targetType, parameter, culture) =>
                                              {
                                                  var parameterType = parameter as Type;
                                                  if (parameterType == null) return null;

                                                  return parameterType.IsInstanceOfType(value);
                                              },
                                              (value, targetType, parameter, culture) =>
                                              {
                                                  var parameterType = parameter as Type;
                                                  if (parameterType == null) return null;

                                                  if (value is bool boolean && boolean)
                                                  {
                                                      if (parameterType == typeof(string)) return string.Empty;
                                                      return Activator.CreateInstance(parameterType);
                                                  }

                                                  return null;
                                              });
        }

        #endregion
    }
}