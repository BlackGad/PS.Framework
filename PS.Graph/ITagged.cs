using System;

namespace PS.Graph
{
    /// <summary>
    ///     An instance holding a tag
    /// </summary>
    /// <typeparam name="TTag"></typeparam>
    public interface ITagged<TTag>
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the tag
        /// </summary>
        TTag Tag { get; set; }

        #endregion

        #region Events

        /// <summary>
        ///     Raised when the tag is changed
        /// </summary>
        event EventHandler TagChanged;

        #endregion
    }
}