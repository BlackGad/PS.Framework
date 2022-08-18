using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PS.Extensions
{
    public static class AssemblyExtensions
    {
        public static string LoadString(this Assembly assembly, string resourceName)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentNullException(nameof(resourceName));
            try
            {
                var name = assembly.ResolveResourceName(resourceName);
                if (name != null)
                {
                    var manifestResourceStream = assembly.GetManifestResourceStream(name);
                    if (manifestResourceStream != null)
                    {
                        using (var reader = new StreamReader(manifestResourceStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                //Nothing
            }

            return string.Empty;
        }

        public static string ResolveResourceName(this Assembly assembly, string resourceName)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentNullException(nameof(resourceName));
            var existingNames = assembly.GetManifestResourceNames();
            resourceName = resourceName.ResolveResourceNamespace();
            var name = existingNames.FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase));
            return name;
        }

        public static string ResolveResourceNamespace(this string resourceNamespace)
        {
            if (string.IsNullOrWhiteSpace(resourceNamespace)) throw new ArgumentNullException(nameof(resourceNamespace));
            return resourceNamespace.Replace("/", ".").Replace("\\", ".");
        }
    }
}
