using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Newtonsoft.Json;
using PS.Commander.Models.ExplorerService;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.WPF;
using PS.WPF.Patterns.Command;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    [JsonObject(MemberSerialization.OptIn)]
    public class ExplorerViewModel : BaseNotifyPropertyChanged,
                                     IViewModel,
                                     ITitleAware,
                                     IExplorerSerializableProperties
    {
        #region Static members

        public static string GetDefaultPath()
        {
            return DriveInfo.GetDrives()
                            .First(d => d.DriveType == DriveType.Fixed && d.IsReady)
                            .RootDirectory.FullName;
        }

        #endregion

        private readonly ILifetimeScope _scope;
        private string _container;
        private IReadOnlyList<FileSystemItemViewModel> _items;
        private string _origin;
        private string _title;

        #region Constructors

        public ExplorerViewModel(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Origin = GetDefaultPath();
            CloseCommand = new RelayUICommand(Close);
            TestCommand = new RelayUICommand(Test);
        }

        #endregion

        #region Properties

        public IUICommand CloseCommand { get; }

        public IReadOnlyList<FileSystemItemViewModel> Items
        {
            get { return _items; }
            private set { SetField(ref _items, value); }
        }

        public IUICommand TestCommand { get; }

        #endregion

        #region IExplorerSerializableProperties Members

        [JsonProperty]
        public string Container
        {
            get { return _container; }
            set { SetField(ref _container, value); }
        }

        [JsonProperty]
        public string Origin
        {
            get { return _origin; }
            set
            {
                if (SetField(ref _origin, value))
                {
                    Refresh();
                }
            }
        }

        #endregion

        #region ITitleAware Members

        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        #endregion

        #region Members

        public void ResetOrigin()
        {
            Origin = GetDefaultPath();
        }

        private void Close()
        {
            _scope.Resolve<ExplorerService>().ExplorerViewModels.Remove(this);
        }

        private void Refresh()
        {
            var directoryInfo = new DirectoryInfo(Origin ?? string.Empty);
            Items = directoryInfo.GetFileSystemInfos()
                                 .Select(i => _scope.Resolve<FileSystemItemViewModel>(TypedParameter.From(i)))
                                 .ToList();
            Title = directoryInfo.Name;
        }

        private void Test()
        {
            Container = Container.AreEqual("Left") ? "Right" : "Left";
        }

        #endregion
    }
}