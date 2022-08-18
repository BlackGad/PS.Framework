using System;

namespace PS.Data.Fuzzy.Computers
{
    public class DelegateCoefficientComputer : FuzzyCoefficientComputer
    {
        private readonly Func<string, string, double> _compute;

        public DelegateCoefficientComputer(Func<string, string, double> compute)
        {
            _compute = compute ?? throw new ArgumentNullException(nameof(compute));
        }

        public override double Compute(string input, string target)
        {
            return _compute(input, target);
        }
    }
}
