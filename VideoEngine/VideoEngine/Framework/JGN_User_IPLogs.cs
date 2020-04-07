using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{   
    public partial class JGN_User_IPLogs
    {
        [Key]
        public int id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        [MaxLength(20)]
        public string ipaddress { get; set; }
        public DateTime created_at { get; set; }
    }
}
