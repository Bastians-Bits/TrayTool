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
    /// The base model for all other displayed models. It provides all functions to work properly
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Each time a property is changed and the view has to be notified, this method has to be called
        /// </summary>
        /// <typeparam name="T">The type of the set value</typeparam>
        /// <param name="field">A reference to the field</param>
        /// <param name="newValue">The new value of the field</param>
        /// <param name="propertyName">The name of the changed property. Filled by the runtime</param>
        /// <returns>True, if the event has been invoked, otherwise false</returns>
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