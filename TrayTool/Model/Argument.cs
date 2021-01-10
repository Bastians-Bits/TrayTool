namespace TrayTool.Model
{
    /// <summary>
    /// A single argument with a key, a value and a concetenator. All three will be used to build the finished cli argument
    /// </summary>
    [System.Serializable()]
    public class Argument : BaseModel
    {
        private string _value;
        private string _key;
        private string _concatenator;

        /// <summary>
        /// The value of the argument
        /// </summary>
        public string Value { get => _value; set => SetProperty(ref _value, value); }
        /// <summary>
        /// The key of the argument
        /// </summary>
        public string Key { get => _key; set => SetProperty(ref _key, value); }
        /// <summary>
        /// The concetenator of the argument
        /// </summary>
        public string Concatenator { get => _concatenator; set => SetProperty(ref _concatenator, value); }
    }
}
