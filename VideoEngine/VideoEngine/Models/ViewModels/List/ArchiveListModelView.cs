using Jugnoon.Utility;
using System.Collections.Generic;

namespace Jugnoon.Models
{  
    public class ArchiveListModelView
    {
        public ContentTypes Type { get; set; } = ContentTypes.Videos;
        public int Mediatype { get; set; } = 0;
        public int TotalRecords { get; set; } = 10;
        public bool isAll { get; set; } = false;
        public string HeadingTitle { get; set; } = "Archive";
        public string Path { get; set; } = "";
        public List<BreadItem> BreadItems { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
