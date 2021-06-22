using System.Windows;
using System.Windows.Data;
using Autofac;
using PS.IoC.Attributes;
using PS.WPF.DataTemplate;

namespace PS.Commander.Models
{
    [DependencyRegisterAsGenericTypeOf(typeof(IDataTemplate<>))]
    internal class IoCDataTemplate<TView> : ViewHierarchicalDataTemplate,
                                            IDataTemplate<TView>
    {
        #region Constructors

        public IoCDataTemplate(ILifetimeScope scope,
                               BindingBase hierarchyBinding = null,
                               PropertyPath hierarchyBindingPropertyPath = null,
                               DependencyProperty hierarchyBindingProperty = null)
        {
            ViewType = typeof(TView);
            ViewFactory = type => (FrameworkElement)scope.Resolve(type);

            var binding = hierarchyBinding;
            if (binding == null)
            {
                var bindingPropertyPath = hierarchyBindingPropertyPath;
                if (bindingPropertyPath == null && hierarchyBindingProperty != null)
                {
                    bindingPropertyPath = new PropertyPath(hierarchyBindingProperty.Name);
                }

                if (bindingPropertyPath != null)
                {
                    binding = new Binding
                    {
                        Path = bindingPropertyPath
                    };
                }
            }

            if (binding != null)
            {
                ItemsSource = binding;
            }
        }

        #endregion
    }
}