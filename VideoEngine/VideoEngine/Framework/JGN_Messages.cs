using Jugnoon.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Messages
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string from_uid { get; set; }
        [MaxLength(256)]
        public string subject { get; set; }
        public string body { get; set; }
        public long reply_id { get; set; }

        [NotMapped]
        public ApplicationUser user { get; set; }
    }
}
