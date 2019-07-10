using System;

namespace PS.MVVM.Services
{
    public interface IViewResolverService
    {
        #region Members

        IViewAssociation FindAssociation(Type viewModelType, object region = null);
        IViewRegistrationBuilder Register(Type viewType);

        #endregion
    }
}