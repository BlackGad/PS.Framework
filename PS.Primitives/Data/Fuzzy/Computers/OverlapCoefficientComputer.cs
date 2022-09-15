namespace PS.Data.Fuzzy.Computers
{
    public class OverlapCoefficientComputer : FuzzyCoefficientComputer
    {
        public override double Compute(string input, string target)
        {
            if (string.IsNullOrEmpty(target)) return double.NaN;
            if (string.IsNullOrEmpty(input)) return double.NaN;
            if (target.Length < input.Length) return double.NaN;

            var scorePerOverlap = 1d / input.Length;

            var result = 0d;
            var currentDistancePenalty = 1d;
            using (var inputEnumerator = input.GetEnumerator())
            {
                inputEnumerator.MoveNext();

                for (var i = 0; i < target.Length; i++)
                {
                    if (inputEnumerator.Current == target[i])
                    {
                        result += scorePerOverlap * currentDistancePenalty;
                        currentDistancePenalty = 1d;
                        if (!inputEnumerator.MoveNext()) break;
                    }
                    else
                    {
                        currentDistancePenalty = (target.Length - i) / ((double)target.Length + 1);
                    }
                }
            }

            return result;
        }
    }
}
