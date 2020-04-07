
using Jugnoon.Entity;
using Jugnoon.Models;
using Jugnoon.Utility;
using Jugnoon.Videos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jugnoon.Framework
{    
    public partial class JGN_Videos
    {
        [Key]
        public long id { get; set; }

        [MaxLength(100)]
        public string userid { get; set; }
        [MaxLength(150)]
        public string title { get; set; }
        public string description { get; set; }
        [MaxLength(200)]
        public string tags { get; set; }
        [MaxLength(20)]
        public string duration { get; set; }
        public int views { get; set; }
        public int favorites { get; set; }
        public int total_rating { get; set; }
        public int comments { get; set; }
        public float ratings { get; set; }
        public float avg_rating { get; set; }
        [MaxLength(150)]
        public string videofilename { get; set; }
        [MaxLength(150)]
        public string thumbfilename { get; set; }
        [MaxLength(150)]
        public string originalvideofilename { get; set; }
        public string embed_script { get; set; }
        public byte isenabled { get; set; }
        public byte isprivate { get; set; }
        public byte iscomments { get; set; }
        public byte isratings { get; set; }
        public byte isfeatured { get; set; }
        public byte isexternal { get; set; }
        public byte isadult { get; set; }
        public int duration_sec { get; set; }
        public byte ispublished { get; set; }
        public byte isapproved { get; set; }
        [MaxLength(200)]
        public string pub_url { get; set; }
        [MaxLength(200)]
        public string org_url { get; set; }
        [MaxLength(200)]
        public string thumb_url { get; set; }
        [MaxLength(200)]
        public string coverurl { get; set; }
        [MaxLength(200)]
        public string preview_url { get; set; }
        public byte errorcode { get; set; }
        public DateTime created_at { get; set; }
        [MaxLength(15)]
        public string ipaddress { get; set; }
        public byte type { get; set; }
        public int liked { get; set; }
        public int disliked { get; set; }
        [MaxLength(150)]
        public string youtubeid { get; set; }
        public int downloads { get; set; }
        public byte mode { get; set; }
        [MaxLength(10)]
        public string authkey { get; set; }
        public long albumid { get; set; }
        public float price { get; set; }
        [MaxLength(500)]
        public string actors { get; set; }
        [MaxLength(500)]
        public string actresses { get; set; }
        public byte movietype { get; set; }
        [MaxLength(100)]
        public string streamoutputs { get; set; }
        [MaxLength(200)]
        public string thumb_preview { get; set; }

        // for internal use only
        [NotMapped]
        public string picturename { get; set; }
        [NotMapped]
        public string url { get; set; }
        [NotMapped]
        public string author_url { get; set; }
        [NotMapped]
        public string customize_date { get; set; }
        [NotMapped]
        public string shorttitle { get; set; }
      
        [NotMapped]
        public ApplicationUser author { get; set; }
        [NotMapped]
        public PlayerEntity player { get; set; }
        [NotMapped]
        public List<FileEntity> thumbs { get; set; }

        [NotMapped]
        public string[] categories { get; set; }

        [NotMapped]
        public List<JGN_CategoryContents> category_list { get; set; }

        [NotMapped]
        public bool isadmin { get; set; }
    }
}
