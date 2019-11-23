using System;

namespace PS.MVVM.Services
{
    public interface IViewResolverService : IViewResolverAssociateAware
    {
        #region Members

        IViewAssociation Find(Type consumerServiceType, Type viewModelType, object region = null);
        IViewAssociationBuilder Region(object region);

        #endregion
    }
}