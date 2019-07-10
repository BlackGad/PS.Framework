using System;
using System.Windows;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.DataTemplate.Base;

namespace PS.WPF.DataTemplate
{
    [ContentProperty]
    public class IoCHierarchicalDataTemplate : HierarchicalDataTemplate,
                                               IIoCCustomDataTemplate
    {
        #region Constructors

        public IoCHierarchicalDataTemplate()
        {
            VisualTree = new CustomFrameworkElementFactory(this);
            VisualTree.InternalMethodCall("Seal", this);
        }

        #endregion

        #region Properties

        public Func<Type, string, object> FrameworkElementResolver { get; set; }
        public string Key { get; set; }
        public Type ServiceType { get; set; }

        #endregion

        #region ICustomDataTemplate Members

        public double? DesignHeight { get; set; }
        public double? DesignWidth { get; set; }

        string ICustomDataTemplate.Description
        {
            get
            {
                var description = $"IoC Hierarchical template: {ServiceType.Name}";
                if (!string.IsNullOrEmpty(Key)) description += $", {Key}";
                return description;
            }
        }

        public FrameworkElement CreateView()
        {
            var resolver = FrameworkElementResolver ?? IoCFactory.GetInstance;
            return (FrameworkElement)resolver.Invoke(ServiceType, Key);
        }

        #endregion
    }
}