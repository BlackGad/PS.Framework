using System;

namespace PS.Data.Fuzzy.Computers
{
    public class LevenshteinDistanceComputer : FuzzyCoefficientComputer
    {
        private static double ComputeDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return (target ?? "").Length;
            if (string.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                var temp = source;
                source = target;
                target = temp; // swap s and t
            }

            var sourceLength = source.Length; // this is also the minimum length of the two strings
            var targetLength = target.Length;

            // suffix common to both strings can be ignored
            while (sourceLength > 0 && source[sourceLength - 1] == target[targetLength - 1])
            {
                sourceLength--;
                targetLength--;
            }

            var start = 0;
            if (source[0] == target[0] || sourceLength == 0)
            {
                // if there's a shared prefix, or all s matches t's suffix
                // prefix common to both strings can be ignored
                while (start < sourceLength && source[start] == target[start])
                {
                    start++;
                }

                sourceLength -= start; // length of the part excluding common prefix and suffix
                targetLength -= start;

                // if all of shorter string matches prefix and/or suffix of longer string, then
                // edit distance is just the delete of additional characters present in longer string
                if (sourceLength == 0) return targetLength;

                target = target.Substring(start, targetLength); // faster than t[start+j] in inner loop below
            }

            var v0 = new int[targetLength];
            for (var j = 0; j < targetLength; j++)
            {
                v0[j] = j + 1;
            }

            var current = 0;
            for (var i = 0; i < sourceLength; i++)
            {
                var sourceChar = source[start + i];
                var left = current = i;
                for (var j = 0; j < targetLength; j++)
                {
                    var above = current;
                    current = left; // cost on diagonal (substitution)
                    left = v0[j];
                    if (sourceChar != target[j])
                    {
                        current++;              // substitution
                        var insDel = above + 1; // deletion
                        if (insDel < current) current = insDel;
                        insDel = left + 1; // insertion
                        if (insDel < current) current = insDel;
                    }

                    v0[j] = current;
                }
            }

            return current;
        }

        public override double Compute(string input, string target)
        {
            var distance = ComputeDistance(input, target);
            var lowerBound = Math.Abs(input.Length - target.Length);
            var coefficient = (distance - lowerBound) / (Math.Max(input.Length, target.Length) - lowerBound);
            return 1 - coefficient;
        }
    }
}
