using System;

namespace PS.MVVM.Services
{
    public interface IViewResolverAssociateAware
    {
        #region Members

        IViewAssociationBuilder Associate(Type consumerServiceType, Type viewModelType, object payload);

        #endregion
    }
}