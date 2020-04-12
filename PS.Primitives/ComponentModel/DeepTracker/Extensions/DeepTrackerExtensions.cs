using System.Linq;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Extensions
{
    public static class DeepTrackerExtensions
    {
        #region Static members

        public static string FormatMessage(this ChangedEventArgs e)
        {
            if (e is ChangedCollectionEventArgs collectionEventArgs)
            {
                var newItemsCount = collectionEventArgs.EventArgs.NewItems.Enumerate().Count();
                var oldItemsCount = collectionEventArgs.EventArgs.OldItems.Enumerate().Count();
                var message = $"{e.Route} collection updated({collectionEventArgs.EventArgs.Action}).";
                if (newItemsCount > 0)
                {
                    message += $" Added: {newItemsCount} items.";
                }

                if (oldItemsCount > 0)
                {
                    message += $" Removed: {oldItemsCount} items.";
                }

                return message;
            }

            if (e is ChangedPropertyEventArgs propertyEventArgs)
            {
                return $"{e.Route} property changed '{propertyEventArgs.OldValue.GetEffectiveString()}' -> '{propertyEventArgs.NewValue.GetEffectiveString()}'";
            }

            return $"{e.Route} changed.";
        }

        #endregion
    }
}