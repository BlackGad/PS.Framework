using PS.MVVM.Services;

namespace PS.MVVM.Components
{
    public interface IDataTemplateFactoryService
    {
        #region Members

        System.Windows.DataTemplate ResolveDataTemplate(object item, IViewAssociation association);

        #endregion
    }
}