using System;
using System.ComponentModel.DataAnnotations;
namespace Jugnoon.Framework
{
    public partial class JGN_User_Account
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public short credits { get; set; }
        public DateTime? last_purchased { get; set; }
        public DateTime? membership_expiry { get; set; }
        public byte islifetimerenewal { get; set; }
        public byte paypal_subscriber { get; set; }
        [MaxLength(70)]
        public string paypal_email { get; set; }
    }
}
