using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    public abstract class AbstractItem : BaseModel
    {
        private string _name;

        public string Name { get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }
}
