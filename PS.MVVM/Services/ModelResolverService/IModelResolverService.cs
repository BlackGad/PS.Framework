namespace PS.MVVM.Services
{
    public interface IModelResolverService
    {
        IObservableModelCollection Collection(object region);

        IObservableModelObject Object(object region);
    }
}
