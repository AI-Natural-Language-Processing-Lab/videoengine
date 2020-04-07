using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Attr_TemplateSections
    {
        [Key]
        public short id { get; set; }
        public short templateid { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        public short priority { get; set; }
        public byte attr_type { get; set; }
        public byte showsection { get; set; }

        [NotMapped]
        public List<JGN_Attr_Attributes> attributes { get; set; }
    }
}
