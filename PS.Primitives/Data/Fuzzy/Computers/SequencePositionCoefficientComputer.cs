using System;

namespace PS.Data.Fuzzy.Computers
{
    public class SequencePositionCoefficientComputer : FuzzyCoefficientComputer
    {
        public override double Compute(string input, string target)
        {
            var index = input.IndexOf(target, StringComparison.Ordinal);
            if (index == -1) return 0;
            return (double)(target.Length - index) / target.Length;
        }
    }
}
