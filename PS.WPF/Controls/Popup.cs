using System;
using System.Windows;
using PS.Extensions;

namespace PS.WPF.Controls
{
    public class Popup : System.Windows.Controls.Primitives.Popup
    {
        private System.Windows.Window _activeWindow;

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            SubscribeRepositionEvents(PlacementTarget);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            UnSubscribeRepositionEvents(PlacementTarget);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Equals(PlacementTargetProperty))
            {
                UnSubscribeRepositionEvents((UIElement)e.OldValue);
                SubscribeRepositionEvents((UIElement)e.NewValue);
            }
        }

        private void ActiveWindowOnDeactivated(object sender, EventArgs e)
        {
            if (IsOpen) SetCurrentValue(IsOpenProperty, false);
        }

        private void ActiveWindowOnLocationChanged(object sender, EventArgs e)
        {
            Reposition();
        }

        private void ElementOnLayoutUpdated(object sender, EventArgs e)
        {
            Reposition();
        }

        private void ElementOnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool newValue && !newValue)
            {
                if (IsOpen) SetCurrentValue(IsOpenProperty, false);
            }
        }

        public void Reposition()
        {
            this.InternalMethodCall("Reposition");
        }

        private void SubscribeRepositionEvents(UIElement element)
        {
            if (element == null) return;

            if (System.Windows.Window.GetWindow(element) is System.Windows.Window activeWindow)
            {
                _activeWindow = activeWindow;
                _activeWindow.LocationChanged += ActiveWindowOnLocationChanged;
                _activeWindow.Deactivated += ActiveWindowOnDeactivated;
            }

            element.LayoutUpdated += ElementOnLayoutUpdated;
            element.IsVisibleChanged += ElementOnVisibleChanged;
        }

        private void UnSubscribeRepositionEvents(UIElement element)
        {
            if (_activeWindow != null)
            {
                _activeWindow.LocationChanged -= ActiveWindowOnLocationChanged;
                _activeWindow.Deactivated -= ActiveWindowOnDeactivated;
                _activeWindow = null;
            }

            if (element != null)
            {
                element.LayoutUpdated -= ElementOnLayoutUpdated;
                element.IsVisibleChanged -= ElementOnVisibleChanged;
            }
        }
    }
}
