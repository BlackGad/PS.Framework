using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PS.Extensions
{
    public static class TypeExtensions
    {
        #region Constants

        private static readonly HashSet<TypeCode> FloatingTypeCodes;
        private static readonly HashSet<TypeCode> IntegerTypeCodes;
        private static readonly HashSet<TypeCode> NumericTypeCodes;

        #endregion

        #region Static members

        /// <summary>
        ///     Returns assembly types.
        /// </summary>
        /// <param name="assembly">Source assembly</param>
        /// <returns>List of types.</returns>
        public static IReadOnlyList<Type> GetAssemblyTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
        }

        /// <summary>
        ///     Returns display name for the type with respect to well known display attributes.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Type display name.</returns>
        public static string GetDisplayName(this Type type)
        {
            if (type == null) return null;

            var displayNameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null) return displayNameAttribute.DisplayName;

            var displayAttribute = type.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null) return displayAttribute.Name;

            return type.Name;
        }

        /// <summary>
        ///     Gets display name from type attributes
        /// </summary>
        /// <param name="field">
        ///     Represent field declarations.
        /// </param>
        /// <returns>
        ///     Type display name.
        /// </returns>
        public static string GetDisplayName(this FieldInfo field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            var result = string.Empty;

            var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                result = string.IsNullOrEmpty(displayAttribute.Name)
                    ? result
                    : displayAttribute.Name;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = field.Name;
            }

            return result;
        }

        /// <summary>
        ///     Gets source type. Skips Nullable wrapper.
        /// </summary>
        /// <param name="type">Source type</param>
        /// <returns>Source type or underline nullable type</returns>
        public static Type GetSourceType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsNullable() ? type.GetGenericArguments().First() : type;
        }

        /// <summary>
        ///     Gets type system default value. Default instance for value types, null for reference types
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>Default type value.</returns>
        public static object GetSystemDefaultValue(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        ///     Checks given type for floating numeric type
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>True if type is floating numeric.</returns>
        public static bool IsFloating(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.IsEnum) return false;

            var typeCode = Type.GetTypeCode(type);
            return FloatingTypeCodes.Contains(typeCode);
        }

        /// <summary>
        ///     Checks given type for integer numeric type
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>True if type is integer numeric.</returns>
        public static bool IsInteger(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.IsEnum) return false;

            var typeCode = Type.GetTypeCode(type);
            return IntegerTypeCodes.Contains(typeCode);
        }

        /// <summary>
        ///     Checks given type for Nullable wrapper.
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>True if type is Nullable.</returns>
        public static bool IsNullable(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        ///     Checks given type for numeric
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>True if type is numeric.</returns>
        public static bool IsNumeric(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type.IsEnum) return false;

            var typeCode = Type.GetTypeCode(type);
            return NumericTypeCodes.Contains(typeCode);
        }

        /// <summary>
        ///     Attempt to get numeric type limits.
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <param name="max">Maximum value.</param>
        /// <param name="min">Minimum value.</param>
        /// <returns>True if result range is valid.</returns>
        public static bool TryGetNumericLimits(this Type type, out object max, out object min)
        {
            if (type == null) throw new ArgumentNullException("type");
            max = null;
            min = null;
            if (!type.IsNumeric()) return false;
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Char:
                    max = Char.MaxValue;
                    min = Char.MinValue;
                    break;
                case TypeCode.SByte:
                    max = SByte.MaxValue;
                    min = SByte.MinValue;
                    break;
                case TypeCode.Byte:
                    max = Byte.MaxValue;
                    min = Byte.MinValue;
                    break;
                case TypeCode.Int16:
                    max = Int16.MaxValue;
                    min = Int16.MinValue;
                    break;
                case TypeCode.UInt16:
                    max = UInt16.MaxValue;
                    min = UInt16.MinValue;
                    break;
                case TypeCode.Int32:
                    max = Int32.MaxValue;
                    min = Int32.MinValue;
                    break;
                case TypeCode.UInt32:
                    max = UInt32.MaxValue;
                    min = UInt32.MinValue;
                    break;
                case TypeCode.Int64:
                    max = Int64.MaxValue;
                    min = Int64.MinValue;
                    break;
                case TypeCode.UInt64:
                    max = UInt64.MaxValue;
                    min = UInt64.MinValue;
                    break;
                case TypeCode.Single:
                    max = Single.MaxValue;
                    min = Single.MinValue;
                    break;
                case TypeCode.Double:
                    max = Double.MaxValue;
                    min = Double.MinValue;
                    break;
                case TypeCode.Decimal:
                    max = Decimal.MaxValue;
                    min = Decimal.MinValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        #endregion

        #region Constructors

        static TypeExtensions()
        {
            FloatingTypeCodes = new HashSet<TypeCode>
            {
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal
            };

            IntegerTypeCodes = new HashSet<TypeCode>
            {
                TypeCode.Char,
                TypeCode.SByte,
                TypeCode.Byte,
                TypeCode.Int16,
                TypeCode.UInt16,
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
            };

            NumericTypeCodes = new HashSet<TypeCode>(IntegerTypeCodes.Union(IntegerTypeCodes));
        }

        #endregion
    }
}