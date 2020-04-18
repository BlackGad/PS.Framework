namespace PS.Graph.Algorithms
{
    public static class DistanceRelaxers
    {
        #region Constants

        public static readonly IDistanceRelaxer CriticalDistance = new CriticalDistanceRelaxer();

        public static readonly IDistanceRelaxer EdgeShortestDistance = new EdgeDistanceRelaxer();
        public static readonly IDistanceRelaxer ShortestDistance = new ShortestDistanceRelaxer();

        #endregion

        #region Nested type: CriticalDistanceRelaxer

        private sealed class CriticalDistanceRelaxer : IDistanceRelaxer
        {
            #region IDistanceRelaxer Members

            public double InitialDistance
            {
                get { return double.MinValue; }
            }

            public int Compare(double a, double b)
            {
                return -a.CompareTo(b);
            }

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            #endregion
        }

        #endregion

        #region Nested type: EdgeDistanceRelaxer

        private sealed class EdgeDistanceRelaxer : IDistanceRelaxer
        {
            #region IDistanceRelaxer Members

            public double InitialDistance
            {
                get { return 0; }
            }

            public int Compare(double a, double b)
            {
                return a.CompareTo(b);
            }

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            #endregion
        }

        #endregion

        #region Nested type: ShortestDistanceRelaxer

        private sealed class ShortestDistanceRelaxer : IDistanceRelaxer
        {
            #region IDistanceRelaxer Members

            public double InitialDistance
            {
                get { return double.MaxValue; }
            }

            public int Compare(double a, double b)
            {
                return a.CompareTo(b);
            }

            public double Combine(double distance, double weight)
            {
                return distance + weight;
            }

            #endregion
        }

        #endregion
    }
}