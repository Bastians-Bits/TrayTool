using System.Collections.Generic;

namespace TrayTool.Repository.Model
{
    public class Directory : AbstractItem
    {
        public IList<BaseModel> Children { get; set; }
    }
}
