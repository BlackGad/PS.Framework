using System;

namespace PS.WPF.Resources
{
    public class ResourceDescriptor
    {
        #region Static members

        public static ResourceDescriptor Create<T>(Uri resourceDictionary, string description = null)
        {
            return new ResourceDescriptor(resourceDictionary, typeof(T), description);
        }

        #endregion

        #region Constructors

        public ResourceDescriptor(Uri resourceDictionary, Type resourceType, string description = null)
        {
            Description = description;
            ResourceDictionary = resourceDictionary;
            ResourceType = resourceType;
        }

        #endregion

        #region Properties

        public string Description { get; }
        public Uri ResourceDictionary { get; }

        public Type ResourceType { get; }

        #endregion
    }
}