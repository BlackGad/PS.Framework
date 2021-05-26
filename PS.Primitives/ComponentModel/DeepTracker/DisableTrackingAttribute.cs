using System;

namespace PS.ComponentModel.DeepTracker
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DisableTrackingAttribute : Attribute
    {
    }
}