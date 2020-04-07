using Jugnoon.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Messages_Recipents
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string to_uid { get; set; }
        public long messageid { get; set; }
        public DateTime? msg_sent { get; set; }
        public DateTime? msg_read { get; set; }
        public DateTime? msg_deleted { get; set; }
        [NotMapped]
        public ApplicationUser user { get; set; }
        public JGN_Messages message { get; set; }
    }
}
