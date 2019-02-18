namespace TrayTool.Model
{
    [System.Serializable()]
    public class Argument : BaseModel
    {
        private string _value;
        private string _key;
        private string _concatenator;

        public string Value { get => _value; set => SetProperty(ref _value, value); }
        public string Key { get => _key; set => SetProperty(ref _key, value); }
        public string Concatenator { get => _concatenator; set => SetProperty(ref _concatenator, value); }
    }
}
