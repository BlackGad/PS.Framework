using System.Windows;
using System.Windows.Documents;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.WPF.Extensions;
using PS.WPF.Theme;

namespace PS.Shell.Infrastructure.ViewModels
{
    public class NotificationViewModel : DependencyObject,
                                         IViewModel,
                                         ITitleAware
    {
        #region Property definitions

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content",
                                        typeof(object),
                                        typeof(NotificationViewModel),
                                        new FrameworkPropertyMetadata(OnContentChanged));

        internal static readonly DependencyProperty FlowDocumentProperty =
            DependencyProperty.Register("FlowDocument",
                                        typeof(FlowDocument),
                                        typeof(NotificationViewModel),
                                        new FrameworkPropertyMetadata(default(FlowDocument)));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title",
                                        typeof(string),
                                        typeof(NotificationViewModel),
                                        new FrameworkPropertyMetadata(default(string)));

        #endregion

        #region Static members

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (NotificationViewModel)d;
            owner.OnContentChanged(e);
        }

        #endregion

        #region Properties

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        internal FlowDocument FlowDocument
        {
            get { return (FlowDocument)GetValue(FlowDocumentProperty); }
            set { SetValue(FlowDocumentProperty, value); }
        }

        #endregion

        #region ITitleAware Members

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region Members

        private void OnContentChanged(DependencyPropertyChangedEventArgs e)
        {
            FlowDocument = e.NewValue.CreateDocument(ThemeFonts.FontFamily, ThemeFonts.FontSize);
        }

        #endregion
    }
}