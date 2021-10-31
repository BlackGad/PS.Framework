using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PS.Extensions;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    class Example : IExample
    {
        #region Constructors

        public Example(string group, string title, object viewModel, IReadOnlyList<string> log)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Order = 0;

            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            Log = log ?? throw new ArgumentNullException(nameof(log));

            Children = new ObservableCollection<ISource>();
        }

        #endregion

        #region IExample Members

        public string Title { get; }
        public string Group { get; }
        public IReadOnlyList<string> Log { get; }
        public object ViewModel { get; }

        public IExample Source<T>(string folder)
        {
            var parentFolder = GetRootFolder(folder);

            var type = typeof(T);
            var assembly = type.Assembly;
            var resourceName = type.FullName;
            var code = assembly.LoadString(resourceName + ".cs");

            parentFolder.Children.Add(new SourceCSharp(type.Name + ".cs", code));

            return this;
        }

        public IExample XamlPage<T>(string folder)
        {
            var parentFolder = GetRootFolder(folder);

            var type = typeof(T);
            var assembly = type.Assembly;
            var resourceName = type.FullName;

            var xamlCode = assembly.LoadString(resourceName + ".xaml");
            var sourceXaml = new SourceXaml(type.Name + ".xaml", xamlCode);

            parentFolder.Children.Add(sourceXaml);

            var xamlCodeBehind = assembly.LoadString(resourceName + ".xaml.cs");
            if (!string.IsNullOrEmpty(xamlCodeBehind))
            {
                sourceXaml.Children.Add(new SourceCSharp(type.Name + ".xaml.cs", xamlCodeBehind));
            }

            return this;
        }

        public IList<ISource> Children { get; }

        public int Order { get; }

        #endregion

        #region Members

        private ISource GetRootFolder(string folder)
        {
            var parts = (folder ?? string.Empty).Split(new[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);

            ISource current = this;

            foreach (var part in parts.Take(parts.Length))
            {
                var existing = current.Children.FirstOrDefault(s => s.Title.AreEqual(part));
                if (existing == null)
                {
                    existing = new SourceFolder(part);
                    current.Children.Add(existing);
                }

                current = existing;
            }

            return current;
        }

        #endregion
    }
}