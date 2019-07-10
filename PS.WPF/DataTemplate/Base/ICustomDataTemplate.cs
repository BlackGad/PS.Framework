using System.Windows;

namespace PS.WPF.DataTemplate.Base
{
    public interface ICustomDataTemplate
    {
        #region Properties

        string Description { get; }
        double? DesignHeight { get; }
        double? DesignWidth { get; }

        #endregion

        #region Members

        FrameworkElement CreateView();

        #endregion
    }
}