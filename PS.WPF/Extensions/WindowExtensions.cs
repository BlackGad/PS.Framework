using System.Reflection;
using System.Windows;

namespace PS.WPF.Extensions
{
    public static class WindowExtensions
    {
        private static readonly FieldInfo ShowingAsDialogField;

        public static bool IsModal(this Window window)
        {
            if (window == null) return false;
            return (bool)ShowingAsDialogField.GetValue(window);
        }

        static WindowExtensions()
        {
            ShowingAsDialogField = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
