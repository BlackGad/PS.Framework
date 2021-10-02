using System.Collections.Generic;

namespace PS.Shell.Infrastructure.Models.ControlsService
{
    public interface IControlsService
    {
        #region Properties

        IList<IControlViewModel> Controls { get; }

        #endregion
    }
}