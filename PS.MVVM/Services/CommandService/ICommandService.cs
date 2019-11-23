using System;
using System.Collections;
using System.Collections.Generic;
using PS.Patterns.Aware;

namespace PS.MVVM.Services.CommandService
{
    public interface ICommandService
    {
        #region Members

        void Add(CommandServiceComponent component);

        IEnumerable CreateView(Func<CommandServiceComponent, bool> filter);

        IReadOnlyCollection<CommandServiceComponent> Find(object identifier);

        ISubscriptionAware HandleActivation(object identifier);

        #endregion
    }
}