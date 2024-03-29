﻿using System;
using System.Windows;
using System.Windows.Controls;
using PS.WPF.Resources;

namespace PS.WPF.Controls.Native
{
    public static class CheckBox
    {
        public static readonly Uri DictionaryLocation = new Uri("/PS.WPF;component/Controls/Native/CheckBox.xaml", UriKind.RelativeOrAbsolute);

        public static readonly ResourceDescriptor CheckBoxStyle =
            ResourceDescriptor.Create<Style>(
                description: "CheckBox style",
                resourceDictionary: DictionaryLocation
            );

        public static readonly ResourceDescriptor CheckBoxTemplate =
            ResourceDescriptor.Create<ControlTemplate>(
                description: "CheckBox template",
                resourceDictionary: DictionaryLocation
            );
    }
}
