using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public class Directory : AbstractItem
    {
        private ObservableCollection<Seperator> _children = new ObservableCollection<Seperator>();

        public ObservableCollection<Seperator> Children { get => _children; set => SetProperty(ref _children, value); }

        public Directory()
        {
            Image = "/TrayTool;component/Resources/Folder.png";
        }

       
    }
}
