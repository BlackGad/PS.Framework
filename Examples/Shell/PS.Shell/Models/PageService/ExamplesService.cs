using System;
using System.Collections.ObjectModel;
using Autofac;
using NLog;
using NLog.Layouts;
using PS.IoC.Attributes;
using PS.Shell.Infrastructure.Models.ExamplesService;

namespace PS.Shell.Models.PageService
{
    [DependencyRegisterAsInterface(typeof(IExamplesService))]
    internal class ExamplesService : ObservableCollection<IExample>,
                                     IExamplesService
    {
        private readonly ILifetimeScope _scope;

        public ExamplesService(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public IExample Add<T>(string group, string title)
        {
            var loggerName = $"{group}-{title}";
            var targetName = loggerName + " target";

            var logs = new ObservableCollection<string>();

            var configuration = LogManager.Configuration;
            var target = new CollectionTarget(logs)
            {
                Name = targetName,
                Layout = new SimpleLayout("${time} ${uppercase:${level}} - ${message} ${exception:format=tostring}")
            };
            configuration.AddTarget(targetName, target);
            configuration.AddRule(LogLevel.Trace, LogLevel.Fatal, targetName, loggerName);
            LogManager.Configuration = configuration;

            var logger = LogManager.GetLogger(loggerName);
            var viewModel = _scope.Resolve<T>(TypedParameter.From<ILogger>(logger));
            var example = new Example(group, title, viewModel, logs);

            Add(example);

            return example;
        }
    }
}
