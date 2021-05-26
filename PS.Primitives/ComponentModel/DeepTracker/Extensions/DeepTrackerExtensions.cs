using System.Linq;
using PS.Extensions;

namespace PS.ComponentModel.DeepTracker.Extensions
{
    public static class DeepTrackerExtensions
    {
        #region Static members

        public static string FormatMessage(this ChangedEventArgs e, bool trimOutput = true)
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
                var oldValueString = propertyEventArgs.OldValue.GetEffectiveString();
                var newValueString = propertyEventArgs.NewValue.GetEffectiveString();
                if (trimOutput)
                {
                    if (oldValueString.Length > 100) oldValueString = oldValueString.Substring(0, 100) + "...";
                    if (newValueString.Length > 100) newValueString = newValueString.Substring(0, 100) + "...";
                }

                return $"{e.Route} property changed '{oldValueString}' -> '{newValueString}'";
            }

            return $"{e.Route} changed.";
        }

        #endregion
    }
}