using System;
using System.ComponentModel.DataAnnotations;

namespace TrayTool.Repository.Model
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}
