using System;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using PS.Extensions;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Ribbon.Controls
{
    public class RibbonToggleButton : System.Windows.Controls.Ribbon.RibbonToggleButton
    {
        #region Property definitions

        public static readonly DependencyProperty SuppressCommandExecutionOnClickProperty =
            DependencyProperty.Register(nameof(SuppressCommandExecutionOnClick),
                                        typeof(bool),
                                        typeof(RibbonToggleButton),
                                        new FrameworkPropertyMetadata(default(bool)));

        #endregion

        #region Constructors

        static RibbonToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonToggleButton), new FrameworkPropertyMetadata(typeof(RibbonToggleButton)));
            ResourceHelper.SetDefaultStyle(typeof(RibbonToggleButton), Resource.ControlStyle);
        }

        #endregion

        #region Properties

        public bool SuppressCommandExecutionOnClick
        {
            get { return (bool)GetValue(SuppressCommandExecutionOnClickProperty); }
            set { SetValue(SuppressCommandExecutionOnClickProperty, value); }
        }

        #endregion

        #region Override members

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.AreEqual(CommandParameterProperty))
            {
                this.InternalMethodCall<ButtonBase>("UpdateCanExecute");
            }
        }

        protected override void OnClick()
        {
            if (SuppressCommandExecutionOnClick)
            {
                //Simulate full call hierarchy except CommandHelpers.ExecuteCommandSource(this) in ButtonBase implementation
                //Toggle button from ToggleButton.OnClick(). Native implementation is internal so we will use toggle AutomationPeer.
                if (OnCreateAutomationPeer() is IToggleProvider toggleProvider)
                {
                    toggleProvider.Toggle();
                }

                //Send click event from ButtonBase.OnClick()
                RaiseEvent(new RoutedEventArgs(ClickEvent, this));

                //Send dismiss popup event from RibbonToggleButton.OnClick()
                RaiseEvent(new RibbonDismissPopupEventArgs());
                return;
            }

            base.OnClick();
        }

        #endregion

        #region Nested type: Resource

        public static class Resource
        {
            #region Constants

            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Ribbon/Controls/RibbonToggleButton.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default RibbonToggleButton style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default RibbonToggleButton control template",
                                                           resourceDictionary: Default);

            #endregion
        }

        #endregion
    }
}