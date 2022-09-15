namespace PS.ComponentModel.Navigation
{
    public static class Routes
    {
        public static Route Empty { get; }

        public static Route Wildcard { get; }

        public static Route WildcardRecursive { get; }

        static Routes()
        {
            Empty = Route.Create();
            Wildcard = Route.Create(Route.Wildcard);
            WildcardRecursive = Route.Create(Route.WildcardRecursive);
        }
    }
}
