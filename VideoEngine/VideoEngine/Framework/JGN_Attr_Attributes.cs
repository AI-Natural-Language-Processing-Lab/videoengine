using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Attr_Attributes
    {
        [Key]
        public short id { get; set; }
        public short sectionid { get; set; }
        [MaxLength(300)]
        public string title { get; set; }
        [MaxLength(500)]
        public string value { get; set; }
        public short priority { get; set; }
        public byte attr_type { get; set; }
        public string options { get; set; }
        public byte element_type { get; set; }
        public byte isrequired { get; set; }
        public byte variable_type { get; set; }
        [MaxLength(150)]
        public string icon { get; set; }
        [MaxLength(200)]
        public string helpblock { get; set; }
        public short min { get; set; }
        public short max { get; set; }
        public string postfix { get; set; }
        public string prefix { get; set; }
        public string tooltip { get; set; }
        public string url { get; set; }
        [NotMapped]
        public bool isdeleted { get; set; }
    }
}
