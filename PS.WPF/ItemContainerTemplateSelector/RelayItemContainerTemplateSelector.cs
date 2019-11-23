using System;
using System.Windows;
using System.Windows.Controls;

namespace PS.WPF.ItemContainerTemplateSelector
{
    public class RelayItemContainerTemplateSelector : System.Windows.Controls.ItemContainerTemplateSelector
    {
        private readonly Func<object, DependencyObject, System.Windows.DataTemplate> _selectTemplateFunc;

        #region Constructors

        public RelayItemContainerTemplateSelector(Func<object, DependencyObject, System.Windows.DataTemplate> selectTemplateFunc)
        {
            _selectTemplateFunc = selectTemplateFunc ?? throw new ArgumentNullException(nameof(selectTemplateFunc));
        }

        #endregion

        #region Override members

        public override System.Windows.DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
        {
            return _selectTemplateFunc(item, parentItemsControl);
        }

        #endregion
    }
}