using System;

namespace PS.MVVM.Services
{
    public interface IViewRegistrationBuilder
    {
        #region Members

        IViewAssociationBuilder Associate(Type viewModelType, object region = null);

        #endregion
    }
}