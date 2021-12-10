using System;
using System.ComponentModel;
using System.Windows.Media;
using PS.WPF.ValueConverters;

namespace PS.WPF.Theming
{
    public enum ThemeColor
    {
        Accent,
        AccentHigh,
        AccentLow,
        AccentDeep,
        AccentForeground,
        AccentBorder,
        AccentBorderHigh,
        AccentBorderLow,
        AccentBorderDeep,
        Main,
        MainHigh,
        MainLow,
        MainDeep,
        MainForeground,
        MainBorder,
        MainBorderHigh,
        MainBorderLow,
        MainBorderDeep,
        Success,
        Failure,
        Warning,
    }
    
    public enum ThemeColorOpacity
    {
        [DefaultValue(1.0)]
        None,
        [DefaultValue(0.75)]
        [PostfixAttribute("75")]
        O75,
        [DefaultValue(0.5)]
        [PostfixAttribute("50")]
        O50,
        [DefaultValue(0.25)]
        [PostfixAttribute("25")]
        O25,
    }

    public enum ThemeFont
    {
        Normal,
        Light,
        Strong,
        ExtraStrong,
    }

    public enum ThemeFontSize
    {
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        XXXL,
        XXXXL,
    }

    public sealed class ThemeFonts : BaseNotifyPropertyChanged
    {
        private FontFamily _normal;
        public FontFamily Normal
        {
            get { return _normal; }
            set { SetField(ref _normal, value); }
        }
        private FontFamily _light;
        public FontFamily Light
        {
            get { return _light; }
            set { SetField(ref _light, value); }
        }
        private FontFamily _strong;
        public FontFamily Strong
        {
            get { return _strong; }
            set { SetField(ref _strong, value); }
        }
        private FontFamily _extraStrong;
        public FontFamily ExtraStrong
        {
            get { return _extraStrong; }
            set { SetField(ref _extraStrong, value); }
        }

        public FontFamily this[ThemeFont font]
        {
            get
            {
                switch(font)
                {
                    case ThemeFont.Normal: return Normal;
                    case ThemeFont.Light: return Light;
                    case ThemeFont.Strong: return Strong;
                    case ThemeFont.ExtraStrong: return ExtraStrong;
                    default:
                        throw new ArgumentException($"{font} is unknown");
                }
            }
            set
            {
                switch(font)
                {
                    case ThemeFont.Normal: Normal = value; break;
                    case ThemeFont.Light: Light = value; break;
                    case ThemeFont.Strong: Strong = value; break;
                    case ThemeFont.ExtraStrong: ExtraStrong = value; break;
                    default:
                        throw new ArgumentException($"{font} is unknown");
                }
            }
        }
    }

    public sealed class ThemeFontSizes : BaseNotifyPropertyChanged
    {
        private double _xS;
        public double XS
        {
            get { return _xS; }
            set { SetField(ref _xS, value); }
        }
        private double _s;
        public double S
        {
            get { return _s; }
            set { SetField(ref _s, value); }
        }
        private double _m;
        public double M
        {
            get { return _m; }
            set { SetField(ref _m, value); }
        }
        private double _l;
        public double L
        {
            get { return _l; }
            set { SetField(ref _l, value); }
        }
        private double _xL;
        public double XL
        {
            get { return _xL; }
            set { SetField(ref _xL, value); }
        }
        private double _xXL;
        public double XXL
        {
            get { return _xXL; }
            set { SetField(ref _xXL, value); }
        }
        private double _xXXL;
        public double XXXL
        {
            get { return _xXXL; }
            set { SetField(ref _xXXL, value); }
        }
        private double _xXXXL;
        public double XXXXL
        {
            get { return _xXXXL; }
            set { SetField(ref _xXXXL, value); }
        }

        public double this[ThemeFontSize fontSize]
        {
            get
            {
                switch(fontSize)
                {
                    case ThemeFontSize.XS: return XS;
                    case ThemeFontSize.S: return S;
                    case ThemeFontSize.M: return M;
                    case ThemeFontSize.L: return L;
                    case ThemeFontSize.XL: return XL;
                    case ThemeFontSize.XXL: return XXL;
                    case ThemeFontSize.XXXL: return XXXL;
                    case ThemeFontSize.XXXXL: return XXXXL;
                    default:
                        throw new ArgumentException($"{fontSize} is unknown");
                }
            }
            set
            {
                switch(fontSize)
                {
                    case ThemeFontSize.XS: XS = value; break;
                    case ThemeFontSize.S: S = value; break;
                    case ThemeFontSize.M: M = value; break;
                    case ThemeFontSize.L: L = value; break;
                    case ThemeFontSize.XL: XL = value; break;
                    case ThemeFontSize.XXL: XXL = value; break;
                    case ThemeFontSize.XXXL: XXXL = value; break;
                    case ThemeFontSize.XXXXL: XXXXL = value; break;
                    default:
                        throw new ArgumentException($"{fontSize} is unknown");
                }
            }
        }
    }

