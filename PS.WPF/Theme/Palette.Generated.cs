using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using PS.Extensions;
using PS.WPF.ValueConverters;

namespace PS.WPF.Theme
{
    public static class ThemeColors
    {
        static ThemeColors()
        {
            Accent = (Color)ColorConverter.ConvertFromString("#FFA52A2A");
            AccentForeground = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            AccentBorder = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            Main = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            MainForeground = (Color)ColorConverter.ConvertFromString("#FF000000");
            MainBorder = (Color)ColorConverter.ConvertFromString("#FFA9A9A9");
            Success = (Color)ColorConverter.ConvertFromString("#FF008000");
            Failure = (Color)ColorConverter.ConvertFromString("#FFFF0000");
            Warning = (Color)ColorConverter.ConvertFromString("#FFFFA500");
            AccentHigh = ColorConverters.ApplyTint(Accent, 0.2);
            AccentLow = ColorConverters.ApplyShade(Accent, 0.3);
            AccentDeep = ColorConverters.ApplyShade(Accent, 0.5);
            AccentBorderHigh = ColorConverters.ApplyTint(AccentBorder, 0.2);
            AccentBorderLow = ColorConverters.ApplyShade(AccentBorder, 0.3);
            AccentBorderDeep = ColorConverters.ApplyShade(AccentBorder, 0.5);
            MainHigh = ColorConverters.ApplyTint(Accent, 0.2);
            MainLow = ColorConverters.ApplyShade(Main, 0.3);
            MainDeep = ColorConverters.ApplyShade(Main, 0.5);
            MainBorderHigh = ColorConverters.ApplyTint(MainBorder, 0.5);
            MainBorderLow = ColorConverters.ApplyShade(MainBorder, 0.3);
            MainBorderDeep = ColorConverters.ApplyShade(MainBorder, 0.5);
        }

