using System;
using System.ComponentModel;
using PS.Extensions;

namespace PS.ComponentModel
{
    public class PropertyReference
    {
        #region Static members

        public static bool operator ==(PropertyReference left, PropertyReference right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PropertyReference left, PropertyReference right)
        {
            return !Equals(left, right);
        }

        #endregion

        private readonly object _descriptor;
        private readonly int _hashCode;
        private readonly object _source;

        #region Constructors

        public PropertyReference(object source, PropertyDescriptor descriptor, bool weakSource = true, bool weakDescriptor = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

            _source = weakSource ? new WeakReference(source) : source;
            _descriptor = weakDescriptor ? (object)new WeakReference(descriptor) : descriptor;

            _hashCode = source.GetHash().MergeHash(descriptor.Name.GetHash());

            Name = descriptor.Name;
            PropertyType = descriptor.PropertyType;
            IsReadOnly = descriptor.IsReadOnly;
            SupportsChangeEvents = descriptor.SupportsChangeEvents;
            SourceType = source.GetType();
        }

        #endregion

        #region Properties

        public bool IsReadOnly { get; }
        public string Name { get; }
        public Type PropertyType { get; }
        public Type SourceType { get; }
        public bool SupportsChangeEvents { get; }

        #endregion

        #region Override members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertyReference)obj);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        #endregion

        #region Members

        public object GetSource()
        {
            var source = _source;

            if (_source is WeakReference weakSource)
            {
                source = weakSource.Target;
            }

            return source;
        }

        public bool TryAddValueChanged(EventHandler handler)
        {
            if (!SupportsChangeEvents) return false;

            if (!GetPair(out var source, out var descriptor)) return false;

            descriptor.AddValueChanged(source, handler);
            return true;
        }

        public bool TryGetDescriptor(out PropertyDescriptor descriptor)
        {
            if (!GetPair(out _, out descriptor)) return false;
            return true;
        }

        public bool TryGetValue(out object value)
        {
            value = null;

            if (!GetPair(out var source, out var descriptor)) return false;
            if (typeof(Type).IsAssignableFrom(SourceType)) return false;

            value = descriptor.GetValue(source);
            return true;
        }

        public bool TryRemoveValueChanged(EventHandler handler)
        {
            if (!SupportsChangeEvents) return false;

            if (!GetPair(out var source, out var descriptor)) return false;

            descriptor.RemoveValueChanged(source, handler);
            return true;
        }

        public bool TryResetValue()
        {
            if (!GetPair(out var source, out var descriptor)) return false;

            if (descriptor.CanResetValue(source))
            {
                descriptor.ResetValue(source);
            }

            return true;
        }

        public bool TrySetValue(object value)
        {
            if (!GetPair(out var source, out var descriptor)) return false;

            if (descriptor.IsReadOnly) return false;

            descriptor.SetValue(source, value);
            return true;
        }

        protected bool Equals(PropertyReference other)
        {
            return _hashCode == other._hashCode;
        }

        private bool GetPair(out object source, out PropertyDescriptor descriptor)
        {
            source = _source;
            descriptor = _descriptor as PropertyDescriptor;

            if (_source is WeakReference weakSource)
            {
                source = weakSource.Target;
            }

            if (_descriptor is WeakReference weakDescriptor)
            {
                descriptor = weakDescriptor.Target as PropertyDescriptor;
            }

            if (source == null || descriptor == null) return false;
            return true;
        }

        #endregion
    }
}