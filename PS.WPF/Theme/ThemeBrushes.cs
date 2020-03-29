using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using PS.Extensions;

namespace PS.WPF.Theme
{
    public static class ThemeBrushes
    {
        #region Constants

        private static readonly ConcurrentDictionary<int, Brush> Cache;

        #endregion

        #region Static members

        public static Brush Accent
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Accent)); }
        }

        public static Brush Accent25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Accent25)); }
        }

        public static Brush Accent50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Accent50)); }
        }

        public static Brush Accent75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Accent75)); }
        }

        public static Brush Basic
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Basic)); }
        }

        public static Brush Basic25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Basic25)); }
        }

        public static Brush Basic50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Basic50)); }
        }

        public static Brush Basic75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Basic75)); }
        }

        public static Brush Error
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Error)); }
        }

        public static Brush Highlight
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Highlight)); }
        }

        public static Brush Highlight25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Highlight25)); }
        }

        public static Brush Highlight50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Highlight50)); }
        }

        public static Brush Highlight75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Highlight75)); }
        }

        public static Brush Main
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Main)); }
        }

        public static Brush Main25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Main25)); }
        }

        public static Brush Main50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Main50)); }
        }

        public static Brush Main75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Main75)); }
        }

        public static Brush Marker
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Marker)); }
        }

        public static Brush Strong
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Strong)); }
        }

        public static Brush Success
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Success)); }
        }

        public static Brush Warning
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Warning)); }
        }

        public static Brush Warning25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Warning25)); }
        }

        public static Brush Warning50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Warning50)); }
        }

        public static Brush Warning75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Warning75)); }
        }

        private static int Key([CallerMemberName] string propertyName = null)
        {
            return Thread.CurrentThread.ManagedThreadId.GetHash().MergeHash(propertyName.GetHash());
        }

        #endregion

        #region Constructors

        static ThemeBrushes()
        {
            Cache = new ConcurrentDictionary<int, Brush>();
        }

        #endregion
    }
}