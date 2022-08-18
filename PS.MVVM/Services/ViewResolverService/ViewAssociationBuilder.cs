using System;
using System.Collections.Generic;
using System.Linq;
using PS.Data;
using PS.Extensions;
using PS.Reflection;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable CollectionNeverUpdated.Local

namespace PS.MVVM.Services
{
    internal class ViewAssociationBuilder : IViewAssociationBuilder
    {
        private readonly ObjectsStorage<Tuple<Type, Type>, AssociationData> _associations;
        private readonly object _region;
        private Dictionary<object, object> _chainAssociationMetadata;

        public ViewAssociationBuilder(object region)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));
            _associations = new ObjectsStorage<Tuple<Type, Type>, AssociationData>();
        }

        public IViewAssociationBuilder Associate(Type consumerServiceType, Type viewModelType, object payload)
        {
            if (consumerServiceType == null) throw new ArgumentNullException(nameof(consumerServiceType));
            if (viewModelType == null) throw new ArgumentNullException(nameof(viewModelType));
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            var key = new Tuple<Type, Type>(consumerServiceType, viewModelType);
            if (_associations.ContainsKey(key))
            {
                throw new ArgumentException($"{viewModelType.Name} is already associated with payload for {consumerServiceType.Name} consumer service");
            }

            var data = _associations[key];
            data.Payload = payload;

            _chainAssociationMetadata = data.Metadata;

            return this;
        }

        public IViewAssociationBuilder Metadata(object key, object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_chainAssociationMetadata != null) _chainAssociationMetadata.AddOrUpdate(key, k => value, (k, existing) => value);
            return this;
        }

        public IViewAssociation Find(Type consumerServiceType, Type viewModelType)
        {
            if (consumerServiceType == null) throw new ArgumentNullException(nameof(consumerServiceType));
            if (viewModelType == null) return null;

            var consumerAssociations = _associations.Where(record => record.Key.Item1 == consumerServiceType)
                                                    .Select(record => new
                                                    {
                                                        Record = record,
                                                        InheritanceDepth = TypeInheritanceDepth.Measure(record.Key.Item2, viewModelType)
                                                    })
                                                    .Where(a => a.InheritanceDepth.Depth.HasValue)
                                                    .ToList();
            if (!consumerAssociations.Any()) return null;

            var association = consumerAssociations.MinBy(a => a.InheritanceDepth.Depth);
            if (association == null) return null;

            return new ViewAssociation
            {
                Region = _region,
                ConsumerServiceType = consumerServiceType,
                ViewModelType = association.Record.Key.Item2,
                Metadata = association.Record.Value.Metadata,
                Payload = association.Record.Value.Payload,
                Depth = association.InheritanceDepth.Depth ?? int.MaxValue
            };
        }

        #region Nested type: AssociationData

        private class AssociationData
        {
            public AssociationData()
            {
                Metadata = new Dictionary<object, object>();
            }

            public Dictionary<object, object> Metadata { get; }

            public object Payload { get; set; }
        }

        #endregion
    }
}
