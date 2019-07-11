using System;
using System.Windows;

namespace PS.WPF.StyleSelector
{
    public class RelayStyleSelector : System.Windows.Controls.StyleSelector
    {
        private readonly Func<object, DependencyObject, Style> _selectTemplateFunc;

        #region Constructors

        public RelayStyleSelector(Func<object, DependencyObject, Style> selectTemplateFunc)
        {
            _selectTemplateFunc = selectTemplateFunc ?? throw new ArgumentNullException(nameof(selectTemplateFunc));
        }

        #endregion

        #region Override members

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return _selectTemplateFunc(item, container);
        }

        #endregion
    }
}