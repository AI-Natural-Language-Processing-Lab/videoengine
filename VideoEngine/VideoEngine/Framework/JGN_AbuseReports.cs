using Jugnoon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_AbuseReports
    {
        [Key]
        public long id { get; set; }
        public long contentid { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        [MaxLength(30)]
        public string ipaddress { get; set; }
        public string reason { get; set; }
        public System.DateTime created_at { get; set; }
        public byte type { get; set; }
        public byte status { get; set; }
        public string review_comment { get; set; }

        [NotMapped]
        public ApplicationUser report_user { get; set; }
    }
}
