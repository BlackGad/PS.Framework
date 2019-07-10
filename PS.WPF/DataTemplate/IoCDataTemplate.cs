using System;
using System.Windows;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.DataTemplate.Base;

namespace PS.WPF.DataTemplate
{
    [ContentProperty]
    public class IoCDataTemplate : System.Windows.DataTemplate,
                                   IIoCCustomDataTemplate
    {
        #region Constructors

        public IoCDataTemplate()
        {
            VisualTree = new CustomFrameworkElementFactory(this);
            VisualTree.InternalMethodCall("Seal", this);
        }

        #endregion

        #region Properties

        public string Key { get; set; }
        public Type ServiceType { get; set; }

        #endregion

        #region IIoCCustomDataTemplate Members

        public Func<Type, string, object> FrameworkElementResolver { get; set; }

        public double? DesignHeight { get; set; }
        public double? DesignWidth { get; set; }

        string ICustomDataTemplate.Description
        {
            get
            {
                var description = $"IoC template: {ServiceType.Name}";
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