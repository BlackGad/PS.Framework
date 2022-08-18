namespace PS.MVVM.Services
{
    public interface IViewAssociationBuilder : IViewResolverAssociateAware
    {
        IViewAssociationBuilder Metadata(object key, object value);
    }
}
