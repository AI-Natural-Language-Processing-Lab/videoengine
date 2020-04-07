
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{
    public partial class JGN_Tags
    {
        [Key]
        public int id { get; set; }
        [MaxLength(100)]
        public string title { get; set; }
        public byte isenabled { get; set; }
        public byte tag_level { get; set; }
        public byte priority { get; set; }
        public byte type { get; set; }
        public int records { get; set; }
        public byte tag_type { get; set; }
        [MaxLength(100)]
        public string term { get; set; }
    }
}
