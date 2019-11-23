using System.Collections.Generic;
using System.Linq;
using PS.Extensions;

namespace PS.ComponentModel.Navigation
{
    internal class RouteTokenSequenceBuilder
    {
        #region Constants

        private static readonly RouteToken Wildcard;
        private static readonly RouteToken WildcardRecursive;

        #endregion

        private readonly List<RouteToken> _records;
        private readonly List<string> _regexInputTokens;
        private readonly List<string> _regexPatternTokens;
        private int _hash;
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
        }

        #endregion

        #region Members

        public void Add(RouteToken record)
        {
            _records.Add(record);

            var recordIndex = _records.Count;
            _hash = _hash.MergeHash(record.Hash);

            if (Equals(record, Wildcard))
            {
                _regexPatternTokens.Add("(;\\d+;)");
                if (!_recursiveStart.HasValue) _recursiveStart = recordIndex - 1;
                _recursiveEnd = recordIndex;
            }
            else if (Equals(record, WildcardRecursive))
            {
                _regexPatternTokens.Add("(;\\d+;)*?");
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
                                          _hash,
                                          _recursiveStart,
                                          _recursiveEnd,
                                          regexInput,
                                          regexPattern);
        }

        #endregion
    }
}