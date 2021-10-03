namespace TrayTool.Repository.Model
{
    public class Argument : BaseModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Concatenator { get; set; }
    }
}
