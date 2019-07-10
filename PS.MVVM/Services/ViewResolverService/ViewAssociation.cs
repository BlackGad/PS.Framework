using System;
using System.Collections.Generic;

namespace PS.MVVM.Services
{
    internal class ViewAssociation : IViewAssociation
    {
        #region IViewAssociation Members

        public int Depth { get; set; }
        public IReadOnlyDictionary<string, object> Metadata { get; set; }
        public object Region { get; set; }
        public Type ViewModelType { get; set; }
        public Type ViewType { get; set; }

        #endregion
    }
}