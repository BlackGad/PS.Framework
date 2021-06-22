using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using PS.Commander.ViewModels;
using PS.Extensions;
using PS.IoC.Attributes;
using PS.MVVM.Services;

namespace PS.Commander.Models.ExplorerService
{
    [DependencyRegisterAsSelf]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    [DependencyAutoActivate]
    public class ExplorerService
    {
        #region Static members

        private static string GetDefaultPath()
        {
            return DriveInfo.GetDrives()
                            .First(d => d.DriveType == DriveType.Fixed && d.IsReady)
                            .RootDirectory.FullName;
        }

        #endregion

        private readonly IReadOnlyDictionary<Area, IObservableModelCollection> _items;
        private readonly ILifetimeScope _scope;

        #region Constructors

        public ExplorerService(ILifetimeScope scope, IModelResolverService modelResolverService)
        {
            if (modelResolverService == null) throw new ArgumentNullException(nameof(modelResolverService));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            _items = new Dictionary<Area, IObservableModelCollection>
            {
                { Area.Left, modelResolverService.Collection(Area.Left) },
                { Area.Right, modelResolverService.Collection(Area.Right) }
            };
        }

        #endregion

        #region Members

        public void Delete(ExplorerViewModel explorerViewModel)
        {
            var collection = _items.Values.FirstOrDefault(c => c.Contains(explorerViewModel));
            if (collection != null)
            {
                collection.Remove(explorerViewModel);
                if (collection.Count == 0)
                {
                    var explorer = new Explorer
                    {
                        Origin = GetDefaultPath()
                    };

                    collection.Add(_scope.Resolve<ExplorerViewModel>(TypedParameter.From(explorer)));
                }
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
            ExplorerViewModel Create(string origin)
            {
                var explorer = new Explorer
                {
                    Origin = origin
                };
                return _scope.Resolve<ExplorerViewModel>(TypedParameter.From(explorer));
            }

            AddItem(Area.Left, Create(@"c:\Projects\GitHub\BlackGad\PS.Framework\"));
            AddItem(Area.Left, Create(@"c:\"));
            AddItem(Area.Left, Create(@"c:\Temp\"));
            AddItem(Area.Right, Create(@"c:\Temp\Demo\"));
            AddItem(Area.Right, Create(@"c:\"));
        }

        public void PlaceExplorer(ExplorerViewModel source, Place place, ExplorerViewModel target)
        {
            if (source.AreEqual(target)) return;

            var sourceCollection = _items.Values.FirstOrDefault(i => i.Contains(source));
            var targetCollection = _items.Values.FirstOrDefault(i => i.Contains(target));

            if (sourceCollection == null || targetCollection == null) return;

            var targetItemIndex = targetCollection.IndexOf(target);
            if (place == Place.After)
            {
                targetItemIndex++;
            }

            if (sourceCollection.AreEqual(targetCollection))
            {
                var sourceItemIndex = sourceCollection.IndexOf(source);
                if (sourceItemIndex == targetItemIndex) return;
            }

            sourceCollection.Remove(source);

            if (targetItemIndex < targetCollection.Count)
            {
                targetCollection.Insert(targetItemIndex, source);
            }
            else
            {
                targetCollection.Add(source);
            }
        }

        private void AddItem(Area type, ExplorerViewModel model)
        {
            GetExplorers(type).Add(model);
        }

        #endregion
    }
}