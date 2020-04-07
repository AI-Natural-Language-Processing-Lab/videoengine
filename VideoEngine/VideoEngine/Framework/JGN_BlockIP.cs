
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_BlockIP
    {
        [Key]
        public int id { get; set; }
        [MaxLength(50)]
        public string ipaddress { get; set; }
        public System.DateTime created_at { get; set; }
    }
}
