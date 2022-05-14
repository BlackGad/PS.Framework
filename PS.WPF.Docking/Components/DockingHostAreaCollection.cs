using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using PS.WPF.Docking.Controls;

namespace PS.WPF.Docking.Components
{
    public class DockingHostAreaCollection : ObservableCollection<FrameworkElement>
    {
        #region Constructors

        public DockingHostAreaCollection()
        {
            Window = new Window
            {
                SizeToContent = SizeToContent.WidthAndHeight,
                Height = double.NaN,
                Width = double.NaN,
                Content = new DockingHost
                {
                    //TODO: Possible memory leak - rework in future
                    ItemsSource = this
                }
            };
        }

        #endregion

        #region Properties

        public Window Window { get; set; }

        #endregion

        #region Override members

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (Count > 0)
            {
                Window.Show();
            }
            else
            {
                Window.Hide();
            }
        }

        #endregion
    }
}