using System.Windows;
using PS.WPF.Resources;

namespace App.Module.Main;

public static class XamlResources
{
    private static readonly Uri DEFAULT = new("/App.Module.Main;component/XamlResources.xaml", UriKind.RelativeOrAbsolute);

    public static readonly ResourceDescriptor MainWindowStyle = ResourceDescriptor.Create<Style>(DEFAULT);
}
