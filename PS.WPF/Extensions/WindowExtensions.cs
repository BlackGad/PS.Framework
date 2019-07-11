using System.Reflection;
using System.Windows;

namespace PS.WPF.Extensions
{
    public static class WindowExtensions
    {
        #region Constants

        private static readonly FieldInfo ShowingAsDialogField;

        #endregion

        #region Static members

        public static bool IsModal(this Window window)
        {
            if (window == null) return false;
            return (bool)ShowingAsDialogField.GetValue(window);
        }

        #endregion

        #region Constructors

        static WindowExtensions()
        {
            ShowingAsDialogField = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        #endregion
    }
}