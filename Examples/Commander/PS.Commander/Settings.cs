using System;
using System.Collections.Generic;
using System.Configuration;

namespace PS.Commander
{
    public static class Settings
    {
        #region Constants

        private static readonly IReadOnlyDictionary<string, object> Defaults;
        public static readonly string SettingsFolder = nameof(SettingsFolder);

        #endregion

        #region Static members

        public static T Get<T>(string settingName)
        {
            if (string.IsNullOrWhiteSpace(settingName)) throw new ArgumentNullException(nameof(settingName));
            if (!Defaults.ContainsKey(settingName)) throw new InvalidOperationException($"Not declared '{settingName}' setting");

            var setting = ConfigurationManager.AppSettings[settingName];
            if (setting == null) return (T)Defaults[settingName];

            return SafeConvert<T>(setting);
        }

        private static T SafeConvert<T>(string value)
        {
            var type = typeof(T);

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            if (underlyingType.IsEnum)
            {
                return string.IsNullOrWhiteSpace(value)
                    ? default(T)
                    : (T)Enum.Parse(underlyingType, value);
            }

            return string.IsNullOrWhiteSpace(value)
                ? default(T)
                : (T)Convert.ChangeType(value, underlyingType);
        }

        #endregion

        #region Constructors

        static Settings()
        {
            Defaults = new Dictionary<string, object>
            {
                { SettingsFolder, @"%APPDATA%\PS.Commander\" }
            };
        }

        #endregion
    }
}