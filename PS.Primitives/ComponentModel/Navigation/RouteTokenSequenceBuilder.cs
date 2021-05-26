using System.Collections.Generic;
using System.Linq;
using PS.Extensions;

namespace PS.ComponentModel.Navigation
{
    internal class RouteTokenSequenceBuilder
    {
        #region Constants

        public static readonly char EscapeSymbol = ';';
        private static readonly RouteToken Wildcard;
        private static readonly RouteToken WildcardRecursive;

        #endregion

        #region Static members

        public static int TokenCountInSequence(string sequence)
        {
            return sequence.Count(c => c == EscapeSymbol) / 2;
        }

        #endregion

        private readonly List<int> _hashes;
        private readonly List<RouteToken> _records;
        private readonly List<string> _regexInputTokens;
        private readonly List<string> _regexPatternTokens;
        private int? _recursiveEnd;
        private int? _recursiveStart;

        #region Constructors

        static RouteTokenSequenceBuilder()
        {
            Wildcard = Route.EnsureTokenCached(Route.Wildcard);
            WildcardRecursive = Route.EnsureTokenCached(Route.WildcardRecursive);
        }

        public RouteTokenSequenceBuilder()
        {
            _regexPatternTokens = new List<string>();
            _regexInputTokens = new List<string>();
            _records = new List<RouteToken>();
            _hashes = new List<int>();
        }

        #endregion

        #region Members

        public void Add(RouteToken record)
        {
            _records.Add(record);

            var recordIndex = _records.Count;
            var hashForSize = _hashes.LastOrDefault().MergeHash(record.Hash);
            _hashes.Add(hashForSize);

            if (Equals(record, Wildcard))
            {
                _regexPatternTokens.Add($"({EscapeSymbol}\\d+{EscapeSymbol})");
                if (!_recursiveStart.HasValue) _recursiveStart = recordIndex - 1;
                _recursiveEnd = recordIndex;
            }
            else if (Equals(record, WildcardRecursive))
            {
                _regexPatternTokens.Add($"({EscapeSymbol}\\d+{EscapeSymbol})*?");
                if (!_recursiveStart.HasValue) _recursiveStart = recordIndex - 1;
                _recursiveEnd = recordIndex;
            }
            else
            {
                _regexPatternTokens.Add($"({record.TokenString})");
            }

            _regexInputTokens.Add(record.TokenString);
        }

        public RouteTokenSequence Build()
        {
            var regexPattern = _regexPatternTokens.Aggregate(string.Empty, (agg, t) => agg + t);
            var regexInput = _regexInputTokens.Aggregate(string.Empty, (agg, t) => agg + t);

            return new RouteTokenSequence(_records,
                                          _hashes.ToArray(),
                                          _recursiveStart,
                                          _recursiveEnd,
                                          regexInput,
                                          regexPattern);
        }

        #endregion
    }
}