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
        #region Constructors

        public ViewDataTemplate()
        {
            VisualTree = new CustomFrameworkElementFactory(this);
            VisualTree.InternalMethodCall("Seal", this);
        }

        #endregion

        #region Properties

        public Type ViewType { get; set; }

        #endregion

        #region ICustomDataTemplate Members

        public double? DesignHeight { get; set; }
        public double? DesignWidth { get; set; }

        string ICustomDataTemplate.Description
        {
            get
            {
                var description = $"View of {ViewType} type template";
                return description;
            }
        }

        public FrameworkElement CreateView()
        {
            return Activator.CreateInstance(ViewType) as FrameworkElement;
        }

        #endregion
    }
}