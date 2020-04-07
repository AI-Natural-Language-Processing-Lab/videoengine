using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_CategoryContents
    {
        [Key]
        public long id { get; set; }
        public short categoryid { get; set; }
        public long contentid { get; set; }
        public byte type { get; set; }

        [NotMapped]
        public JGN_Categories category { get; set; }
    }
}
