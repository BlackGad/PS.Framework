using System.ComponentModel;
using PS.Data.Descriptors;
using PS.WPF.Docking.Controls;

namespace PS.WPF.Docking.Components
{
    [TypeConverter(typeof(DockingHostDescriptorConverter))]
    public class DockingHostDescriptor : Descriptor<DockingArea>
    {
        #region Constructors

        public DockingHostDescriptor(string id = null)
            : base(id)
        {
        }

        #endregion
    }
}