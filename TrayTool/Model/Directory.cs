
using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public class Directory : AbstractItem
    {
        private ObservableCollection<BaseModel> _children = new ObservableCollection<BaseModel>();

        public Directory()
        {
            Image = "/TrayTool;component/Resources/Folder.png";
        }

        public ObservableCollection<BaseModel> Children
        {
            get => _children;
            set
            {
                if (_children != value)
                {
                    _children = value;
                    OnPropertyChanged("Children");
                }
            }
        }
    }
}
