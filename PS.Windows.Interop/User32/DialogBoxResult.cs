using System;

// ReSharper disable UnusedMember.Global

namespace PS.Windows.Interop
{
    /// <summary>
    ///     Dialog Box Command IDs
    /// </summary>
    [Flags]
    public enum DialogBoxResult : uint
    {
        /// <summary>
        ///     Dialogbox result is OK
        /// </summary>
        OK = 1,

        /// <summary>
        ///     Dialogbox result is Cancel
        /// </summary>
        Cancel = 2,

        /// <summary>
        ///     Dialogbox result is Abort
        /// </summary>
        Abort = 3,

        /// <summary>
        ///     Dialogbox result is Retry
        /// </summary>
        Retry = 4,

        /// <summary>
        ///     Dialogbox result is Ignore
        /// </summary>
        Ignore = 5,

        /// <summary>
        ///     Dialogbox result is Yes
        /// </summary>
        Yes = 6,

        /// <summary>
        ///     Dialogbox result is No
        /// </summary>
        No = 7,

        /// <summary>
        ///     Dialogbox result is Close
        /// </summary>
        Close = 8,

        /// <summary>
        ///     Dialogbox result is Help
        /// </summary>
        Help = 9,

        /// <summary>
        ///     Dialogbox result is Try again
        /// </summary>
        TryAgain = 10,

        /// <summary>
        ///     Dialogbox result is Continue
        /// </summary>
        Continue = 11
    }
}