using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using PS.Data.Descriptors;
using PS.Extensions;

namespace PS.WPF.Docking.Layout
{
    [Serializable]
    public class DockingLayout : IXmlSerializable,
                                 ICloneable
    {
        #region Constructors

        public DockingLayout()
        {
            Windows = new Dictionary<Descriptor, DockingLayoutWindow>();
            Areas = new Dictionary<Descriptor, DockingLayoutArea>();
        }

        #endregion

        #region Properties

        public Dictionary<Descriptor, DockingLayoutArea> Areas { get; set; }
        public Dictionary<Descriptor, DockingLayoutWindow> Windows { get; set; }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotSupportedException();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(Windows));
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(Areas));
            writer.WriteEndElement();

            throw new NotImplementedException();
        }

        #endregion

        #region Members

        public DockingLayout Clone()
        {
            return new DockingLayout
            {
                Areas = Areas.Enumerate().ToDictionary(pair => pair.Key, pair => pair.Value.Clone()),
                Windows = Windows.Enumerate().ToDictionary(pair => pair.Key, pair => pair.Value.Clone())
            };
        }

        #endregion
    }
}