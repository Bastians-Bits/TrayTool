using System;

namespace TrayTool.Model
{
    /// <summary>
    /// This class represents an abstract item. For an item to be abstract, it has to fullfill two conditions: 
    /// 1) It has to be presentable (inherited by Seperator) and 2) it has to be functional (inherited by BaeModel)
    /// </summary>
    [Serializable()]
    public abstract class AbstractItem : Seperator
    {
        private string _name;

        /// <summary>
        /// The Name of the Object, shown in the Dialog and the system tray
        /// </summary>
        public string Name { get => _name; set => SetProperty(ref _name, value); }
    }
}
