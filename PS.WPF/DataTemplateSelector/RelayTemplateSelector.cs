using System;
using System.Windows;

namespace PS.WPF.DataTemplateSelector
{
    public class RelayTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        private readonly Func<object, DependencyObject, System.Windows.DataTemplate> _selectTemplateFunc;

        public RelayTemplateSelector(Func<object, DependencyObject, System.Windows.DataTemplate> selectTemplateFunc)
        {
            _selectTemplateFunc = selectTemplateFunc ?? throw new ArgumentNullException(nameof(selectTemplateFunc));
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return _selectTemplateFunc(item, container);
        }
    }
}
