using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public class ArguementTemplate : BaseModel
    {
        private string _name;
        private ObservableCollection<Argument> _arguments;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<Argument> Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
        }
    }
}
