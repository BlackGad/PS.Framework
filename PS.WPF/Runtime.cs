using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PS.WPF
{
    public static class Runtime
    {
        private static readonly IReadOnlyList<string> DebugFlagAliases;

        public static string DumpApplicationInformation()
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            if (currentAssembly == null)
            {
                return string.Empty;
            }

            var version = currentAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "<not specified>";
            var informationalVersion = currentAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "<not specified>";
            var framework = currentAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            var buildMode = IsDebugBuild ? "Debug" : "Release";
            var debuggingPanelState = "Disabled";
            if (IsDebugMode)
            {
                debuggingPanelState = "Active";
                if (IsDebugBuild)
                {
                    debuggingPanelState += " " + "(Debug build)";
                }
                else if (IsDebugModeForced)
                {
                    debuggingPanelState += " " + "(Forced via command line)";
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("Application debug information");
            sb.AppendLine($"* Framework: {framework}");
            sb.AppendLine($"* File version: {version}");
            sb.AppendLine($"* Informational version: {informationalVersion}");
            sb.AppendLine($"* Build: {buildMode}");
            sb.AppendLine($"* Debug panel: {debuggingPanelState}");

            return sb.ToString();
        }

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
