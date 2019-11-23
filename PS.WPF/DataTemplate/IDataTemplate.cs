// ReSharper disable UnusedTypeParameter

using System.Windows.Data;

namespace PS.WPF.DataTemplate
{
    public interface IDataTemplate
    {
        #region Properties

        BindingBase ItemsSource { get; set; }

        #endregion
    }

    public interface IDataTemplate<T> : IDataTemplate
    {
    }
}