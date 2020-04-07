
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_Favorites
    {
        [Key]
        public long id { get; set; }
        public long contentid { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public System.DateTime created_at { get; set; }
        public byte type { get; set; }
    }
}
