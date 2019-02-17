namespace TrayTool.Model
{
    public class Argument : BaseModel
    {
        private string _value;
        private string _key;

        public string Value { get => _value; set => SetProperty(ref _value, value); }
        public string Key { get => _key; set => SetProperty(ref _key, value); }
    }
}
