using System;

namespace PS.MVVM.Services
{
    public interface IViewResolverService : IViewResolverAssociateAware
    {
        IViewAssociation Find(Type consumerServiceType, Type viewModelType, object region = null);

        IViewAssociationBuilder Region(object region);
    }
}
