using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PS.Data.Fuzzy.Computers;

namespace PS.Data.Fuzzy
{
    internal class FuzzyCoefficientComputerConfiguration : IFuzzyCoefficientComputerConfiguration
    {
        private readonly bool _caseSensitive;
        private readonly List<Tuple<FuzzyCoefficientComputer, double>> _computers;

        public FuzzyCoefficientComputerConfiguration(bool caseSensitive)
        {
            _caseSensitive = caseSensitive;
            _computers = new List<Tuple<FuzzyCoefficientComputer, double>>();
        }

        public IFuzzyCoefficientComputerConfiguration Use<TComputer>(double weight)
            where TComputer : FuzzyCoefficientComputer
        {
            _computers.Add(new Tuple<FuzzyCoefficientComputer, double>(Activator.CreateInstance<TComputer>(), Math.Max(weight, 0)));
            return this;
        }

        public FuzzyCoefficientComputer Create()
        {
            return new DelegateCoefficientComputer((input, target) =>
            {
                var computeInput = input ?? string.Empty;
                var computeTarget = target ?? string.Empty;
                if (!_caseSensitive)
                {
                    computeInput = computeInput.ToUpper();
                    computeTarget = computeTarget.ToUpper();
                }

                var result = new double[_computers.Count];
                Parallel.ForEach(_computers, (tuple, state, index) => { result[index] = 1 - (1 - tuple.Item1.Compute(computeInput, computeTarget)) * tuple.Item2; });
                return result.Average();
            });
        }
    }
}
