namespace PS.Patterns.Aware
{
    public interface IExtendedToolTipAware : IToolTipAware
    {
        #region Properties

        string ToolTipFooterDescription { get; set; }
        object ToolTipFooterImage { get; set; }
        string ToolTipFooterTitle { get; set; }

        #endregion
    }
}