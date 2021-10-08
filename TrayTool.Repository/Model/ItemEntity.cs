using System.Collections.Generic;

namespace TrayTool.Repository.Model
{
    public class ItemEntity : AbstractItemEntity
    {
        public string Path { get; set; }
        public virtual ICollection<ArgumentEntity> Arguments { get; set; } = new List<ArgumentEntity>();
    }
}
