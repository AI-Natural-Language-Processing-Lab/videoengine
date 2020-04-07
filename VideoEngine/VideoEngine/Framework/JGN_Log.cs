using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{     
    public partial class JGN_Log
    {
        [Key]
        public int id { get; set; }
        [MaxLength(500)]
        public string description { get; set; }
        [MaxLength(300)]
        public string url { get; set; }
        public string stack_trace { get; set; }
        public System.DateTime created_at { get; set; }
    }
}
