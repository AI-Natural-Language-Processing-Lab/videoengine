using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Attr_Templates
    {
        [Key]
        public short id { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        public byte attr_type { get; set; }

        [NotMapped]
        public List<JGN_Attr_TemplateSections> sections { get; set; }
    }
}
