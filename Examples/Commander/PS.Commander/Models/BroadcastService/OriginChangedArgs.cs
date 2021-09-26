using System;
using PS.Commander.Models.ExplorerService;

namespace PS.Commander.Models.BroadcastService
{
    public class OriginChangedArgs
    {
        #region Constructors

        public OriginChangedArgs(Explorer explorer, string oldValue, string newValue)
        {
            Explorer = explorer ?? throw new ArgumentNullException(nameof(explorer));
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        public Explorer Explorer { get; }
        public string NewValue { get; }
        public string OldValue { get; }

        #endregion
    }
}