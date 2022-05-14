using System;
using System.Collections.Generic;
using System.Linq;
using PS.Extensions;

namespace PS.Data.Descriptors
{
    [Serializable]
    public class CompositeDescriptor : Descriptor
    {
        private readonly string _key;

        #region Constructors

        public CompositeDescriptor(params Descriptor[] descriptors)
        {
            var linearDescriptors = new List<Descriptor>();
            foreach (var descriptor in descriptors.Enumerate())
            {
                if (descriptor is CompositeDescriptor composite)
                {
                    linearDescriptors.AddRange(composite.Descriptors);
                }
                else
                {
                    linearDescriptors.Add(descriptor);
                }
            }

            _key = linearDescriptors.Select(d => d.GetKey())
                                    .OrderBy(h => h)
                                    .Aggregate(string.Empty, (agg, key) => agg + key);

            Descriptors = linearDescriptors.ToArray();
        }

        #endregion

        #region Properties

        public IReadOnlyList<Descriptor> Descriptors { get; }

        #endregion

        #region Override members

        public override string ToString()
        {
            var type = GetType();
            return $"CompositeDescriptor<{type.Name}>: {string.Join(" | ", Descriptors.Select(d => d.ToString()))}";
        }

        public override string GetKey()
        {
            return _key;
        }

        #endregion

        #region Members

        public Descriptor<T> Get<T>()
        {
            return Descriptors.OfType<Descriptor<T>>().FirstOrDefault();
        }

        #endregion
    }
}