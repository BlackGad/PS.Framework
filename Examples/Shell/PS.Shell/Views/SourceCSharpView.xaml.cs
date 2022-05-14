using System.Windows;
using System.Windows.Data;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ISourceCSharp>))]
    public partial class SourceCSharpView : IView<ISourceCSharp>
    {
        #region Property definitions

        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(nameof(Code),
                                        typeof(string),
                                        typeof(SourceCSharpView),
                                        new FrameworkPropertyMetadata(OnCodeChanged));

        #endregion

        #region Static members

        private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SourceCSharpView)d;
            owner.Text = (string)e.NewValue;
        }

        #endregion

        #region Constructors

        public SourceCSharpView()
        {
            InitializeComponent();

            SetBinding(CodeProperty,
                       new Binding
                       {
                           Path = new PropertyPath($"{nameof(ISourceCSharp.Code)}")
                       });

            TextArea.SelectionCornerRadius = 0;
            TextArea.SelectionBorder = null;
        }

        #endregion

        #region Properties

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        #endregion

        #region IView<ISourceCSharp> Members

        public ISourceCSharp ViewModel
        {
            get { return DataContext as ISourceCSharp; }
        }

        #endregion
    }
}