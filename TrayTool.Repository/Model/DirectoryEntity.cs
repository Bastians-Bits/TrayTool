using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrayTool.Repository.Model
{
    public class DirectoryEntity : AbstractItemEntity
    {
        [NotMapped]
        private IList<SeperatorEntity> _children;
        public virtual IList<SeperatorEntity> Children 
        {
            get
            {
                if (_children == null)
                    _children = new List<SeperatorEntity>();
                return _children;
            }
            set
            {
                _children = value;
            }
        }
    }
}
