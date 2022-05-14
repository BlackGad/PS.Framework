using System;

namespace PS.WPF.Docking.Layout
{
    [Serializable]
    public class DockingLayoutWindow : ICloneable
    {
        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public DockingLayoutWindow Clone()
        {
            return new DockingLayoutWindow();
        }

        #endregion
    }
}