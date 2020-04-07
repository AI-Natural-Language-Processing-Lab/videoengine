using Jugnoon.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_Comments
    {
        [Key]
        public long id { get; set; }
        public long contentid { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }
        public byte isenabled { get; set; }
        public byte type { get; set; }
        public int points { get; set; }
        public byte isapproved { get; set; }
        public long replyid { get; set; }
        [MaxLength(100)]
        public string levels { get; set; }
        public short replies { get; set; }

        [NotMapped]
        public string author_url { get; set; }

        [NotMapped]
        public ApplicationUser author { get; set; }
    }
}
