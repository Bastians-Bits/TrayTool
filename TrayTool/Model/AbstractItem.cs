using System;

namespace TrayTool.Model
{
    [Serializable()]
    public abstract class AbstractItem : Seperator
    {
        private string _name;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
    }
}
