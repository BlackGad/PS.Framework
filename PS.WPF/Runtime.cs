using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PS.WPF
{
    public static class Runtime
    {
        private static readonly IReadOnlyList<string> DebugFlagAliases;

        public static ImageSource GetApplicationIcon()
        {
            var iconDecoder = new IconBitmapDecoder(
                new Uri(@"pack://application:,,/icon.ico", UriKind.RelativeOrAbsolute),
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
            var frame = iconDecoder.Frames.FirstOrDefault(f => f.Width <= SystemParameters.IconWidth) ?? iconDecoder.Frames[0];
            return frame;
        }

        public static string GetApplicationName()
        {
            var assembly = Assembly.GetEntryAssembly();
            var title = assembly?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

            return title;
        }

        public static string GetApplicationTitle()
        {
            var title = GetApplicationName();
            var version = GetApplicationVersion();
            if (!string.IsNullOrWhiteSpace(version))
            {
                title += ": " + version;
            }

            if (IsDebugBuild) title += " (DEBUG)";
            return title;
        }

        public static string GetApplicationVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>() is AssemblyInformationalVersionAttribute assemblyInformationalVersionAttribute)
            {
                return assemblyInformationalVersionAttribute.InformationalVersion;
            }

            if (assembly?.GetCustomAttribute<AssemblyVersionAttribute>() is AssemblyVersionAttribute assemblyVersionAttribute)
            {
                return assemblyVersionAttribute.Version;
            }

            if (assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>() is AssemblyFileVersionAttribute assemblyFileVersionAttribute)
            {
                return assemblyFileVersionAttribute.Version;
            }

            return string.Empty;
        }

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
            get { return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue; }
        }

        static Runtime()
        {
            DebugFlagAliases = new List<string>
            {
                "-d",
                "--debug"
            };
        }
    }
}
