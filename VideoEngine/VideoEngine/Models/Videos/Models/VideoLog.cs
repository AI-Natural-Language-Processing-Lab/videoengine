using System.Collections.Generic;

namespace Jugnoon.Videos.Models
{
    public class PublishedReport
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<VideoLog> report { get; set; }

    }

    public class VideoLog
    {
        public string sourcefile { get; set; }
        public string publishedfile { get; set; }
        public string status { get; set; }
        public string errorcode { get; set; }
        public string message { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
