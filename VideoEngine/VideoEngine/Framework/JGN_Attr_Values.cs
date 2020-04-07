using System.ComponentModel.DataAnnotations;
namespace Jugnoon.Framework
{
    public partial class JGN_Attr_Values
    {
        [Key]
        public long id { get; set; }
        public short attr_id { get; set; }
        public long contentid { get; set; } = 0;
        public string userid { get; set; }
        [MaxLength(300)]
        public string title { get; set; }
        [MaxLength(500)]
        public string value { get; set; }
        public short priority { get; set; }
        public byte attr_type { get; set; }
        
    }
}
