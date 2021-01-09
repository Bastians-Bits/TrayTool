using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TrayTool.Model
{
    // See also SerializeWraqpper and Directory
    [XmlInclude(typeof(Directory))]
    [XmlInclude(typeof(Item))]
    [XmlInclude(typeof(Seperator))]
    [System.Serializable()]
    /// <summary>
    /// Abstract class for models.
    /// Contains just the PropertyChanged- Methods.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}