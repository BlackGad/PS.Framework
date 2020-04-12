using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PS.ComponentModel.Navigation.Extensions
{
    public static class RouteExtensions
    {
        #region Constants

        private const string DynamicRoutePrefix = "__dynamic_";

        #endregion

        #region Static members

        public static bool Contains(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty()) return route.IsEmpty();
            if (route.IsEmpty()) return true;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, route.Sequences[modeIndex].RegexPattern);
            return match.Success;
        }

        public static Route CreateDynamicRoute(this Guid id)
        {
            return new Route(new[] { DynamicRoutePrefix + id.ToString("N") });
        }

        public static bool EndWith(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty()) return route.IsEmpty();
            if (route.IsEmpty()) return true;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, route.Sequences[modeIndex].RegexPattern + "$");
            return match.Success;
        }

        public static bool IsDynamic(this Route source)
        {
            return source.Any(p => p.IsDynamicRoute());
        }

        public static bool IsDynamicRoute(this string source)
        {
            return (source ?? string.Empty).StartsWith(DynamicRoutePrefix);
        }

        public static bool IsEmpty(this Route source)
        {
            return (source?.Count ?? 0) == 0;
        }

        public static bool Match(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty()) return route.IsEmpty();
            if (route.IsEmpty()) return false;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, "^" + route.Sequences[modeIndex].RegexPattern + "$");
            return match.Success;
        }

        public static bool MatchPartially(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            while (!route.IsEmpty())
            {
                if (source.Match(route, caseSensitive)) return true;
                route = new Route(route.Take(route.Count - 1));
            }

            return false;
        }

        public static Route Select(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (route.IsEmpty() && source.IsEmpty()) return Routes.Empty;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return null;

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, route.Sequences[modeIndex].RegexPattern);
            if (!match.Success) return null;

            var tokensToSkip = RouteTokenSequenceBuilder.TokenCountInSequence(source.Sequences[modeIndex].RegexInput.Substring(0, match.Index));
            var tokensToTake = RouteTokenSequenceBuilder.TokenCountInSequence(match.Value);

            return source.Sub(tokensToSkip, tokensToTake);
        }

        public static bool StartWith(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty()) return route.IsEmpty();
            if (route.IsEmpty()) return true;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, "^" + route.Sequences[modeIndex].RegexPattern);
            return match.Success;
        }

        public static Route Sub(this Route source, int skip, int? take = null)
        {
            if (source.IsEmpty()) return Routes.Empty;
            take = take ?? source.Count - skip;
            return new Route(source.Skip(skip).Take(take.Value));
        }

        #endregion
    }
}