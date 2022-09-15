using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PS.WPF.DataTemplate.Base
{
    public class CustomFrameworkElementFactory : FrameworkElementFactory
    {
        private readonly ICustomDataTemplate _customDataTemplate;

        public CustomFrameworkElementFactory(ICustomDataTemplate customDataTemplate)
            : base(typeof(Control))
        {
            _customDataTemplate = customDataTemplate ?? throw new ArgumentNullException(nameof(customDataTemplate));

            OverrideDefaultFactory();
        }

        internal void OverrideDefaultFactory()
        {
            var field = typeof(FrameworkElementFactory).GetField("_knownTypeFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) throw new NotSupportedException();
            field.SetValue(this, new Func<object>(CreateViewInstance));
        }

        private object CreateViewInstance()
        {
            if (Runtime.IsDesignMode)
            {
                var designTimeBlankContainer = new Grid
                {
                    Background = new SolidColorBrush(Color.FromArgb(50, 120, 120, 120))
                };
                if (_customDataTemplate.DesignWidth.HasValue) designTimeBlankContainer.Width = _customDataTemplate.DesignWidth.Value;
                if (_customDataTemplate.DesignHeight.HasValue) designTimeBlankContainer.Height = _customDataTemplate.DesignHeight.Value;

                var formatText = _customDataTemplate.Description;
                designTimeBlankContainer.Children.Add(new TextBlock
                {
                    Text = formatText,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                return designTimeBlankContainer;
            }

            var container = new ContentPresenter
            {
                Content = _customDataTemplate.CreateView()
            };

            return container;
        }
    }
}
