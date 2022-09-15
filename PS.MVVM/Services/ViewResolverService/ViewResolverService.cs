using System;
using PS.Data;

// ReSharper disable CollectionNeverUpdated.Local

namespace PS.MVVM.Services
{
    public class ViewResolverService : IViewResolverService
    {
        private readonly object _defaultRegion;
        private readonly ObjectsStorage<object, ViewAssociationBuilder> _regionAssociationBuilders;

        public ViewResolverService()
        {
            _regionAssociationBuilders = new ObjectsStorage<object, ViewAssociationBuilder>(region => new ViewAssociationBuilder(region));
            _defaultRegion = new object();
        }

        public IViewAssociationBuilder Associate(Type consumerServiceType, Type viewModelType, object payload)
        {
            return Region(_defaultRegion).Associate(consumerServiceType, viewModelType, payload);
        }

        public IViewAssociation Find(Type consumerServiceType, Type viewModelType, object region = null)
        {
            if (consumerServiceType == null) throw new ArgumentNullException(nameof(consumerServiceType));
            if (viewModelType == null) throw new ArgumentNullException(nameof(viewModelType));

            if (_regionAssociationBuilders.TryGetValue(region ?? _defaultRegion, out var associationBuilder))
            {
                return associationBuilder.Find(consumerServiceType, viewModelType);
            }

            return null;
        }

        public IViewAssociationBuilder Region(object region)
        {
            return _regionAssociationBuilders[region ?? _defaultRegion];
        }
    }
}
