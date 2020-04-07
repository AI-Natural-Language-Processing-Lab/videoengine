using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{  
    public partial class JGN_Packages
    {
        [Key]
        public byte id { get; set; }
        [MaxLength(100)]
        public string name { get; set; }
        public string description { get; set; }
        public byte isenabled { get; set; }
        public float price { get; set; }
        public DateTime created_at { get; set; }
        public byte type { get; set; }
        public int credits { get; set; }
        public byte package_type { get; set; }
        [MaxLength(5)]
        public string currency { get; set; }
        public int months { get; set; }
        public float discount { get; set; }
    }
}
