using System.Collections.Generic;
using PS.Shell.Infrastructure.ViewModels;

namespace PS.Shell.Infrastructure.Models
{
    public interface IControlsService
    {
        #region Properties

        IList<IControlViewModel> Controls { get; }

        #endregion
    }
}