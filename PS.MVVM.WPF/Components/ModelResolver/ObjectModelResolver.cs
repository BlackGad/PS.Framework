using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using PS.MVVM.Services;

namespace PS.MVVM.Components.ModelResolver
{
    public class ObjectModelResolver : ModelResolver
    {
        protected override Binding ProvideBinding(bool isReadOnly)
        {
            var path = new List<string>
            {
                nameof(Model),
                nameof(IObservableModelObject.Value)
            };

            if (!string.IsNullOrEmpty(Path))
            {
                path.Add(Path);
            }

            return new Binding
            {
                Source = this,
                Path = new PropertyPath(string.Join(".", path)),
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                Converter = Converter,
                ConverterParameter = ConverterParameter
            };
        }

        protected override object ProvideObservableModel(IModelResolverService modelResolverService)
        {
            return modelResolverService.Object(Region);
        }
    }
}
