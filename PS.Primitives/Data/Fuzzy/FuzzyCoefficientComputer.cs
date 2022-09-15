namespace PS.Data.Fuzzy
{
    public abstract class FuzzyCoefficientComputer
    {
        public static IFuzzyCoefficientComputerConfiguration Setup(bool caseSensitive = false)
        {
            return new FuzzyCoefficientComputerConfiguration(caseSensitive);
        }

        public abstract double Compute(string input, string target);
    }
}