        private static Color _accent;
        public static Color Accent
        {
            get { return _accent; }
            set
            {
                _accent = value;
                Accent75 = ColorConverters.ApplyOpacity(value, 0.75);
                Accent50 = ColorConverters.ApplyOpacity(value, 0.50);
                Accent25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color Accent25 { get; private set; }
        public static Color Accent50 { get; private set; }
        public static Color Accent75 { get; private set; }

        private static Color _accentHigh;
        public static Color AccentHigh
        {
            get { return _accentHigh; }
            set
            {
                _accentHigh = value;
                AccentHigh75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentHigh50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentHigh25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentHigh25 { get; private set; }
        public static Color AccentHigh50 { get; private set; }
        public static Color AccentHigh75 { get; private set; }

        private static Color _accentLow;
        public static Color AccentLow
        {
            get { return _accentLow; }
            set
            {
                _accentLow = value;
                AccentLow75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentLow50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentLow25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentLow25 { get; private set; }
        public static Color AccentLow50 { get; private set; }
        public static Color AccentLow75 { get; private set; }

        private static Color _accentDeep;
        public static Color AccentDeep
        {
            get { return _accentDeep; }
            set
            {
                _accentDeep = value;
                AccentDeep75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentDeep50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentDeep25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentDeep25 { get; private set; }
        public static Color AccentDeep50 { get; private set; }
        public static Color AccentDeep75 { get; private set; }

        private static Color _accentForeground;
        public static Color AccentForeground
        {
            get { return _accentForeground; }
            set
            {
                _accentForeground = value;
                AccentForeground75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentForeground50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentForeground25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentForeground25 { get; private set; }
        public static Color AccentForeground50 { get; private set; }
        public static Color AccentForeground75 { get; private set; }

        private static Color _accentBorder;
        public static Color AccentBorder
        {
            get { return _accentBorder; }
            set
            {
                _accentBorder = value;
                AccentBorder75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentBorder50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentBorder25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentBorder25 { get; private set; }
        public static Color AccentBorder50 { get; private set; }
        public static Color AccentBorder75 { get; private set; }

        private static Color _accentBorderHigh;
        public static Color AccentBorderHigh
        {
            get { return _accentBorderHigh; }
            set
            {
                _accentBorderHigh = value;
                AccentBorderHigh75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentBorderHigh50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentBorderHigh25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentBorderHigh25 { get; private set; }
        public static Color AccentBorderHigh50 { get; private set; }
        public static Color AccentBorderHigh75 { get; private set; }

        private static Color _accentBorderLow;
        public static Color AccentBorderLow
        {
            get { return _accentBorderLow; }
            set
            {
                _accentBorderLow = value;
                AccentBorderLow75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentBorderLow50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentBorderLow25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentBorderLow25 { get; private set; }
        public static Color AccentBorderLow50 { get; private set; }
        public static Color AccentBorderLow75 { get; private set; }

        private static Color _accentBorderDeep;
        public static Color AccentBorderDeep
        {
            get { return _accentBorderDeep; }
            set
            {
                _accentBorderDeep = value;
                AccentBorderDeep75 = ColorConverters.ApplyOpacity(value, 0.75);
                AccentBorderDeep50 = ColorConverters.ApplyOpacity(value, 0.50);
                AccentBorderDeep25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color AccentBorderDeep25 { get; private set; }
        public static Color AccentBorderDeep50 { get; private set; }
        public static Color AccentBorderDeep75 { get; private set; }

        private static Color _main;
        public static Color Main
        {
            get { return _main; }
            set
            {
                _main = value;
                Main75 = ColorConverters.ApplyOpacity(value, 0.75);
                Main50 = ColorConverters.ApplyOpacity(value, 0.50);
                Main25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color Main25 { get; private set; }
        public static Color Main50 { get; private set; }
        public static Color Main75 { get; private set; }

        private static Color _mainHigh;
        public static Color MainHigh
        {
            get { return _mainHigh; }
            set
            {
                _mainHigh = value;
                MainHigh75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainHigh50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainHigh25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainHigh25 { get; private set; }
        public static Color MainHigh50 { get; private set; }
        public static Color MainHigh75 { get; private set; }

        private static Color _mainLow;
        public static Color MainLow
        {
            get { return _mainLow; }
            set
            {
                _mainLow = value;
                MainLow75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainLow50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainLow25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainLow25 { get; private set; }
        public static Color MainLow50 { get; private set; }
        public static Color MainLow75 { get; private set; }

        private static Color _mainDeep;
        public static Color MainDeep
        {
            get { return _mainDeep; }
            set
            {
                _mainDeep = value;
                MainDeep75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainDeep50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainDeep25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainDeep25 { get; private set; }
        public static Color MainDeep50 { get; private set; }
        public static Color MainDeep75 { get; private set; }

        private static Color _mainForeground;
        public static Color MainForeground
        {
            get { return _mainForeground; }
            set
            {
                _mainForeground = value;
                MainForeground75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainForeground50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainForeground25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainForeground25 { get; private set; }
        public static Color MainForeground50 { get; private set; }
        public static Color MainForeground75 { get; private set; }

        private static Color _mainBorder;
        public static Color MainBorder
        {
            get { return _mainBorder; }
            set
            {
                _mainBorder = value;
                MainBorder75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainBorder50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainBorder25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainBorder25 { get; private set; }
        public static Color MainBorder50 { get; private set; }
        public static Color MainBorder75 { get; private set; }

        private static Color _mainBorderHigh;
        public static Color MainBorderHigh
        {
            get { return _mainBorderHigh; }
            set
            {
                _mainBorderHigh = value;
                MainBorderHigh75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainBorderHigh50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainBorderHigh25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainBorderHigh25 { get; private set; }
        public static Color MainBorderHigh50 { get; private set; }
        public static Color MainBorderHigh75 { get; private set; }

        private static Color _mainBorderLow;
        public static Color MainBorderLow
        {
            get { return _mainBorderLow; }
            set
            {
                _mainBorderLow = value;
                MainBorderLow75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainBorderLow50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainBorderLow25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainBorderLow25 { get; private set; }
        public static Color MainBorderLow50 { get; private set; }
        public static Color MainBorderLow75 { get; private set; }

        private static Color _mainBorderDeep;
        public static Color MainBorderDeep
        {
            get { return _mainBorderDeep; }
            set
            {
                _mainBorderDeep = value;
                MainBorderDeep75 = ColorConverters.ApplyOpacity(value, 0.75);
                MainBorderDeep50 = ColorConverters.ApplyOpacity(value, 0.50);
                MainBorderDeep25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color MainBorderDeep25 { get; private set; }
        public static Color MainBorderDeep50 { get; private set; }
        public static Color MainBorderDeep75 { get; private set; }

        private static Color _success;
        public static Color Success
        {
            get { return _success; }
            set
            {
                _success = value;
                Success75 = ColorConverters.ApplyOpacity(value, 0.75);
                Success50 = ColorConverters.ApplyOpacity(value, 0.50);
                Success25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color Success25 { get; private set; }
        public static Color Success50 { get; private set; }
        public static Color Success75 { get; private set; }

        private static Color _failure;
        public static Color Failure
        {
            get { return _failure; }
            set
            {
                _failure = value;
                Failure75 = ColorConverters.ApplyOpacity(value, 0.75);
                Failure50 = ColorConverters.ApplyOpacity(value, 0.50);
                Failure25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color Failure25 { get; private set; }
        public static Color Failure50 { get; private set; }
        public static Color Failure75 { get; private set; }

        private static Color _warning;
        public static Color Warning
        {
            get { return _warning; }
            set
            {
                _warning = value;
                Warning75 = ColorConverters.ApplyOpacity(value, 0.75);
                Warning50 = ColorConverters.ApplyOpacity(value, 0.50);
                Warning25 = ColorConverters.ApplyOpacity(value, 0.25);
            }
        }

        public static Color Warning25 { get; private set; }
        public static Color Warning50 { get; private set; }
        public static Color Warning75 { get; private set; }

	}

    public static class ThemeBrushes
    {
        private static readonly ConcurrentDictionary<int, Brush> Cache = new ConcurrentDictionary<int, Brush>();

        private static int Key([CallerMemberName] string propertyName = null)
        {
            return Thread.CurrentThread.ManagedThreadId.GetHash().MergeHash(propertyName.GetHash());
        }

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
        public static Brush AccentHigh
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentHigh)); }
        }

        public static Brush AccentHigh25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentHigh25)); }
        }

        public static Brush AccentHigh50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentHigh50)); }
        }

        public static Brush AccentHigh75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentHigh75)); }
        }
        public static Brush AccentLow
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentLow)); }
        }

        public static Brush AccentLow25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentLow25)); }
        }

        public static Brush AccentLow50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentLow50)); }
        }

        public static Brush AccentLow75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentLow75)); }
        }
        public static Brush AccentDeep
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentDeep)); }
        }

        public static Brush AccentDeep25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentDeep25)); }
        }

