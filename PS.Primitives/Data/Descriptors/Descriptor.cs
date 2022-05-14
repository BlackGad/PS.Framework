using System;
using System.Linq;
using PS.Extensions;

namespace PS.Data.Descriptors
{
    [Serializable]
    public abstract class Descriptor
    {
        #region Override members

        public sealed override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Descriptor)obj);
        }

        public override string ToString()
        {
            var type = GetType();
            var properties = type.GetProperties()
                                 .Where(p => p.CanRead)
                                 .Select(p => $"{p.Name}={p.GetValue(this) ?? "<null>"}");
            return $"{type.Name}: {string.Join(", ", properties)}";
        }

        public sealed override int GetHashCode()
        {
            return GetKey().GetHash();
        }

        #endregion

        #region Members

        public abstract string GetKey();

        private bool Equals(Descriptor other)
        {
            return GetHashCode().AreEqual(other.GetHash());
        }

        #endregion
    }

    [Serializable]
    public class Descriptor<T> : Descriptor
    {
        private string _key;

        #region Constructors

        public Descriptor(object id = null)
        {
            Id = id;

            _key = GetType().GetShortAssemblyQualifiedName();
            if (id == null)
            {
                _key += "8B92F6B981D0417882F2B8D9D454E07A";
            }
            else
            {
                _key += id;
            }
        }

        #endregion

        #region Properties

        public object Id { get; }

        #endregion

        #region Override members

        public override string ToString()
        {
            return $"Descriptor: <{typeof(T).Name}>: {Id ?? "<null>"}";
        }

        public override string GetKey()
        {
            return _key;
        }

        #endregion
    }
}