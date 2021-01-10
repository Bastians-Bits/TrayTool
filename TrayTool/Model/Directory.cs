using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    /// <summary>
    /// A basic drectory. Beside the normal values like name and image, it can story a list of children
    /// </summary>
    [System.Serializable()]
    public class Directory : AbstractItem
    {
        [XmlIgnore]
        private ObservableCollection<BaseModel> _children = new ObservableCollection<BaseModel>();
        
        /// <summary>
        /// A list of children below this directory
        /// </summary>
        [XmlArrayItem(typeof(Directory), ElementName = "Directory")]
        [XmlArrayItem(typeof(Item), ElementName = "Item")]
        [XmlArrayItem(typeof(Seperator), ElementName = "Seperator")]
        public ObservableCollection<BaseModel> Children { get => _children; set => SetProperty(ref _children, value); }
        

        public Directory()
        {
            Image = CreateImage("/TrayTool;component/Resources/Folder.png");
        }
    }
}
