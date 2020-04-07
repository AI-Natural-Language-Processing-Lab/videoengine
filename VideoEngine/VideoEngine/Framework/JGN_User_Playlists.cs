using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{
    public partial class JGN_User_Playlists
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        public string description { get; set; }
        [MaxLength(300)]
        public string tags { get; set; }
        public DateTime created_at { get; set; }
        public int videos { get; set; }
        public byte isenabled { get; set; }
        public byte privacy { get; set; }
        [MaxLength(150)]
        public string picturename { get; set; }
        public byte isapproved { get; set; }
    }
}
