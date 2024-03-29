﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PS.ComponentModel.Navigation
{
    internal class RouteTokenSequence : IReadOnlyList<RouteToken>,
                                        IFormattable
    {
        public static bool operator ==(RouteTokenSequence left, RouteTokenSequence right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RouteTokenSequence left, RouteTokenSequence right)
        {
            return !Equals(left, right);
        }

        public readonly IReadOnlyList<int> Hashes;
        private readonly List<RouteToken> _records;

        public RouteTokenSequence(List<RouteToken> records, IReadOnlyList<int> hashes, int? recursiveStart, int? recursiveEnd, string regexInput, string regexPattern)
        {
            _records = records;
            Hashes = hashes;

            RecursiveStart = recursiveStart;
            RecursiveEnd = recursiveEnd;
            RegexPattern = regexPattern;
            RegexInput = regexInput;
        }

        public int? RecursiveEnd { get; }

        public int? RecursiveStart { get; }

        public string RegexInput { get; }

        public string RegexPattern { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RouteTokenSequence)obj);
        }

        public override int GetHashCode()
        {
            return Hashes.LastOrDefault();
        }

        public override string ToString()
        {
            return ToString(null);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            var routeFormatProvider = formatProvider as RouteFormatting ?? RouteFormatting.Default;
            return string.Join(routeFormatProvider.Separator,
                               _records.Select(p => p.Value.Replace(routeFormatProvider.Separator,
                                                                    RouteFormatting.EscapeSymbol + routeFormatProvider.Separator)));
        }

        public int Count
        {
            get { return _records.Count; }
        }

        public RouteToken this[int index]
        {
            get { return _records[index]; }
        }

        public IEnumerator<RouteToken> GetEnumerator()
        {
            return _records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ((IFormattable)this).ToString(null, formatProvider);
        }

        protected bool Equals(RouteTokenSequence other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}
