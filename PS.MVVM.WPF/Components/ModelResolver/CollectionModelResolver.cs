using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using PS.MVVM.Services;

namespace PS.MVVM.Components.ModelResolver
{
    public class CollectionModelResolver : ModelResolver
    {
        protected override Binding ProvideBinding(bool isReadOnly)
        {
            var path = new List<string>
            {
                nameof(Model)
            };

            if (!string.IsNullOrEmpty(Path))
            {
                path.Add(Path);
            }

            return new Binding
            {
                Source = this,
                Path = new PropertyPath(string.Join(".", path)),
                Mode = BindingMode.OneWay,
                Converter = Converter,
                ConverterParameter = ConverterParameter
            };
        }

        protected override object ProvideObservableModel(IModelResolverService modelResolverService)
        {
            return modelResolverService.Collection(Region);
        }
    }
}
