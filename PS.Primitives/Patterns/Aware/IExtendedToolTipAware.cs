namespace PS.Patterns.Aware
{
    public interface IExtendedToolTipAware : IToolTipAware
    {
        string ToolTipFooterDescription { get; set; }

        object ToolTipFooterImage { get; set; }

        string ToolTipFooterTitle { get; set; }
    }
}
