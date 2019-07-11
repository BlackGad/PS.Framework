namespace PS.MVVM.Services
{
    public interface IDataTemplateFactoryService
    {
        #region Members

        System.Windows.DataTemplate ResolveDataTemplate(object item, IViewAssociation association);

        #endregion
    }
}