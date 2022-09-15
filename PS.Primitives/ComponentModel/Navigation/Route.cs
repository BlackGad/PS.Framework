using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PS.Extensions;

namespace PS.ComponentModel.Navigation
{
    [Serializable]
    public class Route : IFormattable,
                         ISerializable,
                         IXmlSerializable,
                         IEquatable<Route>,
                         IReadOnlyList<string>,
                         ICloneable
    {
        internal const string Wildcard = "*";
        internal const string WildcardRecursive = "**";

        private static readonly Dictionary<int, RouteToken> RoutesCache;

        public static Route Create(params object[] parts)
        {
            return new Route(parts.Enumerate().SelectMany(ExpandPart).ToArray());
        }

        public static Route CreateFromUri(object uri)
        {
            if (uri == null) return Routes.Empty;
            string uriString;
            if (uri is Uri)
            {
                uriString = ((Uri)uri).OriginalString;
            }
            else
            {
                uriString = uri.ToString();
            }

            Uri uriObject;
            if (Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                uriObject = new Uri(uriString, UriKind.Absolute);
            }
            else
            {
                var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), uriString);
                absolutePath = Path.GetFullPath(absolutePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                try
                {
                    uriObject = new Uri(absolutePath, UriKind.Absolute);
                }
                catch (Exception e)
                {
                    throw new NotSupportedException($"{uri} is not well formatted uri", e);
                }
            }

            return Parse(uriObject.ToString(),
                         new RouteFormatting
                         {
                             Separator = "/"
                         });
        }

        public static bool operator ==(Route left, Route right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Route left, Route right)
        {
            return !Equals(left, right);
        }

        public static Route Parse(string source, RouteFormatting formatting = null)
        {
            if (string.IsNullOrEmpty(source)) return Routes.Empty;

            formatting = formatting ?? RouteFormatting.Default;
            var separator = formatting.Separator;

            var temporaryString = '\a'.ToString();
            var unescapedSource = source.Replace(RouteFormatting.EscapeSymbol + separator, temporaryString);
            var parts = unescapedSource.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(p => p.Replace(temporaryString, separator))
                                       .ToArray();
            return new Route(parts);
        }

        internal static RouteToken EnsureTokenCached(string part)
        {
            var partHash = part.GetHashCode();

            lock (RoutesCache)
            {
                return RoutesCache.GetOrAdd(partHash, h => new RouteToken(part, h, RoutesCache.Count));
            }
        }

        private static IEnumerable<string> ExpandPart(object part)
        {
            if (part == null) return Enumerable.Empty<string>();
            if (part is string s) return new[] { s };
            if (part is Route route) return route;
            if (part is IEnumerable) return part.Enumerate().SelectMany(ExpandPart);
            return new[] { part.ToString() };
        }

        private static Route Merge(Route route, Route with)
        {
            route.Sequences = with.Sequences;
            return route;
        }

        private readonly int _hashCode;

        internal RouteTokenSequence[] Sequences;

        static Route()
        {
            RoutesCache = new Dictionary<int, RouteToken>();
        }

        private Route()
        {
        }

        public Route(IEnumerable<string> parts)
        {
            var sequenceBuilder = new RouteTokenSequenceBuilder();
            var sequenceInsensibleBuilder = new RouteTokenSequenceBuilder();

            foreach (var part in parts.Enumerate())
            {
                if (string.IsNullOrEmpty(part)) continue;

                sequenceBuilder.Add(EnsureTokenCached(part));
                sequenceInsensibleBuilder.Add(EnsureTokenCached(part.ToUpperInvariant()));
            }

            var sensibleSequence = sequenceBuilder.Build();
            Sequences = new[]
            {
                sensibleSequence,
                sequenceInsensibleBuilder.Build()
            };

            _hashCode = sensibleSequence.GetHashCode();
            IsWild = sensibleSequence.RecursiveStart.HasValue;
            Count = sensibleSequence.Count;
        }

        protected Route(SerializationInfo info, StreamingContext context)
        {
            var route = new Route((string[])info.GetValue(nameof(Route), typeof(string[])));
            Merge(this, route);
        }

        public bool IsWild { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Route)obj);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override string ToString()
        {
            return ToString(null);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool Equals(Route other)
        {
            return Equals(_hashCode, other?._hashCode);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            var routeFormatProvider = formatProvider as RouteFormatting ?? RouteFormatting.Default;
            return string.Join(routeFormatProvider.Separator,
                               this.Select(p => p.Replace(routeFormatProvider.Separator, RouteFormatting.EscapeSymbol + routeFormatProvider.Separator)));
        }

        public int Count { get; }

        public string this[int index]
        {
            get { return Sequences[(int)RouteCaseMode.Sensitive][index].Value; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Sequences[(int)RouteCaseMode.Sensitive].Select(h => h.Value).GetEnumerator();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Route), this.ToArray(), typeof(string[]));
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();
            var parts = new List<string>();
            while (reader.IsStartElement())
            {
                parts.Add(reader.ReadElementString("Part"));
            }

            Merge(this, new Route(parts.ToArray()));
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (var part in this)
            {
                writer.WriteElementString("Part", string.Empty, part);
            }
        }

        public Route Clone()
        {
            return Merge(new Route(), this);
        }

        public IReadOnlyList<int> GetHashCodes(RouteCaseMode mode)
        {
            return Sequences[(int)mode].Hashes;
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ((IFormattable)this).ToString(null, formatProvider);
        }
    }
}
