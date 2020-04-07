
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_Roles
    {
        [Key]
        public short id { get; set; }
        [MaxLength(100)]
        public string rolename { get; set; }
        public System.DateTime created_at { get; set; }

        [NotMapped]
        public List<JGN_RolePermissions> permissions { get; set; }
    }
}
