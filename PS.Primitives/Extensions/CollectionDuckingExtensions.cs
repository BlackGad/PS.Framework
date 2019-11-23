using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PS.Extensions
{
    namespace Ducking
    {
        public static class CollectionDuckingExtensions
        {
            #region Static members

            /// <summary>
            ///     Ducking Add item to collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="item">Item object.</param>
            /// <returns>True if added. False otherwise.</returns>
            public static bool CollectionAdd(this object enumeration, object item)
            {
                if (enumeration.CollectionCanAdd())
                {
                    var method = GetMethods(enumeration.GetType(), "Add").First();
                    var returnValue = method.Invoke(enumeration, new[] { item });
                    if (returnValue is bool b) return b;
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Ducking object check for collection Add ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanAdd(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly() || enumeration.CollectionIsFixedSize()) return false;
                return enumeration.GetType().CollectionCanAdd();
            }

            /// <summary>
            ///     Ducking object check for collection Add ability.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanAdd(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");
                if (enumerationType.IsArray) return false;
                return GetMethods(enumerationType, "Add").Any();
            }

            /// <summary>
            ///     Ducking object check for collection Clear ability.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanClear(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");
                if (enumerationType.IsArray) return false;
                return GetMethods(enumerationType, "Clear").Any();
            }

            /// <summary>
            ///     Ducking object check for collection Clear ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanClear(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly() || enumeration.CollectionIsFixedSize()) return false;
                return enumeration.GetType().CollectionCanClear();
            }

            /// <summary>
            ///     Ducking object check for collection Insert ability.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanInsert(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");
                if (enumerationType.IsArray) return false;
                return GetMethods(enumerationType, "Insert").Any();
            }

            /// <summary>
            ///     Ducking object check for collection Insert ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanInsert(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly() || enumeration.CollectionIsFixedSize()) return false;
                return enumeration.GetType().CollectionCanInsert();
            }

            /// <summary>
            ///     Ducking object check for collection Remove ability.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanRemove(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");
                if (enumerationType.IsArray) return false;
                return GetMethods(enumerationType, "Remove").Any();
            }

            /// <summary>
            ///     Ducking object check for collection Remove ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanRemove(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly() || enumeration.CollectionIsFixedSize()) return false;
                return enumeration.GetType().CollectionCanRemove();
            }

            /// <summary>
            ///     Ducking object check for collection RemoveAt ability.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanRemoveAt(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");
                if (enumerationType.IsArray) return false;
                return GetMethods(enumerationType, "RemoveAt").Any();
            }

            /// <summary>
            ///     Ducking object check for collection RemoveAt ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanRemoveAt(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly() || enumeration.CollectionIsFixedSize()) return false;
                return enumeration.GetType().CollectionCanRemoveAt();
            }

            /// <summary>
            ///     Ducking object check for collection Replace ability.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanReplace(this object enumeration)
            {
                if (enumeration == null) return false;
                if (enumeration.CollectionIsReadOnly()) return false;

                return enumeration.GetType().CollectionCanReplace();
            }

            /// <summary>
            ///     Ducking object check for collection Replace ability.
            /// </summary>
            /// <param name="enumerationType">Source object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionCanReplace(this Type enumerationType)
            {
                if (enumerationType == null) throw new ArgumentNullException("enumerationType");

                var properties = GetProperties(enumerationType, "Item");
                var indexer = properties.FirstOrDefault(p => p.CanRead && p.CanWrite);

                return indexer != null;
            }

            /// <summary>
            ///     Ducking Clear collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>True if cleared. False otherwise.</returns>
            public static bool CollectionClear(this object enumeration)
            {
                if (enumeration.CollectionCanClear())
                {
                    var method = GetMethods(enumeration.GetType(), "Clear").First();
                    var returnValue = method.Invoke(enumeration, new object[] { });
                    if (returnValue is bool b) return b;
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Ducking get by index item method.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="index">Item position.</param>
            public static object CollectionGetByIndex(this object enumeration, int index)
            {
                if (index == -1) return null;
                if (enumeration?.GetType().CollectionCanReplace() == true)
                {
                    if (index == -1) return null;

                    var property = GetProperties(enumeration.GetType(), "Item").First();
                    return property.GetValue(enumeration, new object[] { index });
                }

                return null;
            }

            /// <summary>
            ///     Ducking get index for item method.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="value">Item object.</param>
            public static int CollectionGetIndex(this object enumeration, object value)
            {
                if (enumeration.CollectionCanReplace())
                {
                    var method = GetMethods(enumeration.GetType(), "IndexOf").First();
                    var returnValue = method.Invoke(enumeration, new[] { value });
                    if (returnValue is int i) return i;
                }

                return -1;
            }

            /// <summary>
            ///     Returns item index in collection using item's Runtime hash code.
            /// </summary>
            /// <param name="sourceEnumeration">Source <c>object</c>.</param>
            /// <param name="value">Item <c>object</c>.</param>
            public static int CollectionGetIndexByReference(this object sourceEnumeration, object value)
            {
                if (sourceEnumeration is IEnumerable enumeration)
                {
                    var currentIndex = 0;
                    var valueHashCode = RuntimeHelpers.GetHashCode(value);

                    foreach (var item in enumeration)
                    {
                        if (RuntimeHelpers.GetHashCode(item) == valueHashCode) return currentIndex;
                        currentIndex++;
                    }
                }

                return -1;
            }

            /// <summary>
            ///     Ducking Insert item into collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="index">Item index.</param>
            /// <param name="item">Item object.</param>
            /// <returns>True if inserted. False otherwise.</returns>
            public static bool CollectionInsert(this object enumeration, int index, object item)
            {
                if (enumeration.CollectionCanInsert())
                {
                    var method = GetMethods(enumeration.GetType(), "Insert").First();
                    var returnValue = method.Invoke(enumeration, new[] { index, item });
                    if (returnValue is bool b) return b;
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Ducking object check for collection IsFixedSize
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>Value of IsFixedSize property if found. False otherwise.</returns>
            public static bool CollectionIsFixedSize(this object enumeration)
            {
                if (enumeration == null) return false;
                var enumerationType = enumeration.GetType();

                var interfaces = FindCollectionInterfaces(enumerationType);
                foreach (var type in interfaces)
                {
                    var propertyInfo = type.GetProperty("IsFixedSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo != null)
                    {
                        return (bool)propertyInfo.GetValue(enumeration);
                    }
                }

                return false;
            }

            /// <summary>
            ///     Ducking object check for collection IsReadOnly
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>Value of IsReadOnly property if found. True otherwise.</returns>
            public static bool CollectionIsReadOnly(this object enumeration)
            {
                if (enumeration == null) return true;
                var enumerationType = enumeration.GetType();

                var interfaces = FindCollectionInterfaces(enumerationType);

                var commonCollections = interfaces.Where(i => typeof(ICollection).IsAssignableFrom(i)).ToArray();
                if (commonCollections.Any()) return false;

                foreach (var type in interfaces)
                {
                    var propertyInfo = type.GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (propertyInfo != null)
                    {
                        return (bool)propertyInfo.GetValue(enumeration);
                    }
                }

                return true;
            }

            /// <summary>
            ///     Ducking Remove item from collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="item">Item object.</param>
            /// <returns>True if able. False otherwise.</returns>
            public static bool CollectionRemove(this object enumeration, object item)
            {
                if (enumeration.CollectionCanRemove())
                {
                    var method = GetMethods(enumeration.GetType(), "Remove").First();
                    var returnValue = method.Invoke(enumeration, new[] { item });
                    if (returnValue is bool b) return b;
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Ducking RemoveAt item by index from collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="index">Item index.</param>
            public static void CollectionRemoveAt(this object enumeration, int index)
            {
                if (enumeration.CollectionCanRemoveAt())
                {
                    var method = GetMethods(enumeration.GetType(), "RemoveAt").First();
                    method.Invoke(enumeration, new object[] { index });
                }
            }

            /// <summary>
            ///     Ducking Replace item in collection.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <param name="index">Replace item position.</param>
            /// <param name="item">Item object.</param>
            /// <returns>True if replaced. False otherwise.</returns>
            public static bool CollectionReplace(this object enumeration, int index, object item)
            {
                if (enumeration.CollectionCanReplace())
                {
                    var property = GetProperties(enumeration.GetType(), "Item").First();
                    property.SetValue(enumeration, item, new object[] { index });
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Gets array of base collection interfaces.
            /// </summary>
            /// <param name="enumeration">Source object.</param>
            /// <returns>Array of base collection interfaces.</returns>
            public static Type[] FindCollectionInterfaces(this object enumeration)
            {
                return enumeration == null
                    ? Enumerable.Empty<Type>().ToArray()
                    : enumeration.GetType().FindCollectionInterfaces();
            }

            /// <summary>
            ///     Gets array of base collection interfaces.
            /// </summary>
            /// <param name="enumerationType">Source object type.</param>
            /// <returns>Array of base collection interfaces.</returns>
            public static Type[] FindCollectionInterfaces(this Type enumerationType)
            {
                if (enumerationType == null) return Enumerable.Empty<Type>().ToArray();

                bool TypeFilter(Type type, object criteria)
                {
                    if (type == typeof(ICollection)) return true;
                    if (type == typeof(IList)) return true;
                    if (type.IsGenericType && typeof(ICollection<>) == type.GetGenericTypeDefinition()) return true;
                    if (type.IsGenericType && typeof(IList<>) == type.GetGenericTypeDefinition()) return true;
                    return false;
                }

                var interfaces = enumerationType.FindInterfaces(TypeFilter, null).ToList();
                if (TypeFilter(enumerationType, null)) interfaces.Add(enumerationType);

                return interfaces.Distinct().ToArray();
            }

            public static Type[] FindPossibleCollectionItemTypes(this Type enumerationType)
            {
                if (enumerationType == null) return Enumerable.Empty<Type>().ToArray();

                var result = new List<Type>();

                var interfaceTypes = enumerationType.FindCollectionInterfaces();
                if (interfaceTypes.Contains(typeof(ICollection))) result.Add(typeof(object));

                result.AddRange(interfaceTypes.Where(t => t.IsGenericType).Select(t => t.GetGenericArguments().First()));
                return result.Distinct().ToArray();
            }

            private static IEnumerable<MethodInfo> GetMethods(Type sourceType, string methodName)
            {
                var interfaces = FindCollectionInterfaces(sourceType);
                var methodInfos = interfaces.Select(type => type.GetMethod(methodName,
                                                                           BindingFlags.Instance |
                                                                           BindingFlags.Public |
                                                                           BindingFlags.NonPublic))
                                            .Where(p => p != null)
                                            .ToArray();
                return methodInfos;
            }

            private static IEnumerable<PropertyInfo> GetProperties(Type sourceType, string name)
            {
                var interfaces = FindCollectionInterfaces(sourceType);
                var methodInfos = interfaces.Select(type => type.GetProperty(name,
                                                                             BindingFlags.Instance |
                                                                             BindingFlags.Public |
                                                                             BindingFlags.NonPublic))
                                            .Where(p => p != null)
                                            .ToArray();
                return methodInfos;
            }

            #endregion
        }
    }
}