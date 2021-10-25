using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;
using PS.Patterns.Command;

namespace PS.WPF.Markup
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class RelayCommand : MarkupExtension
    {
        #region Properties

        public string ElementName { get; set; }

        public string MethodName { get; set; }
        public Type ParameterType { get; set; }

        #endregion

        #region Override members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            if (rootObjectProvider?.RootObject is FrameworkElement frameworkElement)
            {
                return new RelayCommand<object>(param =>
                {
                    if (!frameworkElement.IsLoaded) return;

                    var targetElement = frameworkElement.FindName(ElementName);
                    if (targetElement == null) return;

                    var methods = targetElement.GetType()
                                               .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                               .Where(m => Equals(m.Name, MethodName))
                                               .Where(m => IsMethodAbleToProcessCommand(m, param))
                                               .ToList();
                    if (methods.Count > 1) return;
                    var selectedMethod = methods.First();
                    if (selectedMethod.GetParameters().Any())
                    {
                        selectedMethod.Invoke(targetElement, new[] { param });
                    }
                    else
                    {
                        selectedMethod.Invoke(targetElement, new object[] { });
                    }
                });
            }

            return null;
        }

        #endregion

        #region Members

        private bool IsMethodAbleToProcessCommand(MethodInfo m, object argument)
        {
            var parameters = m.GetParameters();

            if (parameters.Length == 0) return true;
            if (parameters.Length > 1) return false;

            var argumentType = argument?.GetType() ?? ParameterType;
            var parameterType = parameters.First().ParameterType;

            if (argumentType == null && parameterType.IsClass) return true;

            return parameterType == argumentType;
        }

        #endregion
    }
}