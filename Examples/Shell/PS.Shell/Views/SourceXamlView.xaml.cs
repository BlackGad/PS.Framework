using System.Windows;
using System.Windows.Data;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ISourceXaml>))]
    public partial class SourceXamlView : IView<ISourceXaml>
    {
        #region Property definitions

        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(nameof(Code),
                                        typeof(string),
                                        typeof(SourceXamlView),
                                        new FrameworkPropertyMetadata(OnCodeChanged));

        #endregion

        #region Static members

        private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var owner = (SourceXamlView)d;
            owner.Text = (string)e.NewValue;
        }

        #endregion

        #region Constructors

        public SourceXamlView()
        {
            InitializeComponent();

            SetBinding(CodeProperty,
                       new Binding
                       {
                           Path = new PropertyPath($"{nameof(ISourceXaml.Code)}")
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

        #region IView<ISourceXaml> Members

        public ISourceXaml ViewModel
        {
            get { return DataContext as ISourceXaml; }
        }

        #endregion

        #region Event handlers

       
        #endregion
    }
}