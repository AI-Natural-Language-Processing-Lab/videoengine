using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{ 
    public partial class JGN_RolePermissions
    {
        [Key]
        public short id { get; set; }
        public short roleid { get; set; }
        public short objectid { get; set; }

        [NotMapped]
        public JGN_RoleObjects robject { get; set; }
    }
}
