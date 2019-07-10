using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PS.Extensions
{
    public static class TypeExtensions
    {
        #region Static members

        public static Type[] GetAssemblyTypes(this Assembly assembly)
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
        ///     Checks given type for numeric
        /// </summary>
        /// <param name="type">Given type.</param>
        /// <returns>True if type is numeric.</returns>
        public static bool IsNumeric(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (type.IsEnum) return false;
            var numericTypeCodes = new List<TypeCode>
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
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal
            };
            var typeCode = Type.GetTypeCode(type);
            return numericTypeCodes.Contains(typeCode);
        }

        public static IEnumerable<Type> TypesAssignableFrom(this Type candidateType)
        {
            return candidateType.GetTypeInfo().ImplementedInterfaces.Concat(candidateType.Traverse(t => t.GetTypeInfo().BaseType));
        }

        #endregion
    }
}