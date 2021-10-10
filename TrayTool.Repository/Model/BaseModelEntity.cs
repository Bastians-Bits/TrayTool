using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrayTool.Repository.Model
{
    public class BaseModelEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
