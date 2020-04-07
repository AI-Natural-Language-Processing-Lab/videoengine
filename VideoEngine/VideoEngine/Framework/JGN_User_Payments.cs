
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_User_Payments
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public short package_id { get; set; }
        public float price { get; set; }
        public float credits { get; set; }
        public DateTime created_at { get; set; }
        public int months { get; set; }
        public byte isenabled { get; set; }
        [MaxLength(50)]
        public string transactionid { get; set; }
        [MaxLength(70)]
        public string payer_email { get; set; }
        [MaxLength(150)]
        public string item_name { get; set; }
        [MaxLength(20)]
        public string payment_status { get; set; }
        [MaxLength(100)]
        public string pending_reason { get; set; }
        [MaxLength(20)]
        public string payment_fee { get; set; }
        [MaxLength(20)]
        public string payment_gross { get; set; }
        [MaxLength(20)]
        public string txn_type { get; set; }
        [MaxLength(50)]
        public string first_name { get; set; }
        [MaxLength(50)]
        public string last_name { get; set; }
        [MaxLength(100)]
        public string address_street { get; set; }
        [MaxLength(100)]
        public string address_city { get; set; }
        [MaxLength(100)]
        public string address_state { get; set; }
        [MaxLength(100)]
        public string address_zip { get; set; }
        [MaxLength(100)]
        public string address_country { get; set; }
        [MaxLength(20)]
        public string address_status { get; set; }
        [MaxLength(20)]
        public string payer_status { get; set; }
        [MaxLength(20)]
        public string payer_id { get; set; }
        [MaxLength(20)]
        public string payment_type { get; set; }

        [NotMapped]
        public JGN_Packages packages { get; set; }
    }
}
