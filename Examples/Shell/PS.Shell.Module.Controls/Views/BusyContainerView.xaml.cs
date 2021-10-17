using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Module.Controls.ViewModels;
using PS.WPF.Controls.BusyContainer;
using TextBox = PS.WPF.Controls.TextBox;

namespace PS.Shell.Module.Controls.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<BusyContainerViewModel>))]
    public partial class BusyContainerView : IView<BusyContainerViewModel>
    {
        #region Constants

        public static readonly BusyState BusyState = new BusyState();

        public static readonly string BusyStatePayload = "BusyState payload";
        public static readonly string NullPayload = "Null";
        public static readonly string StringPayload = "String payload";

        #endregion

        #region Constructors

        public BusyContainerView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<BusyContainerViewModel> Members

        public BusyContainerViewModel ViewModel
        {
            get { return DataContext as BusyContainerViewModel; }
        }

        #endregion

        #region Event handlers

        private void ControlIsBusyStateAssigned_OnCheckedUnchecked(object sender, RoutedEventArgs e)
        {
            UpdateControlPayload();
        }

        private void ControlPayloadType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateControlPayload();
        }

        #endregion

        #region Members

        private void UpdateControlPayload()
        {
            if (StackPayloadString == null || StackPayloadBusyState == null || Control == null)
            {
                return;
            }

            BindingOperations.ClearBinding(Control, BusyContainer.StateProperty);
            Control.ClearValue(BusyContainer.StateProperty);

            if (ControlPayloadType.SelectedItem.AreEqual(NullPayload))
            {
                Control.State = null;
                StackPayloadString.Visibility = Visibility.Collapsed;
                StackPayloadBusyState.Visibility = Visibility.Collapsed;
            }

            if (ControlPayloadType.SelectedItem.AreEqual(StringPayload))
            {
                Control.SetBinding(BusyContainer.StateProperty,
                                   new Binding
                                   {
                                       Source = ControlStringPayload,
                                       Path = new PropertyPath(TextBox.ValueProperty),
                                       Mode = BindingMode.TwoWay
                                   });

                StackPayloadString.Visibility = Visibility.Visible;
                StackPayloadBusyState.Visibility = Visibility.Collapsed;
            }

            if (ControlPayloadType.SelectedItem.AreEqual(BusyStatePayload))
            {
                if (ControlIsBusyStateAssigned.IsChecked == true)
                {
                    Control.State = BusyState;
                }
                else
                {
                    Control.State = null;
                }

                StackPayloadString.Visibility = Visibility.Collapsed;
                StackPayloadBusyState.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}