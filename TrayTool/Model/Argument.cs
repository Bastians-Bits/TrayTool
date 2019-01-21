namespace TrayTool.Model
{
    public class Argument : AbstractModel
    {
        private string _key;
        private string _value;

        public string Value
        {
            get => _value; set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        public string Key
        {
            get => _key; set
            {
                if (_key != value)
                {
                    _key = value;
                    OnPropertyChanged("Key");
                }
            }
        }
    }
}
