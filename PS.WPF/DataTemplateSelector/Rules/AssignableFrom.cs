using System;

namespace PS.WPF.DataTemplateSelector.Rules
{
    public class AssignableFrom : SelectRule
    {
        #region Properties

        public Type DataType { get; set; }

        public bool Derived { get; set; }

        #endregion

        #region Override members

        public override bool IsValid(object item)
        {
            if (item == null) return false;
            if (DataType == null) return false;

            var itemType = item as Type;
            if (itemType == null) itemType = item.GetType();

            if (Derived) return DataType.IsAssignableFrom(itemType);
            return itemType == DataType;
        }

        #endregion
    }
}