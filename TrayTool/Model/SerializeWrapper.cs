using System.Collections.Generic;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    [XmlRoot("root")]
    public class SerializeWrapper
    {
        [XmlArray("elements")]
        [XmlArrayItem(typeof(Directory), ElementName = "Directory")]
        [XmlArrayItem(typeof(Item), ElementName = "Item")]
        [XmlArrayItem(typeof(Seperator), ElementName = "Seperator")]
        public List<BaseModel> Elements { get; set; }

        public SerializeWrapper() { }

        public SerializeWrapper(List<BaseModel> elements)
        {
            Elements = elements;
        }
    }
}
