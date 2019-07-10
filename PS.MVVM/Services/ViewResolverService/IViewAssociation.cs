using System;
using System.Collections.Generic;

namespace PS.MVVM.Services
{
    public interface IViewAssociation
    {
        #region Properties

        int Depth { get; }
        IReadOnlyDictionary<string, object> Metadata { get; }
        object Region { get; }
        Type ViewModelType { get; }
        Type ViewType { get; }

        #endregion
    }
}