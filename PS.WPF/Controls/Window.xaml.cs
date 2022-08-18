using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PS.Extensions;
using PS.WPF.Extensions;
using PS.WPF.Patterns.Command;
using PS.WPF.Resources;

namespace PS.WPF.Controls
{
    public class Window : System.Windows.Window
    {
        private static readonly DependencyPropertyKey ValidationErrorsPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ValidationErrors),
                                                typeof(IReadOnlyList<ValidationError>),
                                                typeof(Window),
                                                new FrameworkPropertyMetadata(OnValidationErrorsChanged));

        public static readonly DependencyProperty CommandButtonsHorizontalAlignmentProperty =
            DependencyProperty.Register("CommandButtonsHorizontalAlignment",
                                        typeof(HorizontalAlignment),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(default(HorizontalAlignment)));

        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands",
                                        typeof(UICommandCollection),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(default(UICommandCollection), null, OnCoerceCommands));

        public static readonly DependencyProperty IsResizableProperty =
            DependencyProperty.Register("IsResizable",
                                        typeof(bool),
                                        typeof(Window),
                                        new FrameworkPropertyMetadata(true, OnResizableChanged));

        public static readonly DependencyProperty ValidationErrorsProperty = ValidationErrorsPropertyKey.DependencyProperty;

        private static object OnCoerceCommands(DependencyObject d, object baseValue)
        {
            return baseValue ?? new UICommandCollection();
        }

        private static void OnResizableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (Window)d;
            if (e.NewValue is bool value)
            {
                owner.ResizeMode = value ? ResizeMode.CanResize : ResizeMode.NoResize;
            }
        }

        private static void OnValidationErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (Window)d;

            foreach (var command in owner.Commands.Enumerate())
            {
                command.RaiseCanExecuteChanged();
            }
        }

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            ResourceHelper.SetDefaultStyle(typeof(Window), WindowResource.ControlStyle);
        }

        public Window()
        {
            CoerceValue(CommandsProperty);

            Validation.AddErrorHandler(this, ErrorHandler);
        }

        public HorizontalAlignment CommandButtonsHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(CommandButtonsHorizontalAlignmentProperty); }
            set { SetValue(CommandButtonsHorizontalAlignmentProperty, value); }
        }

        public UICommandCollection Commands
        {
            get { return (UICommandCollection)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public bool IsResizable
        {
            get { return (bool)GetValue(IsResizableProperty); }
            set { SetValue(IsResizableProperty, value); }
        }

        public IReadOnlyList<ValidationError> ValidationErrors
        {
            get { return (IReadOnlyList<ValidationError>)GetValue(ValidationErrorsProperty); }
            private set { SetValue(ValidationErrorsPropertyKey, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var source = DependencyPropertyHelper.GetValueSource(this, CommandsProperty).BaseValueSource;
            if (source == BaseValueSource.Style && Commands != null)
            {
                //Commands collection instance was set via style. Because we use SharedResourceDictionary instance sharing as well.
                //We need to make a duplicate.
                SetCurrentValue(CommandsProperty, new UICommandCollection(Commands));
            }
        }

        private void ErrorHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                var list = ValidationErrors.Enumerate().ToList();
                if (list.All(existing => existing.AreDiffers(e.Error)))
                {
                    list.Add(e.Error);
                    ValidationErrors = list;
                }
            }
            else
            {
                var list = ValidationErrors.Enumerate().ToList();
                var errorsToRemove = list.Where(existing => existing.AreEqual(e.Error)).ToList();
                if (errorsToRemove.Any())
                {
                    foreach (var error in errorsToRemove)
                    {
                        list.Remove(error);
                    }

                    ValidationErrors = list.Any() ? list : null;
                }
            }

            foreach (var command in Commands.Enumerate())
            {
                command.RaiseCanExecuteChanged();
            }

            Dispatcher.Postpone(() => Validation.GetHasError(this));
        }

        #region Nested type: WindowResource

        public static class WindowResource
        {
            private static readonly Uri Default =
                new Uri("/PS.WPF;component/Controls/Window.xaml", UriKind.RelativeOrAbsolute);

            public static readonly ResourceDescriptor ControlStyle =
                ResourceDescriptor.Create<Style>(description: "Default Window style",
                                                 resourceDictionary: Default);

            public static readonly ResourceDescriptor ControlTemplate =
                ResourceDescriptor.Create<ControlTemplate>(description: "Default Window control template",
                                                           resourceDictionary: Default);
        }

        #endregion
    }
}
