using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrayTool.Repository.Model
{
    public class DirectoryEntity : AbstractItemEntity
    {
        [NotMapped]
        private IList<BaseModelEntity> _children;
        public virtual IList<BaseModelEntity> Children 
        {
            get
            {
                if (_children == null)
                    _children = new List<BaseModelEntity>();
                return _children;
            }
            set
            {
                _children = value;
            }
        }
    }
}
