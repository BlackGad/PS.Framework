using System;
using PS.Data.Descriptors;

namespace PS.WPF.Docking.Layout
{
    [Serializable]
    public class DockingLayoutArea : ICloneable
    {
        #region Properties

        public Descriptor Window { get; set; }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region Members

        public DockingLayoutArea Clone()
        {
            return new DockingLayoutArea();
        }

        #endregion
    }
}