using System;

namespace TrayTool.Repository.Model
{
    public class ArgumentEntity
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Concatenator { get; set; }
    }
}
