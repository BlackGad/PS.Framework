using System;
using System.Windows;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.DataTemplate.Base;

namespace PS.WPF.DataTemplate
{
    [ContentProperty]
    public class ViewDataTemplate : System.Windows.DataTemplate,
                                    ICustomDataTemplate
    {
        public ViewDataTemplate()
        {
            VisualTree = new CustomFrameworkElementFactory(this);
            VisualTree.InternalMethodCall("Seal", this);
            ViewFactory = type => Activator.CreateInstance(ViewType) as FrameworkElement;
        }

        public Func<Type, FrameworkElement> ViewFactory { get; set; }

        public Type ViewType { get; set; }

        public double? DesignHeight { get; set; }

        public double? DesignWidth { get; set; }

        string ICustomDataTemplate.Description
        {
            get { return $"View of {ViewType} type template"; }
        }

        public FrameworkElement CreateView()
        {
            return ViewFactory?.Invoke(ViewType);
        }
    }
}
