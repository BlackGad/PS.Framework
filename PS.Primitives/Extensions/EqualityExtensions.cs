using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace PS.Extensions
{
    public static class EqualityExtensions
    {
        public static bool AreDiffers(this object source, object target)
        {
            return !AreEqual(source, target);
        }

        public static bool AreDiffers(this string source, string target, StringComparison comparison)
        {
            return !AreEqual(source, target, comparison);
        }

        public static bool AreEqual(this string source, string target, StringComparison comparison)
        {
            return string.Equals(source, target, comparison);
        }

        public static bool AreEqual(this object source, object target)
        {
            if (ReferenceEquals(source, target)) return true;
            if (source == null) return false;
            if (target == null) return false;

            if (source is XNode sourceNode && target is XNode targetNode)
            {
                return XNode.DeepEquals(sourceNode, targetNode);
            }

            var sourceType = source.GetType();
            var targetType = target.GetType();

            if (sourceType != targetType)
            {
                if (sourceType.IsNumeric() && targetType.IsNumeric())
                {
                    var convertedSource = Convert.ChangeType(source, Type.GetTypeCode(targetType));
                    return convertedSource.Equals(target);
                }

                return false;
            }

            var equatableInterface = sourceType.GetTypeInfo()
                                               .ImplementedInterfaces
                                               .Where(i => i.IsGenericType)
                                               .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEquatable<>));
            if (equatableInterface != null && equatableInterface.GetGenericArguments().Single().IsAssignableFrom(targetType))
            {
                var equalMethod = equatableInterface.GetMethods().Single();
                return (bool)equalMethod.Invoke(source, new[] { target });
            }

            var equalityComparerInterface = sourceType.GetTypeInfo()
                                                      .ImplementedInterfaces
                                                      .Where(i => i.IsGenericType)
                                                      .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEqualityComparer<>));
            if (equalityComparerInterface != null && equalityComparerInterface.GetGenericArguments().Single().IsAssignableFrom(targetType))
            {
                var equalsMethod = equalityComparerInterface.GetMethods().Single(m => Equals(m.Name, nameof(Equals)));
                return (bool)equalsMethod.Invoke(source, new[] { source, target });
            }

            return Equals(source, target);
        }
    }
}
