namespace PS.MVVM.Services
{
    public interface IViewAssociationBuilder : IViewResolverAssociateAware
    {
        #region Members

        IViewAssociationBuilder Metadata(object key, object value);

        #endregion
    }
}