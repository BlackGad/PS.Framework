using System.Windows;

namespace PS.WPF.Data
{
    /// <summary>
    ///     This class is used to propagate inheritance context to non-visual class instances that might be part of the WPF
    ///     control element tree.
    ///     Note that while it relies on the special notion that Freezable classes have for "inheritance context", you should
    ///     not rely on any other characteristics
    ///     that Freezable objects usually have (cloning, freezing, etc.). Actually this object will explicitly prevent the
    ///     framework from freezing its
    ///     instances and it will generally behave as any other non-Freezable object.
    /// </summary>
    public class InheritanceContextPropagator : Freezable
    {
        #region Override members

        /// <summary>
        ///     When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable" />
        ///     derived class.
        /// </summary>
        /// <returns>
        ///     The new instance.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            return this;
        }

        /// <summary>
        ///     Makes the <see cref="T:System.Windows.Freezable" /> object unmodifiable or tests whether it can be made
        ///     unmodifiable.
        /// </summary>
        /// <param name="isChecking">
        ///     True to return an indication of whether the object can be frozen (without actually freezing
        ///     it); false to actually freeze the object.
        /// </param>
        /// <returns>
        ///     If <paramref name="isChecking" /> is true, this method returns true if the
        ///     <see cref="T:System.Windows.Freezable" /> can be made unmodifiable, or false if it cannot be made unmodifiable. If
        ///     <paramref name="isChecking" /> is false, this method returns true if the if the specified
        ///     <see cref="T:System.Windows.Freezable" /> is now unmodifiable, or false if it cannot be made unmodifiable.
        /// </returns>
        protected override bool FreezeCore(bool isChecking)
        {
            return false;
        }

        #endregion
    }
}