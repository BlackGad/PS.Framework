// ReSharper disable UnusedTypeParameter

using System.Windows.Data;

namespace PS.WPF.DataTemplate
{
    public interface IDataTemplate
    {
        BindingBase ItemsSource { get; set; }
    }

    public interface IDataTemplate<T> : IDataTemplate
    {
    }
}
