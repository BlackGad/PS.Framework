using System;

namespace PS.MVVM.Services
{
    public interface IViewResolverAssociateAware
    {
        IViewAssociationBuilder Associate(Type consumerServiceType, Type viewModelType, object payload);
    }
}
