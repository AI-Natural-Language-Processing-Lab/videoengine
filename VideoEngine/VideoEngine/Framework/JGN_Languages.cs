
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_Languages
    {
        [Key]
        public short id { get; set; }
        [MaxLength(15)]
        public string culturename { get; set; }
        [MaxLength(100)]
        public string language { get; set; }
        [MaxLength(100)]
        public string region { get; set; }
        public byte isdefault { get; set; }
        public byte isselected { get; set; }
    }
}
