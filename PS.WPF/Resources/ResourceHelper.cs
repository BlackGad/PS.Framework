using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace PS.WPF.Resources
{
    public static class ResourceHelper
    {
        private static readonly MethodInfo FrameworkElementFindResourceMethod;

        public static T GetResource<T>(this ResourceDescriptor descriptor)
        {
            return (T)GetResource(descriptor);
        }

        public static object GetResource(this ResourceDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));

            var element = new FrameworkElement();
            element.Resources.MergedDictionaries.Add(new SharedResourceDictionary
            {
                Source = descriptor.ResourceDictionary
            });
            var result = element.FindResource(descriptor);
            if (descriptor.ResourceType.IsInstanceOfType(result)) return result;
            throw new InvalidOperationException("Resource not found");
        }

        public static void SetDefaultStyle(Type elementType, ResourceDescriptor descriptor)
        {
            if (Runtime.IsDesignMode) return;
            var stylePropertyMetadata = (FrameworkPropertyMetadata)FrameworkElement.StyleProperty.GetMetadata(elementType);
            FrameworkElement.StyleProperty.OverrideMetadata(elementType,
                                                            new FrameworkPropertyMetadata
                                                            {
                                                                PropertyChangedCallback = stylePropertyMetadata.PropertyChangedCallback,
                                                                CoerceValueCallback = stylePropertyMetadata.CoerceValueCallback,
                                                                DefaultValue = GetResource<Style>(descriptor)
                                                            });
        }

        public static void SetDefaultStyle(this FrameworkElement element, ResourceDescriptor descriptor)
        {
            if (FrameworkElementFindResourceMethod == null) throw new InvalidOperationException("Invalid framework version");
            var resource = FrameworkElementFindResourceMethod.Invoke(null, new object[] { element, null, element.GetType() });
            if (resource == DependencyProperty.UnsetValue) element.Style = GetResource<Style>(descriptor);
        }

        public static void SetDefaultStyle(this FrameworkContentElement element, ResourceDescriptor descriptor)
        {
            if (FrameworkElementFindResourceMethod == null) throw new InvalidOperationException("Invalid framework version");
            var resource = FrameworkElementFindResourceMethod.Invoke(null, new object[] { element, null, element.GetType() });
            if (resource == DependencyProperty.UnsetValue) element.Style = GetResource<Style>(descriptor);
        }

        static ResourceHelper()
        {
            try
            {
                FrameworkElementFindResourceMethod = typeof(FrameworkElement).GetMethod("FindResourceInternal",
                                                                                        BindingFlags.NonPublic | BindingFlags.Static,
                                                                                        null,
                                                                                        new[]
                                                                                        {
                                                                                            typeof(FrameworkElement), typeof(FrameworkContentElement),
                                                                                            typeof(object)
                                                                                        },
                                                                                        null);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debug.WriteLine(e);
            }
        }
    }
}
