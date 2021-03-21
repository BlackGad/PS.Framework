﻿using System;

namespace PS.Graph.Algorithms
{
    /// <summary>
    ///     The computation state of a graph algorithm
    /// </summary>
    [Serializable]
    public enum ComputationState
    {
        /// <summary>
        ///     The algorithm is not running
        /// </summary>
        NotRunning,

        /// <summary>
        ///     The algorithm is running
        /// </summary>
        Running,

        /// <summary>
        ///     An abort has been requested. The algorithm is still running and will cancel as soon as it checks
        ///     the cancellation state
        /// </summary>
        PendingAbortion,

        /// <summary>
        ///     The computation is finished successfully.
        /// </summary>
        Finished,

        /// <summary>
        ///     The computation was aborted
        /// </summary>
        Aborted
    }
}