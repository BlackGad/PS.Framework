using System;
using System.Linq;
using System.Text.RegularExpressions;
using PS.Extensions;

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
            if (source.IsEmpty() || route.IsEmpty()) return false;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            return Regex.IsMatch(source.Sequences[modeIndex].RegexInput, route.Sequences[modeIndex].RegexPattern);
        }

        public static Route CreateDynamicRoute(this Guid id)
        {
            return new Route(new[] { DynamicRoutePrefix + id.ToString("N") });
        }

        public static bool EndWith(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty() || route.IsEmpty()) return false;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            return Regex.IsMatch(source.Sequences[modeIndex].RegexInput,
                                 route.Sequences[modeIndex].RegexPattern + "$");
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
            if (source.IsEmpty() || route.IsEmpty()) return false;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            return Regex.IsMatch(source.Sequences[modeIndex].RegexInput,
                                 "^" + route.Sequences[modeIndex].RegexPattern + "$");
        }

        public static bool MatchPartially(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            while (!route.IsEmpty())
            {
                if (source.Match(route, caseSensitive)) return true;
                route = Route.Create(route.Take(route.Count - 1));
            }

            return false;
        }

        public static RouteRecursiveSplit RecursiveSplit(this Route source, Route mask, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty() || mask.IsEmpty()) return null;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return null;
            var maskTokenSequence = mask.Sequences[modeIndex];

            var match = Regex.Match(source.Sequences[modeIndex].RegexInput, maskTokenSequence.RegexPattern);
            if (!match.Success) return null;
            if (!mask.IsWild) return new RouteRecursiveSplit(Routes.Empty, Routes.Empty, source);

            // ReSharper disable PossibleInvalidOperationException
            var recursiveStart = maskTokenSequence.RecursiveStart.Value;
            var postfixStart = maskTokenSequence.RecursiveEnd.Value;
            // ReSharper restore PossibleInvalidOperationException

            var capturedGroups = match.Groups.Enumerate<Group>().Skip(1).ToList();

            var prefixTokens = string.Join("/", capturedGroups.Take(recursiveStart).Select(g => g.Value));
            var recursiveTokens = string.Join("/", capturedGroups.Skip(recursiveStart).Take(postfixStart - recursiveStart).Select(g => g.Value));

            var sourceRecursiveStart = prefixTokens.Occurrences('/') + 1;
            var sourcePostfixStart = sourceRecursiveStart + recursiveTokens.Occurrences('/') + 1;

            var prefix = source.Sub(0, sourceRecursiveStart);
            var recursive = source.Sub(sourceRecursiveStart, sourcePostfixStart - sourceRecursiveStart);
            var postfix = source.Sub(sourcePostfixStart, source.Count - sourcePostfixStart);

            return new RouteRecursiveSplit(prefix, recursive, postfix);
        }

        public static bool StartWith(this Route source, Route route, RouteCaseMode caseSensitive = RouteCaseMode.Sensitive)
        {
            if (source.IsEmpty()) return false;

            var modeIndex = (int)caseSensitive;
            if (modeIndex > 1) return false;

            return Regex.IsMatch(source.Sequences[modeIndex].RegexInput, "^" + route.Sequences[modeIndex].RegexPattern);
        }

        public static Route Sub(this Route source, int skip, int? take = null)
        {
            if (source.IsEmpty()) return Routes.Empty;
            take = take ?? source.Count - skip;
            return Route.Create(source.Skip(skip).Take(take.Value));
        }

        #endregion
    }
}