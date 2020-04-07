
using System;
using System.ComponentModel.DataAnnotations;

namespace Jugnoon.Framework
{    
    public partial class JGN_Playlist_Videos
    {
        [Key]
        public long id { get; set; }
        public long contentid { get; set; }
        public System.DateTime created_at { get; set; }
    }
}
