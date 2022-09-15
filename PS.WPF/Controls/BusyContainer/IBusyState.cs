using System;
using PS.Patterns.Aware;

namespace PS.WPF.Controls.BusyContainer
{
    public interface IBusyState : IDisposable,
                                  IMutableTitleAware,
                                  IMutableDescriptionAware
    {
    }
}
