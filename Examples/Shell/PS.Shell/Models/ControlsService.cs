using System.Collections.Generic;
using System.Collections.ObjectModel;
using PS.IoC.Attributes;
using PS.Shell.Infrastructure.Models;
using PS.Shell.Infrastructure.Models.ControlsService;
using PS.Shell.Infrastructure.ViewModels;

namespace PS.Shell.Models
{
    [DependencyRegisterAsInterface(typeof(IControlsService))]
    internal class ControlsService : IControlsService
    {
        #region Constructors

        public ControlsService()
        {
            Controls = new ObservableCollection<IControlViewModel>();
        }

        #endregion

        #region IControlsService Members

        public IList<IControlViewModel> Controls { get; }

        #endregion
    }
}