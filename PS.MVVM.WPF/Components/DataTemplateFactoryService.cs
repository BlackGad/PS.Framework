﻿using PS.MVVM.Extensions;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;

namespace PS.MVVM.Components
{
    public sealed class DataTemplateFactoryService : IDataTemplateFactoryService
    {
        #region IDataTemplateFactoryService Members

        public System.Windows.DataTemplate ResolveDataTemplate(object item, IViewAssociation association)
        {
            var hierarchyBinding = association.GetHierarchyBinding();
            if (hierarchyBinding != null)
            {
                return new ViewHierarchicalDataTemplate
                {
                    ViewType = association.ViewType,
                    ItemsSource = hierarchyBinding
                };
            }

            return new ViewDataTemplate
            {
                ViewType = association.ViewType
            };
        }

        #endregion
    }
}