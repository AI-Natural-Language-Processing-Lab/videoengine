using System.ComponentModel.DataAnnotations;
namespace Jugnoon.Framework
{   
    public partial class JGN_Ads
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string name { get; set; }
        public string adscript { get; set; }
        public byte type { get; set; }
    }
}
