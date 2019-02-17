using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public class Item : AbstractItem
    {
        private string _path;
        private ObservableCollection<Argument> _arguments;

        public string Path { get => _path; set => SetProperty(ref _path, value); }
        public ObservableCollection<Argument> Arguments { get => _arguments; set => SetProperty(ref _arguments, value); }

        public Item()
        {
            Image = "/TrayTool;component/Resources/Shortcut.png";
        }
    }
}
