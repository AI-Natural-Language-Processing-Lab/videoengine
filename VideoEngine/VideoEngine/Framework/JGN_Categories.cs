using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_Categories
    {
        [Key]
        public short id { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        [MaxLength(150)]
        public string term { get; set; }
        public short parentid { get; set; }
        public byte type { get; set; }
        public int priority { get; set; }
        public byte isenabled { get; set; }
      
        [MaxLength(150)]
        public string picturename { get; set; }
        public string description { get; set; }
        public int records { get; set; }

        [MaxLength(100)]
        public string level { get; set; }
        [MaxLength(30)]
        public string icon { get; set; }
        public byte mode { get; set; }
        // internal use only
        [NotMapped]
        public string img_url { get; set; }
        [NotMapped]
        public string path { get; set; }
    }
}
