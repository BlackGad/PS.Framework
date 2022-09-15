﻿using System.Collections.Generic;
using System.Windows;
using NLog;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;

namespace PS.Shell.Module.Controls.ViewModels
{
    [DependencyRegisterAsSelf]
    public class SuggestListViewViewModel : DependencyObject,
                                            IViewModel
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                                        typeof(string),
                                        typeof(SuggestListViewViewModel),
                                        new FrameworkPropertyMetadata());

        public SuggestListViewViewModel(ILogger logger)
        {
            Logger = logger;
            ItemsSource = new[]
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones",
                "Miller", "Davis", "Garcia", "Rodriguez", "Wilson",
                "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez",
                "Moore", "Martin", "Jackson", "Thompson", "White",
                "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis",
                "Robinson", "Walker", "Perez", "Hall", "Young", "Allen",
                "Sanchez", "Wright", "King", "Scott", "Green", "Baker",
                "Adams", "Nelson", "Hill", "Ramirez", "Campbell",
                "Mitchell", "Roberts", "Carter", "Phillips", "Evans",
                "Turner", "Torres", "Parker", "Collins", "Edwards",
                "Stewart", "Flores", "Morris", "Nguyen", "Murphy",
                "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper",
                "Reed", "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward",
                "Cox", "Diaz", "Richardson", "Wood", "Watson", "Brooks",
                "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price",
                "Myers", "Long", "Foster", "Sanders", "Ross", "Morales",
                "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins",
                "Gutierrez", "Perry", "Butler", "Barnes", "Fisher", "Henderson",
                "Coleman", "Simmons", "Patterson", "Jordan", "Reynolds", "Hamilton",
                "Graham", "Kim", "Gonzales", "Alexander", "Ramos", "Wallace",
                "Griffin", "West", "Cole", "Hayes", "Chavez", "Gibson",
                "Bryant", "Ellis", "Stevens", "Murray", "Ford", "Marshall",
                "Owens", "Mcdonald", "Harrison", "Ruiz", "Kennedy", "Wells",
                "Alvarez", "Woods", "Mendoza", "Castillo", "Olson", "Webb",
                "Washington", "Tucker", "Freeman", "Burns", "Henry",
                "Vasquez", "Snyder", "Simpson", "Crawford", "Jimenez", "Porter",
                "Mason", "Shaw", "Gordon", "Wagner", "Hunter", "Romero", "Hicks",
                "Dixon", "Hunt", "Palmer", "Robertson", "Black", "Holmes", "Stone",
                "Meyer", "Boyd", "Mills", "Warren", "Fox", "Rose", "Rice", "Moreno"
            };
        }

        public IReadOnlyList<string> ItemsSource { get; }

        public ILogger Logger { get; }

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SelectedItemProperty)
            {
                Logger.Info($"Selected item changed from '{e.OldValue}' to '{e.NewValue}'");
            }
        }
    }
}
