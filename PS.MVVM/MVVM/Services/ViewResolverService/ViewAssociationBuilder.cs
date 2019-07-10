using System;
using System.Collections.Generic;

namespace PS.MVVM.Services
{
    internal class ViewAssociationBuilder : IViewAssociationBuilder
    {
        #region Constructors

        public ViewAssociationBuilder(Type viewModelType, object region)
        {
            ViewModelType = viewModelType;
            Region = region;
            Metadata = new Dictionary<string, object>();
        }

        #endregion

        #region Properties

        public Dictionary<string, object> Metadata { get; }

        public object Region { get; }
        public Type ViewModelType { get; }

        #endregion

        #region IViewAssociationBuilder Members

        IViewAssociationBuilder IViewAssociationBuilder.SetMetadata(string key, object value)
        {
            key = key ?? string.Empty;
            if (!Metadata.ContainsKey(key)) Metadata.Add(key, null);
            Metadata[key] = value;
            return this;
        }

        #endregion
    }
}