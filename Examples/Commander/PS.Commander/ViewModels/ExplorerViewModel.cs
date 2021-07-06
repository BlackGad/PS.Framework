using System;
using System.Collections.Generic;
using System.IO;
using PS.Commander.Models.BroadcastService;
using PS.Commander.Models.ExplorerService;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Patterns;
using PS.MVVM.Services;
using PS.MVVM.Services.Extensions;
using PS.Patterns.Aware;
using PS.WPF;

namespace PS.Commander.ViewModels
{
    [DependencyRegisterAsSelf]
    public class ExplorerViewModel : BaseNotifyPropertyChanged,
                                     IViewModel,
                                     ITitleAware,
                                     IDisposable
    {
        private readonly IBroadcastService _broadcastService;
        private readonly ExplorerService _explorerService;
        private IReadOnlyList<FileSystemItemViewModel> _items;

        private string _title;

        #region Constructors

        public ExplorerViewModel(Explorer explorer, ExplorerService explorerService, IBroadcastService broadcastService)
        {
            Explorer = explorer ?? throw new ArgumentNullException(nameof(explorer));
            _explorerService = explorerService ?? throw new ArgumentNullException(nameof(explorerService));
            _broadcastService = broadcastService ?? throw new ArgumentNullException(nameof(broadcastService));

            CloseCommand = new RelayUICommand(Close);

            _broadcastService.Subscribe<OriginChangedArgs>(OnOriginChanged);

            Refresh();
        }

        #endregion

        #region Properties

        public IUICommand CloseCommand { get; }
        public Explorer Explorer { get; }

        public IReadOnlyList<FileSystemItemViewModel> Items
        {
            get { return _items; }
            private set { SetField(ref _items, value); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _broadcastService.Unsubscribe<OriginChangedArgs>(OnOriginChanged);
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

        private void Close()
        {
            _explorerService.Delete(Explorer);
        }

        private void OnOriginChanged(OriginChangedArgs args)
        {
            if (args.Explorer.AreDiffers(Explorer)) return;

            Refresh();
        }

        private void Refresh()
        {
            Items = _explorerService.GetFileSystemItems(Explorer.Origin);
            Title = new DirectoryInfo(Explorer.Origin ?? string.Empty).Name;
        }

        #endregion
    }
}