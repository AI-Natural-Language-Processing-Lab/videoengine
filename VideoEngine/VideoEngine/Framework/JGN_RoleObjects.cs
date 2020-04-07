
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_RoleObjects
    {
        [Key]
        public short id { get; set; }
        [MaxLength(100)]
        public string objectname { get; set; }
        public string description { get; set; }
        [MaxLength(128)]
        public string uniqueid { get; set; }
    }
}
