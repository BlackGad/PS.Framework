using System;
using System.Collections.Generic;

namespace PS.MVVM.Services
{
    internal class ViewAssociation : IViewAssociation
    {
        public int Depth { get; set; }

        public object Payload { get; set; }

        public IReadOnlyDictionary<object, object> Metadata { get; set; }

        public Type ConsumerServiceType { get; set; }

        public object Region { get; set; }

        public Type ViewModelType { get; set; }
    }
}
