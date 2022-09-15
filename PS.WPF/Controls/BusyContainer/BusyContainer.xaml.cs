using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using PS.Patterns.Aware;
using PS.WPF.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.BusyContainer
{
    public class BusyContainer : ContentControl
    {
        public static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.Register(nameof(Behavior),
                                        typeof(BusyBehavior),
                                        typeof(BusyContainer),
                                        new FrameworkPropertyMetadata(OnBusyBehaviorChanged));

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(nameof(IsBusy),
                                        typeof(bool),
                                        typeof(BusyContainer),
                                        new FrameworkPropertyMetadata(OnIsBusyChanged));

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State),
                                        typeof(object),
                                        typeof(BusyContainer),
                                        new FrameworkPropertyMetadata(OnBusyStateChanged));

        private static void OnBusyBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BusyContainer)d;
            owner.Dispatcher.Postpone(owner.UpdateBusyState);
        }

        private static void OnBusyStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BusyContainer)d;
            owner.UpdateBindings();
            owner.Dispatcher.Postpone(owner.UpdateBusyState);
        }

        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (BusyContainer)d;
            owner.Dispatcher.Postpone(owner.UpdateBusyState);
        }

        private TextBlock _description;

        private TextBlock _title;

        static BusyContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyContainer), new FrameworkPropertyMetadata(typeof(BusyContainer)));
            ResourceHelper.SetDefaultStyle(typeof(BusyContainer), Resource.ControlStyle);
        }

        public BusyBehavior Behavior
        {
            get { return (BusyBehavior)GetValue(BehaviorProperty); }
            set { SetValue(BehaviorProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public object State
        {
            get { return GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_Title") is TextBlock title)
            {
                _title = title;
            }

            if (GetTemplateChild("PART_Description") is TextBlock description)
            {
                _description = description;
            }

            UpdateBindings();
            UpdateBusyState();
        }

        private void UpdateBindings()
        {
            if (State is ITitleAware)
            {
                _title?.SetBinding(TextBlock.TextProperty,
                                   new Binding
                                   {
                                       Source = this,
                                       Path = new PropertyPath("(0).Title", StateProperty),
                                       Mode = BindingMode.OneWay
                                   });
            }

            if (State is IDescriptionAware)
            {
                _description?.SetBinding(TextBlock.TextProperty,
                                         new Binding
                                         {
                                             Source = this,
                                             Path = new PropertyPath("(0).Description", StateProperty),
                                             Mode = BindingMode.OneWay
                                         });
            }
            else
            {
                _description?.SetBinding(TextBlock.TextProperty,
                                         new Binding
                                         {
                                             Source = this,
                                             Path = new PropertyPath(StateProperty),
                                             Mode = BindingMode.OneWay
                                         });
            }
        }

        private void UpdateBusyState()
        {
            switch (Behavior)
            {
                case BusyBehavior.Manual:
                    break;
                case BusyBehavior.Auto:
                    SetCurrentValue(IsBusyProperty, State != null);
                    break;
                case BusyBehavior.AutoOnValuableString:
                    SetCurrentValue(IsBusyProperty, !string.IsNullOrWhiteSpace(State as string));
                    break;
            }
        }

        #region Nested type: Resource

        public static class Resource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/BusyContainer/BusyContainer.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ArcGeometry =
                ResourceDescriptor.Create<Geometry>(description: "Arc geometry",
                                                    resourceDictionary: Default);

            public static readonly ResourceDescriptor ArcPathStyle =
                ResourceDescriptor.Create<Style>(description: "Arc path style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default BusyContainer style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default BusyContainer control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