    public sealed class ThemeColors : BaseNotifyPropertyChanged
    {
        private Color _accent;
        public Color Accent
        {
            get { return _accent; }
            set
            {
                if(SetField(ref _accent, value))
                {
                    Accent75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("Accent75");
                    Accent50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("Accent50");
                    Accent25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("Accent25");

                    var args = new ColorChangedEventArgs("Accent", 
                                                         Accent75,
                                                         Accent50,
                                                         Accent25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color Accent75 { get; private set; }
        public Color Accent50 { get; private set; }
        public Color Accent25 { get; private set; }

        private Color _accentHigh;
        public Color AccentHigh
        {
            get { return _accentHigh; }
            set
            {
                if(SetField(ref _accentHigh, value))
                {
                    AccentHigh75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentHigh75");
                    AccentHigh50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentHigh50");
                    AccentHigh25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentHigh25");

                    var args = new ColorChangedEventArgs("AccentHigh", 
                                                         AccentHigh75,
                                                         AccentHigh50,
                                                         AccentHigh25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentHigh75 { get; private set; }
        public Color AccentHigh50 { get; private set; }
        public Color AccentHigh25 { get; private set; }

        private Color _accentLow;
        public Color AccentLow
        {
            get { return _accentLow; }
            set
            {
                if(SetField(ref _accentLow, value))
                {
                    AccentLow75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentLow75");
                    AccentLow50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentLow50");
                    AccentLow25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentLow25");

                    var args = new ColorChangedEventArgs("AccentLow", 
                                                         AccentLow75,
                                                         AccentLow50,
                                                         AccentLow25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentLow75 { get; private set; }
        public Color AccentLow50 { get; private set; }
        public Color AccentLow25 { get; private set; }

        private Color _accentDeep;
        public Color AccentDeep
        {
            get { return _accentDeep; }
            set
            {
                if(SetField(ref _accentDeep, value))
                {
                    AccentDeep75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentDeep75");
                    AccentDeep50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentDeep50");
                    AccentDeep25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentDeep25");

                    var args = new ColorChangedEventArgs("AccentDeep", 
                                                         AccentDeep75,
                                                         AccentDeep50,
                                                         AccentDeep25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentDeep75 { get; private set; }
        public Color AccentDeep50 { get; private set; }
        public Color AccentDeep25 { get; private set; }

        private Color _accentForeground;
        public Color AccentForeground
        {
            get { return _accentForeground; }
            set
            {
                if(SetField(ref _accentForeground, value))
                {
                    AccentForeground75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentForeground75");
                    AccentForeground50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentForeground50");
                    AccentForeground25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentForeground25");

                    var args = new ColorChangedEventArgs("AccentForeground", 
                                                         AccentForeground75,
                                                         AccentForeground50,
                                                         AccentForeground25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentForeground75 { get; private set; }
        public Color AccentForeground50 { get; private set; }
        public Color AccentForeground25 { get; private set; }

        private Color _accentBorder;
        public Color AccentBorder
        {
            get { return _accentBorder; }
            set
            {
                if(SetField(ref _accentBorder, value))
                {
                    AccentBorder75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentBorder75");
                    AccentBorder50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentBorder50");
                    AccentBorder25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentBorder25");

                    var args = new ColorChangedEventArgs("AccentBorder", 
                                                         AccentBorder75,
                                                         AccentBorder50,
                                                         AccentBorder25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentBorder75 { get; private set; }
        public Color AccentBorder50 { get; private set; }
        public Color AccentBorder25 { get; private set; }

        private Color _accentBorderHigh;
        public Color AccentBorderHigh
        {
            get { return _accentBorderHigh; }
            set
            {
                if(SetField(ref _accentBorderHigh, value))
                {
                    AccentBorderHigh75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentBorderHigh75");
                    AccentBorderHigh50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentBorderHigh50");
                    AccentBorderHigh25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentBorderHigh25");

                    var args = new ColorChangedEventArgs("AccentBorderHigh", 
                                                         AccentBorderHigh75,
                                                         AccentBorderHigh50,
                                                         AccentBorderHigh25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentBorderHigh75 { get; private set; }
        public Color AccentBorderHigh50 { get; private set; }
        public Color AccentBorderHigh25 { get; private set; }

        private Color _accentBorderLow;
        public Color AccentBorderLow
        {
            get { return _accentBorderLow; }
            set
            {
                if(SetField(ref _accentBorderLow, value))
                {
                    AccentBorderLow75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentBorderLow75");
                    AccentBorderLow50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentBorderLow50");
                    AccentBorderLow25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentBorderLow25");

                    var args = new ColorChangedEventArgs("AccentBorderLow", 
                                                         AccentBorderLow75,
                                                         AccentBorderLow50,
                                                         AccentBorderLow25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentBorderLow75 { get; private set; }
        public Color AccentBorderLow50 { get; private set; }
        public Color AccentBorderLow25 { get; private set; }

        private Color _accentBorderDeep;
        public Color AccentBorderDeep
        {
            get { return _accentBorderDeep; }
            set
            {
                if(SetField(ref _accentBorderDeep, value))
                {
                    AccentBorderDeep75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("AccentBorderDeep75");
                    AccentBorderDeep50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("AccentBorderDeep50");
                    AccentBorderDeep25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("AccentBorderDeep25");

                    var args = new ColorChangedEventArgs("AccentBorderDeep", 
                                                         AccentBorderDeep75,
                                                         AccentBorderDeep50,
                                                         AccentBorderDeep25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color AccentBorderDeep75 { get; private set; }
        public Color AccentBorderDeep50 { get; private set; }
        public Color AccentBorderDeep25 { get; private set; }

        private Color _main;
        public Color Main
        {
            get { return _main; }
            set
            {
                if(SetField(ref _main, value))
                {
                    Main75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("Main75");
                    Main50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("Main50");
                    Main25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("Main25");

                    var args = new ColorChangedEventArgs("Main", 
                                                         Main75,
                                                         Main50,
                                                         Main25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color Main75 { get; private set; }
        public Color Main50 { get; private set; }
        public Color Main25 { get; private set; }

        private Color _mainHigh;
        public Color MainHigh
        {
            get { return _mainHigh; }
            set
            {
                if(SetField(ref _mainHigh, value))
                {
                    MainHigh75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainHigh75");
                    MainHigh50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainHigh50");
                    MainHigh25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainHigh25");

                    var args = new ColorChangedEventArgs("MainHigh", 
                                                         MainHigh75,
                                                         MainHigh50,
                                                         MainHigh25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainHigh75 { get; private set; }
        public Color MainHigh50 { get; private set; }
        public Color MainHigh25 { get; private set; }

        private Color _mainLow;
        public Color MainLow
        {
            get { return _mainLow; }
            set
            {
                if(SetField(ref _mainLow, value))
                {
                    MainLow75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainLow75");
                    MainLow50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainLow50");
                    MainLow25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainLow25");

                    var args = new ColorChangedEventArgs("MainLow", 
                                                         MainLow75,
                                                         MainLow50,
                                                         MainLow25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainLow75 { get; private set; }
        public Color MainLow50 { get; private set; }
        public Color MainLow25 { get; private set; }

        private Color _mainDeep;
        public Color MainDeep
        {
            get { return _mainDeep; }
            set
            {
                if(SetField(ref _mainDeep, value))
                {
                    MainDeep75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainDeep75");
                    MainDeep50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainDeep50");
                    MainDeep25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainDeep25");

                    var args = new ColorChangedEventArgs("MainDeep", 
                                                         MainDeep75,
                                                         MainDeep50,
                                                         MainDeep25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainDeep75 { get; private set; }
        public Color MainDeep50 { get; private set; }
        public Color MainDeep25 { get; private set; }

        private Color _mainForeground;
        public Color MainForeground
        {
            get { return _mainForeground; }
            set
            {
                if(SetField(ref _mainForeground, value))
                {
                    MainForeground75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainForeground75");
                    MainForeground50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainForeground50");
                    MainForeground25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainForeground25");

                    var args = new ColorChangedEventArgs("MainForeground", 
                                                         MainForeground75,
                                                         MainForeground50,
                                                         MainForeground25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainForeground75 { get; private set; }
        public Color MainForeground50 { get; private set; }
        public Color MainForeground25 { get; private set; }

        private Color _mainBorder;
        public Color MainBorder
        {
            get { return _mainBorder; }
            set
            {
                if(SetField(ref _mainBorder, value))
                {
                    MainBorder75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainBorder75");
                    MainBorder50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainBorder50");
                    MainBorder25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainBorder25");

                    var args = new ColorChangedEventArgs("MainBorder", 
                                                         MainBorder75,
                                                         MainBorder50,
                                                         MainBorder25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainBorder75 { get; private set; }
        public Color MainBorder50 { get; private set; }
        public Color MainBorder25 { get; private set; }

        private Color _mainBorderHigh;
        public Color MainBorderHigh
        {
            get { return _mainBorderHigh; }
            set
            {
                if(SetField(ref _mainBorderHigh, value))
                {
                    MainBorderHigh75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainBorderHigh75");
                    MainBorderHigh50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainBorderHigh50");
                    MainBorderHigh25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainBorderHigh25");

                    var args = new ColorChangedEventArgs("MainBorderHigh", 
                                                         MainBorderHigh75,
                                                         MainBorderHigh50,
                                                         MainBorderHigh25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainBorderHigh75 { get; private set; }
        public Color MainBorderHigh50 { get; private set; }
        public Color MainBorderHigh25 { get; private set; }

        private Color _mainBorderLow;
        public Color MainBorderLow
        {
            get { return _mainBorderLow; }
            set
            {
                if(SetField(ref _mainBorderLow, value))
                {
                    MainBorderLow75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainBorderLow75");
                    MainBorderLow50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainBorderLow50");
                    MainBorderLow25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainBorderLow25");

                    var args = new ColorChangedEventArgs("MainBorderLow", 
                                                         MainBorderLow75,
                                                         MainBorderLow50,
                                                         MainBorderLow25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainBorderLow75 { get; private set; }
        public Color MainBorderLow50 { get; private set; }
        public Color MainBorderLow25 { get; private set; }

        private Color _mainBorderDeep;
        public Color MainBorderDeep
        {
            get { return _mainBorderDeep; }
            set
            {
                if(SetField(ref _mainBorderDeep, value))
                {
                    MainBorderDeep75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("MainBorderDeep75");
                    MainBorderDeep50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("MainBorderDeep50");
                    MainBorderDeep25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("MainBorderDeep25");

                    var args = new ColorChangedEventArgs("MainBorderDeep", 
                                                         MainBorderDeep75,
                                                         MainBorderDeep50,
                                                         MainBorderDeep25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color MainBorderDeep75 { get; private set; }
        public Color MainBorderDeep50 { get; private set; }
        public Color MainBorderDeep25 { get; private set; }

        private Color _success;
        public Color Success
        {
            get { return _success; }
            set
            {
                if(SetField(ref _success, value))
                {
                    Success75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("Success75");
                    Success50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("Success50");
                    Success25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("Success25");

                    var args = new ColorChangedEventArgs("Success", 
                                                         Success75,
                                                         Success50,
                                                         Success25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color Success75 { get; private set; }
        public Color Success50 { get; private set; }
        public Color Success25 { get; private set; }

        private Color _failure;
        public Color Failure
        {
            get { return _failure; }
            set
            {
                if(SetField(ref _failure, value))
                {
                    Failure75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("Failure75");
                    Failure50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("Failure50");
                    Failure25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("Failure25");

                    var args = new ColorChangedEventArgs("Failure", 
                                                         Failure75,
                                                         Failure50,
                                                         Failure25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color Failure75 { get; private set; }
        public Color Failure50 { get; private set; }
        public Color Failure25 { get; private set; }

        private Color _warning;
        public Color Warning
        {
            get { return _warning; }
            set
            {
                if(SetField(ref _warning, value))
                {
                    Warning75 = ColorConverters.ApplyOpacity(value, 0.75); OnPropertyChanged("Warning75");
                    Warning50 = ColorConverters.ApplyOpacity(value, 0.5); OnPropertyChanged("Warning50");
                    Warning25 = ColorConverters.ApplyOpacity(value, 0.25); OnPropertyChanged("Warning25");

                    var args = new ColorChangedEventArgs("Warning", 
                                                         Warning75,
                                                         Warning50,
                                                         Warning25,
                                                         value);
                    ColorChanged?.Invoke(this, args);
                }
            }
        }
        public Color Warning75 { get; private set; }
        public Color Warning50 { get; private set; }
        public Color Warning25 { get; private set; }


        public Color this[ThemeColor color]
        {
            get
            {
                switch(color)
                {
                    case ThemeColor.Accent: return Accent;
                    case ThemeColor.AccentHigh: return AccentHigh;
                    case ThemeColor.AccentLow: return AccentLow;
                    case ThemeColor.AccentDeep: return AccentDeep;
                    case ThemeColor.AccentForeground: return AccentForeground;
                    case ThemeColor.AccentBorder: return AccentBorder;
                    case ThemeColor.AccentBorderHigh: return AccentBorderHigh;
                    case ThemeColor.AccentBorderLow: return AccentBorderLow;
                    case ThemeColor.AccentBorderDeep: return AccentBorderDeep;
                    case ThemeColor.Main: return Main;
                    case ThemeColor.MainHigh: return MainHigh;
                    case ThemeColor.MainLow: return MainLow;
                    case ThemeColor.MainDeep: return MainDeep;
                    case ThemeColor.MainForeground: return MainForeground;
                    case ThemeColor.MainBorder: return MainBorder;
                    case ThemeColor.MainBorderHigh: return MainBorderHigh;
                    case ThemeColor.MainBorderLow: return MainBorderLow;
                    case ThemeColor.MainBorderDeep: return MainBorderDeep;
                    case ThemeColor.Success: return Success;
                    case ThemeColor.Failure: return Failure;
                    case ThemeColor.Warning: return Warning;
                    default:
                        throw new ArgumentException($"{color} is unknown");
                }
            }
            set
            {
                switch(color)
                {
                    case ThemeColor.Accent: Accent = value; break;
                    case ThemeColor.AccentHigh: AccentHigh = value; break;
                    case ThemeColor.AccentLow: AccentLow = value; break;
                    case ThemeColor.AccentDeep: AccentDeep = value; break;
                    case ThemeColor.AccentForeground: AccentForeground = value; break;
                    case ThemeColor.AccentBorder: AccentBorder = value; break;
                    case ThemeColor.AccentBorderHigh: AccentBorderHigh = value; break;
                    case ThemeColor.AccentBorderLow: AccentBorderLow = value; break;
                    case ThemeColor.AccentBorderDeep: AccentBorderDeep = value; break;
                    case ThemeColor.Main: Main = value; break;
                    case ThemeColor.MainHigh: MainHigh = value; break;
                    case ThemeColor.MainLow: MainLow = value; break;
                    case ThemeColor.MainDeep: MainDeep = value; break;
                    case ThemeColor.MainForeground: MainForeground = value; break;
                    case ThemeColor.MainBorder: MainBorder = value; break;
                    case ThemeColor.MainBorderHigh: MainBorderHigh = value; break;
                    case ThemeColor.MainBorderLow: MainBorderLow = value; break;
                    case ThemeColor.MainBorderDeep: MainBorderDeep = value; break;
                    case ThemeColor.Success: Success = value; break;
                    case ThemeColor.Failure: Failure = value; break;
                    case ThemeColor.Warning: Warning = value; break;
                    default:
                        throw new ArgumentException($"{color} is unknown");
                }
            }
        }

        public event EventHandler<ColorChangedEventArgs> ColorChanged;      
    }

    public sealed class ThemeBrushes : BaseNotifyPropertyChanged
    {
        internal ThemeBrushes(ThemeColors colors)
        {
            if (colors == null) throw new ArgumentNullException(nameof(colors));
            colors.ColorChanged += OnColorChanged;
        }
        public Brush Accent { get; private set; }
        public Brush Accent75 { get; private set; }
        public Brush Accent50 { get; private set; }
        public Brush Accent25 { get; private set; }

        public Brush AccentHigh { get; private set; }
        public Brush AccentHigh75 { get; private set; }
        public Brush AccentHigh50 { get; private set; }
        public Brush AccentHigh25 { get; private set; }

        public Brush AccentLow { get; private set; }
        public Brush AccentLow75 { get; private set; }
        public Brush AccentLow50 { get; private set; }
        public Brush AccentLow25 { get; private set; }

        public Brush AccentDeep { get; private set; }
        public Brush AccentDeep75 { get; private set; }
        public Brush AccentDeep50 { get; private set; }
        public Brush AccentDeep25 { get; private set; }

        public Brush AccentForeground { get; private set; }
        public Brush AccentForeground75 { get; private set; }
        public Brush AccentForeground50 { get; private set; }
        public Brush AccentForeground25 { get; private set; }

        public Brush AccentBorder { get; private set; }
        public Brush AccentBorder75 { get; private set; }
        public Brush AccentBorder50 { get; private set; }
        public Brush AccentBorder25 { get; private set; }

        public Brush AccentBorderHigh { get; private set; }
        public Brush AccentBorderHigh75 { get; private set; }
        public Brush AccentBorderHigh50 { get; private set; }
        public Brush AccentBorderHigh25 { get; private set; }

        public Brush AccentBorderLow { get; private set; }
        public Brush AccentBorderLow75 { get; private set; }
        public Brush AccentBorderLow50 { get; private set; }
        public Brush AccentBorderLow25 { get; private set; }

        public Brush AccentBorderDeep { get; private set; }
        public Brush AccentBorderDeep75 { get; private set; }
        public Brush AccentBorderDeep50 { get; private set; }
        public Brush AccentBorderDeep25 { get; private set; }

        public Brush Main { get; private set; }
        public Brush Main75 { get; private set; }
        public Brush Main50 { get; private set; }
        public Brush Main25 { get; private set; }

        public Brush MainHigh { get; private set; }
        public Brush MainHigh75 { get; private set; }
        public Brush MainHigh50 { get; private set; }
        public Brush MainHigh25 { get; private set; }

        public Brush MainLow { get; private set; }
        public Brush MainLow75 { get; private set; }
        public Brush MainLow50 { get; private set; }
        public Brush MainLow25 { get; private set; }

        public Brush MainDeep { get; private set; }
        public Brush MainDeep75 { get; private set; }
        public Brush MainDeep50 { get; private set; }
        public Brush MainDeep25 { get; private set; }

        public Brush MainForeground { get; private set; }
        public Brush MainForeground75 { get; private set; }
        public Brush MainForeground50 { get; private set; }
        public Brush MainForeground25 { get; private set; }

        public Brush MainBorder { get; private set; }
        public Brush MainBorder75 { get; private set; }
        public Brush MainBorder50 { get; private set; }
        public Brush MainBorder25 { get; private set; }

        public Brush MainBorderHigh { get; private set; }
        public Brush MainBorderHigh75 { get; private set; }
        public Brush MainBorderHigh50 { get; private set; }
        public Brush MainBorderHigh25 { get; private set; }

        public Brush MainBorderLow { get; private set; }
        public Brush MainBorderLow75 { get; private set; }
        public Brush MainBorderLow50 { get; private set; }
        public Brush MainBorderLow25 { get; private set; }

        public Brush MainBorderDeep { get; private set; }
        public Brush MainBorderDeep75 { get; private set; }
        public Brush MainBorderDeep50 { get; private set; }
        public Brush MainBorderDeep25 { get; private set; }

        public Brush Success { get; private set; }
        public Brush Success75 { get; private set; }
        public Brush Success50 { get; private set; }
        public Brush Success25 { get; private set; }

        public Brush Failure { get; private set; }
        public Brush Failure75 { get; private set; }
        public Brush Failure50 { get; private set; }
        public Brush Failure25 { get; private set; }

        public Brush Warning { get; private set; }
        public Brush Warning75 { get; private set; }
        public Brush Warning50 { get; private set; }
        public Brush Warning25 { get; private set; }

        public Brush this[ThemeColor color]
        {
            get
            {
                switch(color)
                {
                    case ThemeColor.Accent: return Accent;
                    case ThemeColor.AccentHigh: return AccentHigh;
                    case ThemeColor.AccentLow: return AccentLow;
                    case ThemeColor.AccentDeep: return AccentDeep;
                    case ThemeColor.AccentForeground: return AccentForeground;
                    case ThemeColor.AccentBorder: return AccentBorder;
                    case ThemeColor.AccentBorderHigh: return AccentBorderHigh;
                    case ThemeColor.AccentBorderLow: return AccentBorderLow;
                    case ThemeColor.AccentBorderDeep: return AccentBorderDeep;
                    case ThemeColor.Main: return Main;
                    case ThemeColor.MainHigh: return MainHigh;
                    case ThemeColor.MainLow: return MainLow;
                    case ThemeColor.MainDeep: return MainDeep;
                    case ThemeColor.MainForeground: return MainForeground;
                    case ThemeColor.MainBorder: return MainBorder;
                    case ThemeColor.MainBorderHigh: return MainBorderHigh;
                    case ThemeColor.MainBorderLow: return MainBorderLow;
                    case ThemeColor.MainBorderDeep: return MainBorderDeep;
                    case ThemeColor.Success: return Success;
                    case ThemeColor.Failure: return Failure;
                    case ThemeColor.Warning: return Warning;
                    default:
                        throw new ArgumentException($"{color} is unknown");
                }
            }
        }
        
        private void OnColorChanged(object sender, ColorChangedEventArgs e)
        {
            switch (e.Name)
            {
                case "Accent":
                    Accent = new SolidColorBrush(e.Value); OnPropertyChanged("Accent");
                    Accent75 = new SolidColorBrush(e.O75); OnPropertyChanged("Accent75");
                    Accent50 = new SolidColorBrush(e.O50); OnPropertyChanged("Accent50");
                    Accent25 = new SolidColorBrush(e.O25); OnPropertyChanged("Accent25");
                    break;
                case "AccentHigh":
                    AccentHigh = new SolidColorBrush(e.Value); OnPropertyChanged("AccentHigh");
                    AccentHigh75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentHigh75");
                    AccentHigh50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentHigh50");
                    AccentHigh25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentHigh25");
                    break;
                case "AccentLow":
                    AccentLow = new SolidColorBrush(e.Value); OnPropertyChanged("AccentLow");
                    AccentLow75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentLow75");
                    AccentLow50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentLow50");
                    AccentLow25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentLow25");
                    break;
                case "AccentDeep":
                    AccentDeep = new SolidColorBrush(e.Value); OnPropertyChanged("AccentDeep");
                    AccentDeep75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentDeep75");
                    AccentDeep50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentDeep50");
                    AccentDeep25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentDeep25");
                    break;
                case "AccentForeground":
                    AccentForeground = new SolidColorBrush(e.Value); OnPropertyChanged("AccentForeground");
                    AccentForeground75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentForeground75");
                    AccentForeground50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentForeground50");
                    AccentForeground25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentForeground25");
                    break;
                case "AccentBorder":
                    AccentBorder = new SolidColorBrush(e.Value); OnPropertyChanged("AccentBorder");
                    AccentBorder75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentBorder75");
                    AccentBorder50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentBorder50");
                    AccentBorder25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentBorder25");
                    break;
                case "AccentBorderHigh":
                    AccentBorderHigh = new SolidColorBrush(e.Value); OnPropertyChanged("AccentBorderHigh");
                    AccentBorderHigh75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentBorderHigh75");
                    AccentBorderHigh50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentBorderHigh50");
                    AccentBorderHigh25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentBorderHigh25");
                    break;
                case "AccentBorderLow":
                    AccentBorderLow = new SolidColorBrush(e.Value); OnPropertyChanged("AccentBorderLow");
                    AccentBorderLow75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentBorderLow75");
                    AccentBorderLow50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentBorderLow50");
                    AccentBorderLow25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentBorderLow25");
                    break;
                case "AccentBorderDeep":
                    AccentBorderDeep = new SolidColorBrush(e.Value); OnPropertyChanged("AccentBorderDeep");
                    AccentBorderDeep75 = new SolidColorBrush(e.O75); OnPropertyChanged("AccentBorderDeep75");
                    AccentBorderDeep50 = new SolidColorBrush(e.O50); OnPropertyChanged("AccentBorderDeep50");
                    AccentBorderDeep25 = new SolidColorBrush(e.O25); OnPropertyChanged("AccentBorderDeep25");
                    break;
                case "Main":
                    Main = new SolidColorBrush(e.Value); OnPropertyChanged("Main");
                    Main75 = new SolidColorBrush(e.O75); OnPropertyChanged("Main75");
                    Main50 = new SolidColorBrush(e.O50); OnPropertyChanged("Main50");
                    Main25 = new SolidColorBrush(e.O25); OnPropertyChanged("Main25");
                    break;
                case "MainHigh":
                    MainHigh = new SolidColorBrush(e.Value); OnPropertyChanged("MainHigh");
                    MainHigh75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainHigh75");
                    MainHigh50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainHigh50");
                    MainHigh25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainHigh25");
                    break;
                case "MainLow":
                    MainLow = new SolidColorBrush(e.Value); OnPropertyChanged("MainLow");
                    MainLow75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainLow75");
                    MainLow50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainLow50");
                    MainLow25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainLow25");
                    break;
                case "MainDeep":
                    MainDeep = new SolidColorBrush(e.Value); OnPropertyChanged("MainDeep");
                    MainDeep75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainDeep75");
                    MainDeep50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainDeep50");
                    MainDeep25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainDeep25");
                    break;
                case "MainForeground":
                    MainForeground = new SolidColorBrush(e.Value); OnPropertyChanged("MainForeground");
                    MainForeground75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainForeground75");
                    MainForeground50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainForeground50");
                    MainForeground25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainForeground25");
                    break;
                case "MainBorder":
                    MainBorder = new SolidColorBrush(e.Value); OnPropertyChanged("MainBorder");
                    MainBorder75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainBorder75");
                    MainBorder50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainBorder50");
                    MainBorder25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainBorder25");
                    break;
                case "MainBorderHigh":
                    MainBorderHigh = new SolidColorBrush(e.Value); OnPropertyChanged("MainBorderHigh");
                    MainBorderHigh75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainBorderHigh75");
                    MainBorderHigh50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainBorderHigh50");
                    MainBorderHigh25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainBorderHigh25");
                    break;
                case "MainBorderLow":
                    MainBorderLow = new SolidColorBrush(e.Value); OnPropertyChanged("MainBorderLow");
                    MainBorderLow75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainBorderLow75");
                    MainBorderLow50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainBorderLow50");
                    MainBorderLow25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainBorderLow25");
                    break;
                case "MainBorderDeep":
                    MainBorderDeep = new SolidColorBrush(e.Value); OnPropertyChanged("MainBorderDeep");
                    MainBorderDeep75 = new SolidColorBrush(e.O75); OnPropertyChanged("MainBorderDeep75");
                    MainBorderDeep50 = new SolidColorBrush(e.O50); OnPropertyChanged("MainBorderDeep50");
                    MainBorderDeep25 = new SolidColorBrush(e.O25); OnPropertyChanged("MainBorderDeep25");
                    break;
                case "Success":
                    Success = new SolidColorBrush(e.Value); OnPropertyChanged("Success");
                    Success75 = new SolidColorBrush(e.O75); OnPropertyChanged("Success75");
                    Success50 = new SolidColorBrush(e.O50); OnPropertyChanged("Success50");
                    Success25 = new SolidColorBrush(e.O25); OnPropertyChanged("Success25");
                    break;
                case "Failure":
                    Failure = new SolidColorBrush(e.Value); OnPropertyChanged("Failure");
                    Failure75 = new SolidColorBrush(e.O75); OnPropertyChanged("Failure75");
                    Failure50 = new SolidColorBrush(e.O50); OnPropertyChanged("Failure50");
                    Failure25 = new SolidColorBrush(e.O25); OnPropertyChanged("Failure25");
                    break;
                case "Warning":
                    Warning = new SolidColorBrush(e.Value); OnPropertyChanged("Warning");
                    Warning75 = new SolidColorBrush(e.O75); OnPropertyChanged("Warning75");
                    Warning50 = new SolidColorBrush(e.O50); OnPropertyChanged("Warning50");
                    Warning25 = new SolidColorBrush(e.O25); OnPropertyChanged("Warning25");
                    break;
                default:
                    throw new InvalidCastException($"{e.Name} property is not defined in brushes");
            }
        }
    }

    public sealed class ThemeCurrent : Theme
    {
        internal ThemeCurrent()
        {
            Apply(new ThemeDefault());
        }
        
        public void Apply(Theme theme)
        {
            if (ReferenceEquals(theme, this)) return;

            Colors.Accent = theme.Colors.Accent;
            Colors.AccentHigh = theme.Colors.AccentHigh;
            Colors.AccentLow = theme.Colors.AccentLow;
            Colors.AccentDeep = theme.Colors.AccentDeep;
            Colors.AccentForeground = theme.Colors.AccentForeground;
            Colors.AccentBorder = theme.Colors.AccentBorder;
            Colors.AccentBorderHigh = theme.Colors.AccentBorderHigh;
            Colors.AccentBorderLow = theme.Colors.AccentBorderLow;
            Colors.AccentBorderDeep = theme.Colors.AccentBorderDeep;
            Colors.Main = theme.Colors.Main;
            Colors.MainHigh = theme.Colors.MainHigh;
            Colors.MainLow = theme.Colors.MainLow;
            Colors.MainDeep = theme.Colors.MainDeep;
            Colors.MainForeground = theme.Colors.MainForeground;
            Colors.MainBorder = theme.Colors.MainBorder;
            Colors.MainBorderHigh = theme.Colors.MainBorderHigh;
            Colors.MainBorderLow = theme.Colors.MainBorderLow;
            Colors.MainBorderDeep = theme.Colors.MainBorderDeep;
            Colors.Success = theme.Colors.Success;
            Colors.Failure = theme.Colors.Failure;
            Colors.Warning = theme.Colors.Warning;

            Fonts.Normal = theme.Fonts.Normal;
            Fonts.Light = theme.Fonts.Light;
            Fonts.Strong = theme.Fonts.Strong;
            Fonts.ExtraStrong = theme.Fonts.ExtraStrong;

            FontSizes.XS = theme.FontSizes.XS;
            FontSizes.S = theme.FontSizes.S;
            FontSizes.M = theme.FontSizes.M;
            FontSizes.L = theme.FontSizes.L;
            FontSizes.XL = theme.FontSizes.XL;
            FontSizes.XXL = theme.FontSizes.XXL;
            FontSizes.XXXL = theme.FontSizes.XXXL;
            FontSizes.XXXXL = theme.FontSizes.XXXXL;
        }
	}

    public class ThemeDefault : Theme
    {
        public ThemeDefault()
        {
            Colors.Accent = (Color)ColorConverter.ConvertFromString("#FFA52A2A");
            Colors.AccentForeground = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            Colors.AccentBorder = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            Colors.Main = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
            Colors.MainForeground = (Color)ColorConverter.ConvertFromString("#FF000000");
            Colors.MainBorder = (Color)ColorConverter.ConvertFromString("#FFA9A9A9");
            Colors.Success = (Color)ColorConverter.ConvertFromString("#FF008000");
            Colors.Failure = (Color)ColorConverter.ConvertFromString("#FFFF0000");
            Colors.Warning = (Color)ColorConverter.ConvertFromString("#FFFFA500");
            Colors.AccentHigh = ColorConverters.ApplyTint(Colors.Accent, 0.2);
            Colors.AccentLow = ColorConverters.ApplyShade(Colors.Accent, 0.3);
            Colors.AccentDeep = ColorConverters.ApplyShade(Colors.Accent, 0.5);
            Colors.AccentBorderHigh = ColorConverters.ApplyTint(Colors.AccentBorder, 0.2);
            Colors.AccentBorderLow = ColorConverters.ApplyShade(Colors.AccentBorder, 0.3);
            Colors.AccentBorderDeep = ColorConverters.ApplyShade(Colors.AccentBorder, 0.5);
            Colors.MainHigh = ColorConverters.ApplyTint(Colors.Accent, 0.2);
            Colors.MainLow = ColorConverters.ApplyShade(Colors.Main, 0.3);
            Colors.MainDeep = ColorConverters.ApplyShade(Colors.Main, 0.5);
            Colors.MainBorderHigh = ColorConverters.ApplyTint(Colors.MainBorder, 0.5);
            Colors.MainBorderLow = ColorConverters.ApplyShade(Colors.MainBorder, 0.3);
            Colors.MainBorderDeep = ColorConverters.ApplyShade(Colors.MainBorder, 0.5);

            FontSizes.XS = 10;
            FontSizes.S = 11;
            FontSizes.M = 12;
            FontSizes.L = 14;
            FontSizes.XL = 16;
            FontSizes.XXL = 20;
            FontSizes.XXXL = 24;
            FontSizes.XXXXL = 36;

            Fonts.Normal = new FontFamily("Segoe UI");
            Fonts.Light = new FontFamily("Segoe UI Light");
            Fonts.Strong = new FontFamily("Segoe UI Semibold");
            Fonts.ExtraStrong = new FontFamily("Segoe UI Black");
        }
    }

    public class ColorChangedEventArgs : EventArgs
    {
        public ColorChangedEventArgs(string name, 
                                     Color O75,
                                     Color O50,
                                     Color O25,
                                     Color value)
        {
            Name = name;
            Value = value;

            this.O75 = O75;
            this.O50 = O50;
            this.O25 = O25;
        }

        public Color O75 { get; }
        public Color O50 { get; }
        public Color O25 { get; }

        public Color Value { get; }
        public string Name { get; }
    }
}
