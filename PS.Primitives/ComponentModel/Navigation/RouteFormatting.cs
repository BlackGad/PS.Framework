using System;

namespace PS.ComponentModel.Navigation
{
    public class RouteFormatting : IFormatProvider
    {
        #region Constants

        public const string EscapeSymbol = "@";

        public static readonly RouteFormatting Default = new RouteFormatting();

        #endregion

        #region Constructors

        public RouteFormatting()
        {
            Separator = ".";
        }

        public RouteFormatting(string separator)
        {
            if (string.IsNullOrEmpty(separator)) throw new ArgumentNullException(nameof(separator));
            Separator = separator;
        }

        #endregion

        #region Properties

        public string Separator { get; set; }

        #endregion

        #region IFormatProvider Members

        object IFormatProvider.GetFormat(Type formatType)
        {
            return formatType == typeof(Route);
        }

        #endregion
    }
}