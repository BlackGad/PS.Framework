using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views
{
    [DependencyRegisterAsSelf]
    [DependencyRegisterAsInterface(typeof(IView<ISourceCSharp>))]
    public partial class SourceCSharpView : IView<ISourceCSharp>
    {
        #region Constructors

        public SourceCSharpView()
        {
            InitializeComponent();
        }

        #endregion

        #region IView<ISourceCode> Members

        public ISourceCSharp ViewModel
        {
            get { return DataContext as ISourceCSharp; }
        }

        #endregion
    }
}