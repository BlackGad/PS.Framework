using System;
using System.Linq;
using System.Reflection;

namespace PS.Extensions
{
    public static class ReflectionExtensions
    {
        #region Static members

        public static object InternalFieldGet(this object obj, string name)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return InternalFieldGet(obj, obj.GetType(), name);
        }

        public static object InternalFieldGet(this object obj, Type type, string name)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var info = type.GetField(name,
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance |
                                     BindingFlags.Public |
                                     BindingFlags.FlattenHierarchy);

            try
            {
                return info?.GetValue(obj);
            }
            catch (TargetInvocationException e)
            {
                // ReSharper disable once PossibleNullReferenceException
                throw e.InnerException;
            }
        }

        public static void InternalFieldSet(this object obj, string name, object value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            InternalFieldSet(obj, obj.GetType(), name, value);
        }

        public static void InternalFieldSet(this object obj, Type type, string name, object value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (type == null) throw new ArgumentNullException(nameof(type));

            var info = type.GetField(name,
                                     BindingFlags.NonPublic |
                                     BindingFlags.Instance |
                                     BindingFlags.Public |
                                     BindingFlags.FlattenHierarchy);

            try
            {
                info?.SetValue(obj, value);
            }
            catch (TargetInvocationException e)
            {
                // ReSharper disable once PossibleNullReferenceException
                throw e.InnerException;
            }
        }

        /// <summary>
        ///     Calls internal method from different assembly using reflection.
        /// </summary>
        /// <param name="obj">Object instance.</param>
        /// <param name="methodName">Internal method name.</param>
        /// <param name="args">Internal method arguments.</param>
        /// <returns>Internal method return.</returns>
        public static object InternalMethodCall<T>(this T obj, string methodName, params object[] args)
            where T : class
        {
            return obj.InternalMethodCall(typeof(T), methodName, args);
        }

        /// <summary>
        ///     Calls internal method from different assembly using reflection.
        /// </summary>
        /// <param name="obj">Object instance.</param>
        /// <param name="type">Required reflection type.</param>
        /// <param name="methodName">Internal method name.</param>
        /// <param name="args">Internal method arguments.</param>
        /// <returns>Internal method return.</returns>
        public static object InternalMethodCall(this object obj, Type type, string methodName, params object[] args)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            MethodInfo dynamicMethod;
            if (args.Any(a => a == null))
            {
                dynamicMethod = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
            else
            {
                dynamicMethod = type.GetMethod(methodName,
                                               BindingFlags.NonPublic | BindingFlags.Instance,
                                               null,
                                               args.Select(a => a.GetType()).ToArray(),
                                               null);
            }

            try
            {
                return dynamicMethod?.Invoke(obj, args);
            }
            catch (TargetInvocationException e)
            {
                // ReSharper disable once PossibleNullReferenceException
                throw e.InnerException;
            }
        }

        public static object InternalPropertyGet(this object obj, string propertyName)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var dynamicProperty = obj.GetType().GetProperty(propertyName,
                                                            BindingFlags.NonPublic |
                                                            BindingFlags.Instance |
                                                            BindingFlags.Public |
                                                            BindingFlags.FlattenHierarchy);

            try
            {
                return dynamicProperty?.GetValue(obj);
            }
            catch (TargetInvocationException e)
            {
                // ReSharper disable once PossibleNullReferenceException
                throw e.InnerException;
            }
        }

        public static void InternalPropertySet(this object obj, string propertyName, object value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var dynamicProperty = obj.GetType().GetProperty(propertyName,
                                                            BindingFlags.NonPublic |
                                                            BindingFlags.Instance |
                                                            BindingFlags.Public |
                                                            BindingFlags.FlattenHierarchy);

            try
            {
                if (dynamicProperty?.CanWrite != true) return;
                dynamicProperty.SetValue(obj, value);
            }
            catch (TargetInvocationException e)
            {
                // ReSharper disable once PossibleNullReferenceException
                throw e.InnerException;
            }
        }

        #endregion
    }
}