using System;

namespace PS.WPF.Resources
{
    public class ResourceDescriptor
    {
        public static ResourceDescriptor Create<T>(Uri resourceDictionary, string description = null)
        {
            return new ResourceDescriptor(resourceDictionary, typeof(T), description);
        }

        public ResourceDescriptor(Uri resourceDictionary, Type resourceType, string description = null)
        {
            Description = description;
            ResourceDictionary = resourceDictionary;
            ResourceType = resourceType;
        }

        public string Description { get; }

        public Uri ResourceDictionary { get; }

        public Type ResourceType { get; }
    }
}
