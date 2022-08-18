using System;
using System.Windows;
using System.Windows.Controls;
using PS.MVVM.Components;
using PS.MVVM.Services;
using PS.WPF.DataTemplate;
using PS.WPF.Resources;

namespace PS.MVVM.Extensions;

public static class ViewAssociationExtensions
{
    public static IViewResolverAssociateAware Associate<TViewModel>(this IViewResolverAssociateAware service,
                                                                    ResourceDescriptor container = null,
                                                                    ResourceDescriptor style = null,
                                                                    IDataTemplate template = null)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        if (container != null) service.AssociateContainer<TViewModel>(container);
        if (style != null) service.AssociateStyle<TViewModel>(style);
        if (template != null) service.AssociateTemplate<TViewModel>(template);

        return service;
    }

    public static IViewAssociationBuilder AssociateContainer<TViewModel>(this IViewResolverAssociateAware service, ItemContainerTemplate template)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(ContainerResolver), typeof(TViewModel), template);
    }

    public static IViewAssociationBuilder AssociateContainer<TViewModel>(this IViewResolverAssociateAware service, ResourceDescriptor descriptor)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(ContainerResolver), typeof(TViewModel), descriptor);
    }

    public static IViewAssociationBuilder AssociateStyle<TViewModel>(this IViewResolverAssociateAware service, Style style)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(StyleResolver), typeof(TViewModel), style);
    }

    public static IViewAssociationBuilder AssociateStyle<TViewModel>(this IViewResolverAssociateAware service, ResourceDescriptor descriptor)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(StyleResolver), typeof(TViewModel), descriptor);
    }

    public static IViewAssociationBuilder AssociateTemplate<TViewModel>(this IViewResolverAssociateAware service, DataTemplate template)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(TemplateResolver), typeof(TViewModel), template);
    }

    public static IViewAssociationBuilder AssociateTemplate<TViewModel>(this IViewResolverAssociateAware service, IDataTemplate template)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(TemplateResolver), typeof(TViewModel), template);
    }

    public static IViewAssociationBuilder AssociateTemplate<TViewModel>(this IViewResolverAssociateAware service, ResourceDescriptor descriptor)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        return service.Associate(typeof(TemplateResolver), typeof(TViewModel), descriptor);
    }
}
