using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NLog;
using PS.Commander.ViewModels;
using PS.ComponentModel.DeepTracker;
using PS.ComponentModel.DeepTracker.Extensions;
using PS.Extensions;
using PS.IoC.Attributes;

namespace PS.Commander.Models.ExplorerService
{
    [DependencyRegisterAsSelf]
    [DependencyLifetime(DependencyLifetime.InstanceSingle)]
    [DependencyAutoActivate]
    public class ExplorerService : IDisposable
    {
        #region Constants

        private static readonly JsonSerializerSettings JsonSerializerSettings;

        #endregion

        private readonly ILogger _logger;
        private readonly ILifetimeScope _scope;
        private readonly DeepTracker _tracker;

        #region Constructors

        static ExplorerService()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ",
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented,
                Converters =
                {
                    new StringEnumConverter()
                }
            };
        }

        public ExplorerService(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            _logger = LogManager.GetCurrentClassLogger();
            ExplorerViewModels = new ObservableCollection<ExplorerViewModel>();

            _tracker = DeepTracker.Setup(ExplorerViewModels)
                                  .Include<IExplorerSerializableProperties>()
                                  .Subscribe<ChangedEventArgs>(OnChangedEvent)
                                  .Create()
                                  .Activate();
        }

        #endregion

        #region Properties

        public ObservableCollection<ExplorerViewModel> ExplorerViewModels { get; }

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
            //_saveTrigger.Trigger();
            Save();
        }

        #endregion

        #region Members

        public void Load()
        {
            //Save Throtling trigger!!!
            //ExplorerViewModels.Clear();

            var settingsFile = GetSettingsFile();

            if (settingsFile.Exists)
            {
                var json = File.ReadAllText(settingsFile.FullName, Encoding.UTF8);
                var loadedExplorers = JsonConvert.DeserializeObject<List<JObject>>(json, JsonSerializerSettings);
                foreach (var jObject in loadedExplorers.Enumerate())
                {
                    var explorerViewModel = _scope.Resolve<ExplorerViewModel>();
                    using (var reader = jObject.CreateReader())
                    {
                        JsonSerializer.CreateDefault(JsonSerializerSettings)
                                      .Populate(reader, explorerViewModel);
                    }

                    if (string.IsNullOrEmpty(explorerViewModel.Origin))
                    {
                        explorerViewModel.ResetOrigin();
                    }

                    ExplorerViewModels.Add(explorerViewModel);
                }
            }

            //var explorerViewModel1 = _scope.Resolve<ExplorerViewModel>();
            //explorerViewModel1.Container = "Left";
            //ExplorerViewModels.Add(explorerViewModel1);

            //var explorerViewModel2 = _scope.Resolve<ExplorerViewModel>();
            //explorerViewModel2.Container = "Right";
            //ExplorerViewModels.Add(explorerViewModel2);
        }

        public void Save()
        {
            var models = ExplorerViewModels.ToList();

            var settingsFile = GetSettingsFile();
            if (settingsFile.Directory?.Exists == false)
            {
                settingsFile.Directory.Create();
            }

            var json = JsonConvert.SerializeObject(models, JsonSerializerSettings);
            File.WriteAllText(settingsFile.FullName, json);
        }

        private FileInfo GetSettingsFile()
        {
            var sourceFolder = Settings.Get<string>(Settings.SettingsFolder);
            sourceFolder = Environment.ExpandEnvironmentVariables(sourceFolder);
            return new FileInfo(Path.Combine(sourceFolder, "explorers.json"));
        }

        #endregion
    }
}