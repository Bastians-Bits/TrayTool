using System;

namespace TrayTool.Repository.Model
{
    public class Argument
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Concatenator { get; set; }
    }
}
