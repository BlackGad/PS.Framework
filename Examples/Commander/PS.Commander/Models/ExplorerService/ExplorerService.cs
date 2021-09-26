using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Newtonsoft.Json;
using NLog;
using PS.Commander.Models.BroadcastService;
using PS.Commander.ViewModels;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.ComponentModel.Navigation;
using PS.ComponentModel.Navigation.Extensions;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Services;
using PS.MVVM.Services.Extensions;

namespace PS.Commander.Models.ExplorerService
{
    [DependencyRegisterAsSelf]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    [DependencyAutoActivate]
    public class ExplorerService : IDisposable
    {
        #region Static members

        private static string GetDefaultPath()
        {
            return DriveInfo.GetDrives()
                            .First(d => d.DriveType == DriveType.Fixed && d.IsReady)
                            .RootDirectory.FullName;
        }

        #endregion

        private readonly IBroadcastService _broadcastService;

        private readonly ObservableCollection<Explorer> _explorers;
        private readonly IReadOnlyDictionary<Area, IObservableModelCollection> _items;
        private readonly ILogger _logger;
        private readonly ILifetimeScope _scope;
        private readonly DeepTracker _tracker;

        #region Constructors

        public ExplorerService(ILifetimeScope scope,
                               IModelResolverService modelResolverService,
                               IBroadcastService broadcastService)
        {
            if (modelResolverService == null) throw new ArgumentNullException(nameof(modelResolverService));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            _broadcastService = broadcastService ?? throw new ArgumentNullException(nameof(broadcastService));

            _items = new Dictionary<Area, IObservableModelCollection>
            {
                { Area.Left, modelResolverService.Collection(Area.Left) },
                { Area.Right, modelResolverService.Collection(Area.Right) }
            };
            _logger = LogManager.GetCurrentClassLogger();

            _explorers = new ObservableCollection<Explorer>();
            _tracker = DeepTracker.Setup(_explorers, Routes.WildcardRecursive)
                                  .Subscribe<ObjectAttachmentEventArgs>(OnObjectAttachmentEvent)
                                  .Subscribe<ChangedEventArgs>(OnChangedEvent)
                                  .Create()
                                  .Activate();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _tracker.Dispose();
        }

        #endregion

        #region Event handlers

        private void OnChangedEvent(object sender, ChangedEventArgs e)
        {
            _logger.Debug(e.FormatMessage());

            if (e is ChangedPropertyEventArgs args && e.Route.Match(Route.Create(Routes.Wildcard, nameof(Explorer.Origin))))
            {
                var tracker = (DeepTracker)sender;
                var explorer = (Explorer)tracker.GetObject(e.Route.Select(Routes.Wildcard));
                var eventArgs = new OriginChangedArgs(explorer,
                                                      args.OldValue.GetEffectiveString(),
                                                      args.NewValue.GetEffectiveString());

                _broadcastService.Broadcast(eventArgs);
            }
        }

        private void OnObjectAttachmentEvent(object sender, ObjectAttachmentEventArgs e)
        {
            if (e.Object is Explorer explorer)
            {
                if (e is ObjectAttachedEventArgs)
                {
                    var explorers = GetExplorers(explorer.Area);
                    var viewModel = _scope.Resolve<ExplorerViewModel>(TypedParameter.From(explorer));
                    explorers.Add(viewModel);
                }

                if (e is ObjectDetachedEventArgs)
                {
                    var info = Resolve(explorer);
                    if (info == null) return;

                    info.Collection.Remove(info.ViewModel);
                    info.ViewModel.Dispose();
                }
            }
        }

        #endregion

        #region Members

        public void Add(Explorer explorer)
        {
            if (_explorers.Contains(explorer)) return;
            _explorers.Add(explorer);
        }

        public void Delete(Explorer explorer)
        {
            if (_explorers.Remove(explorer))
            {
                EnsureAreasNotEmpty();
            }
        }

        public IObservableModelCollection GetExplorers(Area type)
        {
            if (_items.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new InvalidOperationException($"Unknown '{type}' area type");
        }

        public IReadOnlyList<FileSystemItemViewModel> GetFileSystemItems(string origin)
        {
            var directoryInfo = new DirectoryInfo(origin);
            return directoryInfo.GetFileSystemInfos()
                                .Select(i => _scope.Resolve<FileSystemItemViewModel>(TypedParameter.From(i)))
                                .ToList();
        }

        public void Load()
        {
            _explorers.Clear();

            var sourceFolder = Settings.Get<string>(Settings.SettingsFolder);
            sourceFolder = Environment.ExpandEnvironmentVariables(sourceFolder);
            var settingsFile = Path.Combine(sourceFolder, "explorers.json");

            if (File.Exists(settingsFile))
            {
                var loadedExplorers = JsonConvert.DeserializeObject<List<Explorer>>(File.ReadAllText(settingsFile, Encoding.UTF8));
                foreach (var explorer in loadedExplorers.Enumerate())
                {
                    _explorers.Add(explorer);
                }
            }

            EnsureAreasNotEmpty();
        }

        public void Place(Explorer source, Place place, Explorer target)
        {
            if (source.AreEqual(target)) return;

            var sourceInfo = Resolve(source);
            var targetInfo = Resolve(target);

            if (sourceInfo == null || targetInfo == null) return;

            var targetItemIndex = targetInfo.Collection.IndexOf(targetInfo.ViewModel);
            if (place == Models.ExplorerService.Place.After)
            {
                targetItemIndex++;
            }

            if (sourceInfo.Collection.AreEqual(targetInfo.Collection))
            {
                var sourceItemIndex = sourceInfo.Collection.IndexOf(sourceInfo.ViewModel);
                if (sourceItemIndex == targetItemIndex) return;
            }

            sourceInfo.Collection.Remove(sourceInfo.ViewModel);
            targetInfo.Collection.SafeInsert(targetItemIndex, sourceInfo.ViewModel);
            source.Area = targetInfo.Area;

            EnsureAreasNotEmpty();
        }

        private void EnsureAreasNotEmpty()
        {
            foreach (var pair in _items)
            {
                if (pair.Value.Count > 0) continue;

                _explorers.Add(new Explorer
                {
                    Area = pair.Key,
                    Origin = GetDefaultPath()
                });
            }
        }

        private ExplorerViewModelInfo Resolve(Explorer explorer)
        {
            if (explorer == null) return null;

            foreach (var subset in _items)
            {
                var existingViewModel = subset.Value.Enumerate<ExplorerViewModel>().FirstOrDefault(m => m.Explorer.AreEqual(explorer));
                if (existingViewModel != null)
                {
                    return new ExplorerViewModelInfo(subset.Key, subset.Value, existingViewModel);
                }
            }

            return null;
        }

        #endregion

        #region Nested type: ExplorerViewModelInfo

        internal class ExplorerViewModelInfo
        {
            #region Constructors

            public ExplorerViewModelInfo(Area area, IObservableModelCollection source, ExplorerViewModel viewModel)
            {
                ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                Collection = source ?? throw new ArgumentNullException(nameof(source));
                Area = area;
            }

            #endregion

            #region Properties

            public Area Area { get; }
            public IObservableModelCollection Collection { get; }
            public ExplorerViewModel ViewModel { get; }

            #endregion
        }

        #endregion
    }
}