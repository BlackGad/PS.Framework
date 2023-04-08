using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PS.IoC.InternalExtensions
{
    internal static class TypeExtensions
    {
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
        /// Returns the first concrete interface supported by the candidate type that
        /// closes the provided open generic service type.
        /// </summary>
        /// <param name="this">The type that is being checked for the interface.</param>
        /// <param name="openGeneric">The open generic type to locate.</param>
        /// <returns>The type of the interface.</returns>
        public static IEnumerable<Type> GetTypesThatClose(this Type @this, Type openGeneric)
        {
            return FindAssignableTypesThatClose(@this, openGeneric);
        }

        /// <summary>
        /// Determines whether the candidate type supports any base or
        /// interface that closes the provided generic type.
        /// </summary>
        /// <param name="this">The type to test.</param>
        /// <param name="openGeneric">The open generic against which the type should be tested.</param>
        public static bool IsClosedTypeOf(this Type @this, Type openGeneric)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (openGeneric == null) throw new ArgumentNullException(nameof(openGeneric));
            return openGeneric.IsOpenGeneric()
                ? @this.GetTypesThatClose(openGeneric).Any()
                : throw new ArgumentException($"{openGeneric.FullName} is not open generic");
        }

        /// <summary>
        /// Determine whether a given type is an open generic.
        /// </summary>
        /// <param name="type">The input type.</param>
        /// <returns>True if the type is an open generic; false otherwise.</returns>
        public static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        /// <summary>
        /// Checks whether this type is an open generic type of a given type.
        /// </summary>
        /// <param name="this">The type we are checking.</param>
        /// <param name="type">The type to validate against.</param>
        /// <returns>True if <paramref name="this" /> is a open generic type of <paramref name="type" />. False otherwise.</returns>
        public static bool IsOpenGenericTypeOf(this Type @this, Type type)
        {
            if (@this == null || type == null)
            {
                return false;
            }

            if (!@this.IsGenericTypeDefinition)
            {
                return false;
            }

            if (@this == type)
            {
                return true;
            }

            return @this.CheckBaseTypeIsOpenGenericTypeOf(type)
                   || @this.CheckInterfacesAreOpenGenericTypeOf(type);
        }

        public static IEnumerable<Type> TypesAssignableFrom(this Type candidateType)
        {
            return candidateType.GetTypeInfo().ImplementedInterfaces.Concat(candidateType.Traverse(t => t.GetTypeInfo().BaseType));
        }

        private static bool CheckBaseTypeIsOpenGenericTypeOf(this Type @this, Type type)
        {
            if (@this.BaseType == null)
            {
                return false;
            }

            return @this.BaseType.IsGenericType
                ? @this.BaseType.GetGenericTypeDefinition().IsOpenGenericTypeOf(type)
                : type.IsAssignableFrom(@this.BaseType);
        }

        private static bool CheckInterfacesAreOpenGenericTypeOf(this Type @this, Type type)
        {
            return @this.GetInterfaces()
                        .Any(it => it.IsGenericType
                                 ? it.GetGenericTypeDefinition().IsOpenGenericTypeOf(type)
                                 : type.IsAssignableFrom(it));
        }

        /// <summary>
        /// Looks for an interface on the candidate type that closes the provided open generic interface type.
        /// </summary>
        /// <param name="candidateType">The type that is being checked for the interface.</param>
        /// <param name="openGenericServiceType">The open generic service type to locate.</param>
        /// <returns>True if a closed implementation was found; otherwise false.</returns>
        private static IEnumerable<Type> FindAssignableTypesThatClose(Type candidateType, Type openGenericServiceType)
        {
            return TypesAssignableFrom(candidateType)
                .Where(t => t.IsClosedTypeOf(openGenericServiceType));
        }
    }
}
