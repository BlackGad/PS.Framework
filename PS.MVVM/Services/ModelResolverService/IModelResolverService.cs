namespace PS.MVVM.Services
{
    public interface IModelResolverService
    {
        #region Members

        IObservableModelCollection Collection(object region);
        IObservableModelObject Object(object region);

        #endregion
    }
}