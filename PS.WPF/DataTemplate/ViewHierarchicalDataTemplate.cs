﻿using System;
using System.Windows;
using System.Windows.Markup;
using PS.Extensions;
using PS.WPF.DataTemplate.Base;

namespace PS.WPF.DataTemplate
{
    [ContentProperty]
    public class ViewHierarchicalDataTemplate : HierarchicalDataTemplate,
                                                ICustomDataTemplate
    {
        #region Constructors

        public ViewHierarchicalDataTemplate()
        {
            VisualTree = new CustomFrameworkElementFactory(this);
            VisualTree.InternalMethodCall("Seal", this);
            ViewFactory = type => Activator.CreateInstance(ViewType) as FrameworkElement;
        }

        #endregion

        #region Properties

        public Func<Type, FrameworkElement> ViewFactory { get; set; }

        public Type ViewType { get; set; }

        #endregion

        #region ICustomDataTemplate Members

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

        #endregion
    }
}