using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace PS.WPF
{
    public static class Runtime
    {
        #region Constants

        private static readonly IReadOnlyList<string> DebugFlagAliases;

        #endregion

        #region Static members

        public static bool IsDebugBuild
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

        public static bool IsDebugMode
        {
            get { return IsDebugBuild || IsDebugModeForced; }
        }

        public static bool IsDebugModeForced
        {
            get
            {
                var commandLine = Environment.CommandLine.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(commandLine)) return false;
                return DebugFlagAliases.Any(f => commandLine.Contains(f));
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

        #region Constructors

        static Runtime()
        {
            DebugFlagAliases = new List<string>
            {
                "-d",
                "--debug"
            };
        }

        #endregion
    }
}