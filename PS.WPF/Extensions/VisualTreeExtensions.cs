using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using PS.Extensions;

namespace PS.WPF.Extensions
{
    public static class VisualTreeExtensions
    {
        #region Property definitions

        private static readonly PropertyInfo InheritanceContextProperty;

        #endregion

        #region Static members

        public static DependencyObject FindVisualParentOf(this DependencyObject source, Type type)
        {
            if (type == null) return null;
            var traverse = source.Traverse(e => e.GetVisualParent(), e => !type.IsInstanceOfType(e));
            var lastOrDefault = traverse.LastOrDefault();
            return type.IsInstanceOfType(lastOrDefault) ? lastOrDefault : null;
        }

        public static T FindVisualParentOf<T>(this DependencyObject source)
            where T : DependencyObject
        {
            return (T)FindVisualParentOf(source, typeof(T));
        }

        public static DependencyObject GetVisualParent(this DependencyObject source)
        {
            if (source is Visual visual)
            {
                var result = VisualTreeHelper.GetParent(visual);
                if (result == null && visual is FrameworkElement frameworkElement)
                {
                    return frameworkElement.Parent;
                }

                return result;
            }

            if (source is ContentElement contentElement)
            {
                return ContentOperations.GetParent(contentElement);
            }

            if (source is Freezable freezable)
            {
                return InheritanceContextProperty.GetValue(freezable) as DependencyObject;
            }

            return null;
        }

        public static bool HasVisualParentOf(this DependencyObject source, Type type)
        {
            if (type == null) return false;
            var traverse = source.Traverse(e => e.GetVisualParent(), e => !type.IsInstanceOfType(e));
            return type.IsInstanceOfType(traverse.LastOrDefault());
        }

        public static bool HasVisualParent(this DependencyObject source, DependencyObject target)
        {
            var traverse = target.Traverse(e => e.GetVisualParent(), e => !e.AreEqual(source));
            return traverse.LastOrDefault().AreEqual(source);
        }

        public static DependencyObject TraverseVisualParentWhere(this DependencyObject source, Func<DependencyObject, bool> func)
        {
            var result = source.Traverse(e => e.GetVisualParent(), func).LastOrDefault();
            if (result == null) return null;

            if (func?.Invoke(result) == false) return result;
            return null;
        }

        #endregion

        #region Constructors

        static VisualTreeExtensions()
        {
            InheritanceContextProperty = typeof(Freezable).GetProperty("InheritanceContext",
                                                                       BindingFlags.NonPublic |
                                                                       BindingFlags.Instance |
                                                                       BindingFlags.FlattenHierarchy);
        }

        #endregion
    }
}