namespace PS.ComponentModel.Navigation
{
    public static class Routes
    {
        #region Static members

        public static Route Empty { get; }

        public static Route Wildcard { get; }

        public static Route WildcardRecursive { get; }

        #endregion

        #region Constructors

        static Routes()
        {
            Empty = Route.Create();
            Wildcard = Route.Create(Route.Wildcard);
            WildcardRecursive = Route.Create(Route.WildcardRecursive);
        }

        #endregion
    }
}