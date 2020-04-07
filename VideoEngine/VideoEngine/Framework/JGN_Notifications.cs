using Jugnoon.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{
    public partial class JGN_Notifications
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string sender_id { get; set; }
        public byte notification_type { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        [MaxLength(200)]
        public string body { get; set; }
        [MaxLength(150)]
        public string href { get; set; }
        [MaxLength(100)]
        public string recipient_id { get; set; }
        public byte is_unread { get; set; }
        public byte is_hidden { get; set; }
        public DateTime created_time { get; set; }

        [NotMapped]
        public ApplicationUser from { get; set; }
    }
}
