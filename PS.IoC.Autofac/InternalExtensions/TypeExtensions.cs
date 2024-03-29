﻿using System;
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

        public static IEnumerable<Type> TypesAssignableFrom(this Type candidateType)
        {
            return candidateType.GetTypeInfo().ImplementedInterfaces.Concat(candidateType.Traverse(t => t.GetTypeInfo().BaseType));
        }
    }
}
