using System.Windows;

namespace PS.WPF.DataTemplate.Base
{
    public interface ICustomDataTemplate
    {
        string Description { get; }

        double? DesignHeight { get; }

        double? DesignWidth { get; }

        FrameworkElement CreateView();
    }
}
