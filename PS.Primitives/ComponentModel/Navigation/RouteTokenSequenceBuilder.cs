using System;
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
        private readonly List<Tuple<string, bool>> _regexPatternTokens;
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
            _regexPatternTokens = new List<Tuple<string, bool>>();
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

            var recordTokenString = record.Token.ToString();
            if (Equals(record, Wildcard))
            {
                _regexPatternTokens.Add(new Tuple<string, bool>("(?<" + recordIndex + ">\\d+?)", false));
                if (!_recursiveStart.HasValue) _recursiveStart = recordIndex - 1;
                _recursiveEnd = recordIndex;
            }
            else if (Equals(record, WildcardRecursive))
            {
                _regexPatternTokens.Add(new Tuple<string, bool>("(?<" + recordIndex + ">.+?)?", true));
                if (!_recursiveStart.HasValue) _recursiveStart = recordIndex - 1;
                _recursiveEnd = recordIndex;
            }
            else
            {
                _regexPatternTokens.Add(new Tuple<string, bool>("(?<" + recordIndex + ">" + recordTokenString + ")", false));
            }

            _regexInputTokens.Add(recordTokenString);
        }

        public RouteTokenSequence Build()
        {
            var regexPattern = string.Join("/", _regexPatternTokens.Select(t => t.Item1));
            foreach (var tokenWithOutSlot in _regexPatternTokens.Where(t => t.Item2).Select(t => t.Item1))
            {
                regexPattern = regexPattern.Replace(tokenWithOutSlot + "/", tokenWithOutSlot + "/?");
                regexPattern = regexPattern.Replace("/" + tokenWithOutSlot, "/?" + tokenWithOutSlot);
            }

            return new RouteTokenSequence(_records,
                                          _hash,
                                          _recursiveStart,
                                          _recursiveEnd,
                                          string.Join("/", _regexInputTokens),
                                          regexPattern);
        }

        #endregion
    }
}