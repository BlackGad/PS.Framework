using System;

namespace PS.ComponentModel.Navigation
{
    public class RouteFormatting : IFormatProvider
    {
        public const string EscapeSymbol = "@";

        public static readonly RouteFormatting Default = new RouteFormatting();

        public RouteFormatting()
        {
            Separator = ".";
        }

        public RouteFormatting(string separator)
        {
            if (string.IsNullOrEmpty(separator)) throw new ArgumentNullException(nameof(separator));
            Separator = separator;
        }

        public string Separator { get; set; }

        object IFormatProvider.GetFormat(Type formatType)
        {
            return formatType == typeof(Route);
        }
    }
}
