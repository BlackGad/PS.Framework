using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;
using PS.Extensions;

namespace PS.WPF.Automation
{
    public class Automation : DependencyObject
    {
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.RegisterAttached("Id",
                                                typeof(string),
                                                typeof(Automation),
                                                new PropertyMetadata(OnAutomationIdChanged));

        public static readonly DependencyProperty RootIdProperty =
            DependencyProperty.RegisterAttached("RootId",
                                                typeof(string),
                                                typeof(Automation),
                                                new PropertyMetadata(OnAutomationIdChanged));

        public static string GetId(DependencyObject obj)
        {
            return (string)obj.GetValue(IdProperty);
        }

        public static string GetRootId(DependencyObject obj)
        {
            return (string)obj.GetValue(RootIdProperty);
        }

        public static void SetId(DependencyObject obj, string value)
        {
            obj.SetValue(IdProperty, value);
        }

        public static void SetRootId(DependencyObject obj, string value)
        {
            obj.SetValue(RootIdProperty, value);
        }

        private static void ElementLoaded(object sender, RoutedEventArgs e)
        {
            var owner = (DependencyObject)sender;
            var hierarchy = owner.Traverse(VisualTreeHelper.GetParent);

            IList<string> routeSegments = new List<string>();
            foreach (var element in hierarchy)
            {
                var id = GetId(element);
                var rootId = GetRootId(element);

                if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(rootId)) continue;

                routeSegments.Add(Translate(element, rootId ?? id));

                if (!string.IsNullOrEmpty(rootId)) break;
            }

            if (!routeSegments.Any()) return;
            AutomationProperties.SetAutomationId(owner, string.Join("_", routeSegments.Reverse()));
        }

        private static void OnAutomationIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (FrameworkElement)d;
            if (string.IsNullOrEmpty(e.NewValue as string))
            {
                owner.Loaded -= ElementLoaded;
            }
            else
            {
                owner.Loaded += ElementLoaded;
            }
        }

        private static string Translate(DependencyObject element, string value)
        {
            if (string.Equals(value, "$(index)", StringComparison.InvariantCultureIgnoreCase))
            {
                if (element is DataGridRow row) return row.GetIndex().ToString();
            }

            return value;
        }
    }
}
