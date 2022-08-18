using PS.Patterns;

namespace PS.Data.Fuzzy
{
    public interface IFuzzyCoefficientComputerConfiguration : IFluentBuilder<FuzzyCoefficientComputer>
    {
        IFuzzyCoefficientComputerConfiguration Use<TComputer>(double weight = 1.0)
            where TComputer : FuzzyCoefficientComputer;
    }
}
