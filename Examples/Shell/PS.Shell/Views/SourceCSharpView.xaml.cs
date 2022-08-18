using System.Windows;
using System.Windows.Data;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Views;

[DependencyRegisterAsSelf]
[DependencyRegisterAsInterface(typeof(IView<ISourceCSharp>))]
public partial class SourceCSharpView : IView<ISourceCSharp>
{
    public static readonly DependencyProperty CodeProperty =
        DependencyProperty.Register(nameof(Code),
                                    typeof(string),
                                    typeof(SourceCSharpView),
                                    new FrameworkPropertyMetadata(OnCodeChanged));

    private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var owner = (SourceCSharpView)d;
        owner.Text = (string)e.NewValue;
    }

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

    public string Code
    {
        get { return (string)GetValue(CodeProperty); }
        set { SetValue(CodeProperty, value); }
    }

    public ISourceCSharp ViewModel
    {
        get { return DataContext as ISourceCSharp; }
    }
}
