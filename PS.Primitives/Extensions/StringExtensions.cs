namespace PS.Extensions
{
    public static class StringExtensions
    {
        #region Static members

        public static int Occurrences(this string input, char value)
        {
            var count = 0;
            for (var index = 0; index < input.Length; index++)
            {
                if (index == value) count++;
            }

            return count;
        }

        #endregion
    }
}