using System;
using System.Linq;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.ValueConverters
{
    public static class BooleanConverters
    {
        #region Constants

        public const string Maybe = nameof(Maybe);
        public const string No = nameof(No);
        public const string Yes = nameof(Yes);

        public static readonly RelayValueConverter Inverted;

        public static readonly RelayMultiValueConverter MultiAnd;
        public static readonly RelayMultiValueConverter MultiOr;

        public static readonly RelayValueConverter ToVisibility;
        public static readonly RelayValueConverter ToVisibilityInverted;

        public static readonly RelayValueConverter ToYesNo;

        #endregion

        #region Constructors

        static BooleanConverters()
        {
            ToYesNo = new RelayValueConverter((value, targetType, parameter, culture) =>
                                              {
                                                  if (value is bool boolean) return boolean ? Yes : No;
                                                  if (value == null) return Maybe;

                                                  throw new NotSupportedException();
                                              },
                                              (value, targetType, parameter, culture) =>
                                              {
                                                  if (value is string stringValue)
                                                  {
                                                      if (string.Equals(Yes, stringValue)) return true;
                                                      if (string.Equals(No, stringValue)) return false;
                                                      if (string.Equals(Maybe, stringValue)) return null;
                                                  }

                                                  if (value == null) return null;

                                                  throw new NotSupportedException();
                                              });

            ToVisibilityInverted = new RelayValueConverter((value, targetType, parameter, culture) =>
                                                           {
                                                               if (value is bool boolean)
                                                               {
                                                                   return boolean ? Visibility.Collapsed : Visibility.Visible;
                                                               }

                                                               if (value == null) return Visibility.Visible;

                                                               throw new NotSupportedException();
                                                           },
                                                           (value, targetType, parameter, culture) =>
                                                           {
                                                               if (value is Visibility castedValue)
                                                               {
                                                                   switch (castedValue)
                                                                   {
                                                                       case Visibility.Visible:
                                                                           return false;
                                                                       case Visibility.Hidden:
                                                                       case Visibility.Collapsed:
                                                                           return true;
                                                                   }
                                                               }

                                                               throw new NotSupportedException();
                                                           });

            ToVisibility = new RelayValueConverter((value, targetType, parameter, culture) =>
                                                   {
                                                       if (value is bool boolean)
                                                       {
                                                           return boolean ? Visibility.Visible : Visibility.Collapsed;
                                                       }

                                                       if (value == null) return Visibility.Collapsed;

                                                       throw new NotSupportedException();
                                                   },
                                                   (value, targetType, parameter, culture) =>
                                                   {
                                                       if (value is Visibility castedValue)
                                                       {
                                                           switch (castedValue)
                                                           {
                                                               case Visibility.Visible:
                                                                   return true;
                                                               case Visibility.Hidden:
                                                               case Visibility.Collapsed:
                                                                   return false;
                                                           }
                                                       }

                                                       throw new NotSupportedException();
                                                   });

            Inverted = new RelayValueConverter((value, targetType, parameter, culture) =>
                                               {
                                                   if (value is bool boolean)
                                                   {
                                                       return !boolean;
                                                   }

                                                   throw new NotSupportedException();
                                               },
                                               (value, targetType, parameter, culture) =>
                                               {
                                                   if (value is bool boolean)
                                                   {
                                                       return !boolean;
                                                   }

                                                   throw new NotSupportedException();
                                               });

            MultiAnd = new RelayMultiValueConverter((values, type, parameter, culture) => values.Enumerate<bool>().All(value => value));
            MultiOr = new RelayMultiValueConverter((values, type, parameter, culture) => values.Enumerate<bool>().Any(value => value));
        }

        #endregion
    }
}