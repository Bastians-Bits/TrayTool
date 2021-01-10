using System.Collections.ObjectModel;

namespace TrayTool.Model
{
    /// <summary>
    /// A stored list of arguments, identified by a unique name
    /// </summary>
    public class ArguementTemplate : BaseModel
    {
        private string _name;
        private ObservableCollection<Argument> _arguments;

        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// The list of arguments stored in the list
        /// </summary>
        public ObservableCollection<Argument> Arguments
        {
            get => _arguments;
            set => SetProperty(ref _arguments, value);
        }
    }
}
