using System.Windows;
using System.Windows.Data;
using PS.MVVM.Services;

namespace PS.MVVM.Components.ModelResolver;

public class ObjectModelResolver : ModelResolver
{
    protected override Binding ProvideBinding(bool isReadOnly)
    {
        return new Binding
        {
            Source = this,
            Path = new PropertyPath($"{nameof(Model)}.{nameof(IObservableModelObject.Value)}"),
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
