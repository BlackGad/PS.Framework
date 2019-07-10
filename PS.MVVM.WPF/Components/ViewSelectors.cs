using System;

namespace PS.MVVM.Components
{
    public class ViewSelectors
    {
        #region Constructors

        public ViewSelectors(System.Windows.Controls.DataTemplateSelector dataTemplateSelector, System.Windows.Controls.StyleSelector containerStyleSelector)
        {
            DataTemplateSelector = dataTemplateSelector ?? throw new ArgumentNullException(nameof(dataTemplateSelector));
            ContainerStyleSelector = containerStyleSelector ?? throw new ArgumentNullException(nameof(containerStyleSelector));
        }

        #endregion

        #region Properties

        public System.Windows.Controls.StyleSelector ContainerStyleSelector { get; }
        public System.Windows.Controls.DataTemplateSelector DataTemplateSelector { get; }

        #endregion
    }
}