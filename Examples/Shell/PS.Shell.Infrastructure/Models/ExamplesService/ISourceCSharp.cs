namespace PS.Shell.Infrastructure.Models.ExamplesService
{
    public interface ISourceCSharp : ISource
    {
        #region Properties

        string Code { get; }

        #endregion
    }
}