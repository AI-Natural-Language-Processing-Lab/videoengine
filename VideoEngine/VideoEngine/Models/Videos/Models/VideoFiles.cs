using System.Collections.Generic;
using Jugnoon.Framework;
using Jugnoon.Utility;

namespace Jugnoon.Videos.Models
{

    public class VideoThumbs
    {
        public string filename { get; set; }
        public string fileIndex { get; set; }
        public bool selected { get; set; }
    }

    public class VideoFiles
    {
        public string username { get; set; }
        public string errorcode { get; set; }
        public string filename { get; set; }
        public string sfle { get; set; }
        public string duration { get; set; }
        public string durationsec { get; set; }
    }


    public class EncodeFFMPEGVideo
    {
        public string userid { get; set; }
        public string pid { get; set; }
        public string sf { get; set; } // source file
        public string pf { get; set; } // publish file
        public int tp { get; set; } // action type
    }

    public class SaveVideoInfo : JGN_Videos
    {
        public string pf { set; get; }
        public string sf { set; get; }
        public string tfile { set; get; }
        public string img_url { get; set; }
        public byte privacy { set; get; }
        public int dursec { set; get; }
        public List<VideoThumbs> video_thumbs { get; set; }
    }

    public class YoutubeEntity
    {
        public string userid { get; set; }
        public string term { get; set; }
        public string[] categories { get; set; }
        public int order { get; set; }
        public int uploaddate { get; set; }
        public string youtubecategory { get; set; }
        public string tags { get; set; }
        public bool isapproved { get; set; }

        public List<Yt_Category> YoutubeCategories { get; set; }
        public Dictionary<string, string> OrderList { get; set; }
        public Dictionary<string, string> DateList { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
