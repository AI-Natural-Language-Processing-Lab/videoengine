using System.ComponentModel.DataAnnotations;
namespace Jugnoon.Framework
{
    public partial class JGN_User_Stats
    {
        [Key]
        public long id { get; set; }
        [MaxLength(100)]
        public string userid { get; set; }
        public int stat_videos { get; set; }
        public int stat_audio { get; set; }
        public int stat_photos { get; set; }
        public int stat_albums { get; set; }
        public int stat_blogs { get; set; }
        public int stat_forum_topics { get; set; }
        public int stat_qa { get; set; }
        public int stat_qaanswers { get; set; }
        public int stat_polls { get; set; }
        public int stat_adlistings { get; set; }
    }
}
