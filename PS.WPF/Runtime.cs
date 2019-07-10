using System.ComponentModel;
using System.Windows;

namespace PS.WPF
{
    public static class Runtime
    {
        #region Static members

        public static bool IsDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsDesignMode
        {
            get
            {
                return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty,
                                                                       typeof(DependencyObject)).Metadata.DefaultValue;
            }
        }

        #endregion
    }
}