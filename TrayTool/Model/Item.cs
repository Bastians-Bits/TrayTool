using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public class Item : AbstractItem
    {
        private string _path;
        private ObservableCollection<Argument> _arguments;

        public Item()
        {
            Image = "/TrayTool;component/Resources/Shortcut.png";
        }

        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        
        public ObservableCollection<Argument> Arguments
        {
            get => _arguments;
            set
            {
                if (_arguments != value)
                {
                    _arguments = value;
                    OnPropertyChanged("Arguments");
                } 
            }
        }
    }
}
