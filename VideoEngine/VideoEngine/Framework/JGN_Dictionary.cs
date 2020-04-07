
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_Dictionary
    {
        [Key]
        public int id { get; set; }
        [MaxLength(100)]
        public string value { get; set; }
        public byte type { get; set; }
    }
}