        public static Brush AccentDeep50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentDeep50)); }
        }

        public static Brush AccentDeep75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentDeep75)); }
        }
        public static Brush AccentForeground
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentForeground)); }
        }

        public static Brush AccentForeground25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentForeground25)); }
        }

        public static Brush AccentForeground50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentForeground50)); }
        }

        public static Brush AccentForeground75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentForeground75)); }
        }
        public static Brush AccentBorder
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorder)); }
        }

        public static Brush AccentBorder25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorder25)); }
        }

        public static Brush AccentBorder50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorder50)); }
        }

        public static Brush AccentBorder75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorder75)); }
        }
        public static Brush AccentBorderHigh
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderHigh)); }
        }

        public static Brush AccentBorderHigh25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderHigh25)); }
        }

        public static Brush AccentBorderHigh50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderHigh50)); }
        }

        public static Brush AccentBorderHigh75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderHigh75)); }
        }
        public static Brush AccentBorderLow
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderLow)); }
        }

        public static Brush AccentBorderLow25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderLow25)); }
        }

        public static Brush AccentBorderLow50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderLow50)); }
        }

        public static Brush AccentBorderLow75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderLow75)); }
        }
        public static Brush AccentBorderDeep
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderDeep)); }
        }

        public static Brush AccentBorderDeep25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderDeep25)); }
        }

        public static Brush AccentBorderDeep50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderDeep50)); }
        }

        public static Brush AccentBorderDeep75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.AccentBorderDeep75)); }
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
        public static Brush MainHigh
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainHigh)); }
        }

        public static Brush MainHigh25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainHigh25)); }
        }

        public static Brush MainHigh50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainHigh50)); }
        }

        public static Brush MainHigh75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainHigh75)); }
        }
        public static Brush MainLow
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainLow)); }
        }

        public static Brush MainLow25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainLow25)); }
        }

        public static Brush MainLow50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainLow50)); }
        }

        public static Brush MainLow75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainLow75)); }
        }
        public static Brush MainDeep
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainDeep)); }
        }

        public static Brush MainDeep25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainDeep25)); }
        }

        public static Brush MainDeep50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainDeep50)); }
        }

        public static Brush MainDeep75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainDeep75)); }
        }
        public static Brush MainForeground
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainForeground)); }
        }

        public static Brush MainForeground25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainForeground25)); }
        }

        public static Brush MainForeground50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainForeground50)); }
        }

        public static Brush MainForeground75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainForeground75)); }
        }
        public static Brush MainBorder
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorder)); }
        }

        public static Brush MainBorder25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorder25)); }
        }

        public static Brush MainBorder50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorder50)); }
        }

        public static Brush MainBorder75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorder75)); }
        }
        public static Brush MainBorderHigh
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderHigh)); }
        }

        public static Brush MainBorderHigh25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderHigh25)); }
        }

        public static Brush MainBorderHigh50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderHigh50)); }
        }

        public static Brush MainBorderHigh75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderHigh75)); }
        }
        public static Brush MainBorderLow
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderLow)); }
        }

        public static Brush MainBorderLow25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderLow25)); }
        }

        public static Brush MainBorderLow50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderLow50)); }
        }

        public static Brush MainBorderLow75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderLow75)); }
        }
        public static Brush MainBorderDeep
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderDeep)); }
        }

        public static Brush MainBorderDeep25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderDeep25)); }
        }

        public static Brush MainBorderDeep50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderDeep50)); }
        }

        public static Brush MainBorderDeep75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.MainBorderDeep75)); }
        }
        public static Brush Success
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Success)); }
        }

        public static Brush Success25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Success25)); }
        }

        public static Brush Success50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Success50)); }
        }

        public static Brush Success75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Success75)); }
        }
        public static Brush Failure
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Failure)); }
        }

        public static Brush Failure25
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Failure25)); }
        }

        public static Brush Failure50
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Failure50)); }
        }

        public static Brush Failure75
        {
            get { return Cache.GetOrAdd(Key(), id => new SolidColorBrush(ThemeColors.Failure75)); }
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
    }

    public class ThemePreset
    {

    }
}

