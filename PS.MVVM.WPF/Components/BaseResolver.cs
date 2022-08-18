using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using PS.Extensions;
using PS.MVVM.Services;
using PS.WPF.Extensions;

namespace PS.MVVM.Components;

public abstract class BaseResolver<TService> : MarkupExtension
    where TService : class
{
    protected static string FormatServiceErrorMessage(DependencyProperty property)
    {
        var message = $"Could not resolve {typeof(TService).Name}. To fix this you can: " +
                      $"set service instance directly via {nameof(Resolver)} property, " +
                      $"specify {property.OwnerType.Name}.{property.Name} attached property to visual parent or " +
                      "specify service globally";
        return message;
    }

    private TService _resolver;
    private ResolverServiceSource _resolverSource;

    protected BaseResolver()
    {
        var globalService = GlobalServices.Get<TService>();
        ResolverSource = globalService == null ? ResolverServiceSource.Ancestor : ResolverServiceSource.Global;
    }

    public TService Resolver
    {
        get { return _resolver; }
        set
        {
            if (_resolver.AreEqual(value)) return;
            _resolver = value;
            ResolverSource = value == null ? ResolverServiceSource.Global : ResolverServiceSource.Direct;
        }
    }

    public ResolverServiceSource ResolverSource
    {
        get { return _resolverSource; }
        set
        {
            if (_resolverSource.AreEqual(value)) return;
            _resolverSource = value;
            if (_resolverSource != ResolverServiceSource.Direct) Resolver = null;
        }
    }

    protected TService GetService(object targetObject, DependencyProperty resolverServiceProperty)
    {
        if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
        if (resolverServiceProperty == null) throw new ArgumentNullException(nameof(resolverServiceProperty));

        switch (ResolverSource)
        {
            case ResolverServiceSource.Global:
                return GlobalServices.Get<TService>();
            case ResolverServiceSource.Ancestor:
                if (targetObject is DependencyObject dependencyObject)
                {
                    var viewRegistryServiceTraverse = dependencyObject.Traverse(o => o.GetVisualParent(), o => o.GetValue(resolverServiceProperty) == null);
                    return viewRegistryServiceTraverse.LastOrDefault()?.GetValue(resolverServiceProperty) as TService;
                }

                throw new InvalidOperationException("Ancestor resolver source available only for DependencyObjects");
            case ResolverServiceSource.Direct:
                return Resolver;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
