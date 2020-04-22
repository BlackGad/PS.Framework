using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PS.Patterns.Aware;

namespace PS.Shell.Module.Diagram.Test
{
    public class MyVertex : IIDAware,
                            INameAware,
                            IDescriptionAware
    {
        #region Constructors

        public MyVertex(string name)
            : this()
        {
            Name = name;
        }

        public MyVertex()
        {
            Groups = new List<string>();
            Id = Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Properties

        [XmlArray]
        [XmlArrayItem("Group")]
        public List<string> Groups { get; set; }

        #endregion

        #region Override members

        public override string ToString()
        {
            var result = $"{GetType().Name}";
            if (!string.IsNullOrEmpty(Name)) result += $": ({Name})";
            return result;
        }

        #endregion

        #region IDescriptionAware Members

        [XmlAttribute]
        public string Description { get; set; }

        #endregion

        #region IIDAware Members

        [XmlAttribute]
        public string Id { get; set; }

        #endregion

        #region INameAware Members

        [XmlAttribute]
        public string Name { get; set; }

        #endregion
    }
}