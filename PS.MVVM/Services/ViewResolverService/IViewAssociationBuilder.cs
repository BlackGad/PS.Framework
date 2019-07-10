namespace PS.MVVM.Services
{
    public interface IViewAssociationBuilder
    {
        #region Members

        IViewAssociationBuilder SetMetadata(string key, object value);

        #endregion
    }
}