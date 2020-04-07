using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{   
    public partial class JGN_User_Ratings
    {
        [Key]
        public long id { get; set; }
        public long itemid { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public byte type { get; set; }
        public byte rating { get; set; }
    }
}
