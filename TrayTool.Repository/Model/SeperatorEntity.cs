using System.ComponentModel.DataAnnotations.Schema;

namespace TrayTool.Repository.Model
{
    public class SeperatorEntity : BaseModelEntity
    {
        [NotMapped]
        private DirectoryEntity _parent;
        public virtual DirectoryEntity Parent { 
            get 
            {
                if (_parent == null)
                    _parent = new DirectoryEntity();
                return _parent;
            }
            set 
            {
                _parent = value; 
            } 
        }
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public bool IsExpanded { get; set; }
        public byte[] Image { get; set; }
        public string ImagePath { get; set; }
    }
}
