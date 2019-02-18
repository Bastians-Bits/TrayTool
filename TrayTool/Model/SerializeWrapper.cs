using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    [XmlRoot("BaseModels")]
    public class SerializeWrapper
    {
        [XmlElement("BaseModel")]
        public List<BaseModel> elements { get; set; }

        public SerializeWrapper() { }

        public SerializeWrapper(List<BaseModel> elements)
        {
            this.elements = elements;
        }
    }
}
