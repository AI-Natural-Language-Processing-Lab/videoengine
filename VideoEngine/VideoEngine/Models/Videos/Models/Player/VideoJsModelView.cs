
using System.Collections.Generic;
using Jugnoon.Framework;

namespace Jugnoon.Videos.Models
{
    public class VideoJsModelView
    {
        public bool enabledAWS { get; set; }
        public List<SupportedVideos> VideoFeed { get; set; }
        public long VideoID { get; set; }
        public int Type { get; set; }
        public string PictureUrl { get; set; }
        public string VideoUrl { get; set; }
        public string EmbedScript { get; set; }
        public JGN_Videos Data { get; set; }
    }

    public class SupportedVideos
    {
        public string src { get; set; }
        public string type { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
