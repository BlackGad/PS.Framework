using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PS.Data;
using PS.Extensions;

namespace PS.ComponentModel.Extensions
{
    public static class PropertyReferenceExtensions
    {
        private static readonly ObjectsStorage<Type, IReadOnlyList<PropertyDescriptor>> CachedTypeProperties;

        public static IReadOnlyList<PropertyDescriptor> GetPropertyDescriptors(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return CachedTypeProperties[type];
        }

        public static IReadOnlyList<PropertyReference> GetPropertyReferences(this object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var type = source.GetType();
            return type.GetPropertyDescriptors().Select(p => new PropertyReference(source, p)).ToList();
        }

        static PropertyReferenceExtensions()
        {
            CachedTypeProperties = new ObjectsStorage<Type, IReadOnlyList<PropertyDescriptor>>(type =>
            {
                try
                {
                    return TypeDescriptor.GetProperties(type).Enumerate<PropertyDescriptor>().ToList();
                }
                catch (Exception)
                {
                    return Enumerable.Empty<PropertyDescriptor>().ToList();
                }
            });
        }
    }
}
