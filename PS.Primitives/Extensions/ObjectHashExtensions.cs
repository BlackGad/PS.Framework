namespace PS.Extensions
{
    public static class ObjectHashExtensions
    {
        #region Static members

        public static int GetHash(this object instance)
        {
            return instance?.GetHashCode() ?? 0;
        }

        public static int MergeHash(this int hash, params int[] addHashes)
        {
            foreach (var addHash in addHashes)
            {
                hash = (hash * 397) ^ addHash;
            }

            return hash;
        }

        #endregion
    }
}