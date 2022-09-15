using System;
using System.Windows;

namespace PS.WPF.ValueConverters.SwitchValueConverter.Cases
{
    public class AssignableFrom : SwitchCase
    {
        public static readonly DependencyProperty DataTypeProperty =
            DependencyProperty.Register(nameof(System.ComponentModel.DataAnnotations.DataType),
                                        typeof(Type),
                                        typeof(AssignableFrom),
                                        new FrameworkPropertyMetadata(default(Type)));

        public static readonly DependencyProperty DerivedProperty =
            DependencyProperty.Register(nameof(Derived),
                                        typeof(bool),
                                        typeof(AssignableFrom),
                                        new FrameworkPropertyMetadata(default(bool)));

        public Type DataType
        {
            get { return (Type)GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        public bool Derived
        {
            get { return (bool)GetValue(DerivedProperty); }
            set { SetValue(DerivedProperty, value); }
        }

        public override bool IsValid(object item)
        {
            if (item == null) return false;
            if (DataType == null) return false;

            var itemType = item as Type ?? item.GetType();

            if (Derived) return DataType.IsAssignableFrom(itemType);
            return itemType == DataType;
        }
    }
}
