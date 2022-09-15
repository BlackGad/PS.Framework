using System.Linq;

namespace PS.Data.Fuzzy.Computers
{
    public class TanimotoCoefficientComputer : FuzzyCoefficientComputer
    {
        public override double Compute(string input, string target)
        {
            double na = input.Length;
            double nb = target.Length;
            double nc = input.Intersect(target).Count();

            return nc / (na + nb - nc);
        }
    }
}
