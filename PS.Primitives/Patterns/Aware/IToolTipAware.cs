namespace PS.Patterns.Aware
{
    public interface IToolTipAware
    {
        string ToolTipDescription { get; set; }

        object ToolTipImage { get; set; }

        string ToolTipTitle { get; set; }
    }
}
