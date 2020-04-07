
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_Tokens
    {
        [Key]
        public int id { get; set; }

        [MaxLength(50)]
        public string token { get; set; }

        public System.DateTime created_at { get; set; }
    }
}
