using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using PS.Commander.Models.ExplorerService;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.Patterns.Aware;
using PS.WPF;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ExplorerViewModel : BaseNotifyPropertyChanged,
                                     IViewModel,
                                     IIDAware,
                                     ITitleAware
    {
        private readonly ILifetimeScope _scope;
        private IReadOnlyList<FileSystemItemViewModel> _items;
        private string _origin;

        #region Constructors

        public ExplorerViewModel(ILifetimeScope scope, Explorer explorer)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            
            Id = explorer.Id;
            Origin = explorer.Origin;

            CloseCommand = new RelayUICommand(Close);
        }

        #endregion

        #region Properties

        public IUICommand CloseCommand { get; }

        public IReadOnlyList<FileSystemItemViewModel> Items
        {
            get { return _items; }
            private set { SetField(ref _items, value); }
        }

        public string Origin
        {
            get { return _origin; }
            set
            {
                if (SetField(ref _origin, value))
                {
                    OnPropertyChanged(nameof(Title));
                    RefreshFiles();
                }
            }
        }

        #endregion

        #region IIDAware Members

        public string Id { get; }

        #endregion

        #region ITitleAware Members

        public string Title
        {
            get { return new DirectoryInfo(_origin ?? string.Empty).Name; }
        }

        #endregion

        #region Members

        private void Close()
        {
            _scope.Resolve<ExplorerService>().Delete(this);
        }

        private void RefreshFiles()
        {
            Items = _scope.Resolve<ExplorerService>().GetFileSystemItems(Origin);
        }

        #endregion
    }
}