using System.Collections.Generic;

namespace TrayTool.Repository.Model
{
    public class Item : AbstractItem
    {
        public string Path { get; set; }
        public IList<Argument> Arguments { get; set; }
    }
}
