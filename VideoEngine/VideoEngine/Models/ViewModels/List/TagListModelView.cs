using System.Collections.Generic;

namespace Jugnoon.Models
{   
    public class TagListModelView
    {

        public bool isMain { get; set; } = false;

        public string HeadingTitle { get; set; } = "";

        public string Term { get; set; } = "";

        /// <summary>
        ///  0: videos, 1: groups, 2: photos, 3: blogs, 4: audio files, 5: general, 6: galleries, 7: forums, 100: all
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 0: normal tags, 1: search tags, 2: all tags
        /// </summary>
        public int Tag_Type { get; set; } = 0;

        /// <summary>
        ///  0: normal tags, 1: search tags, 2: all tags
        /// </summary>
        public int Tag_Level { get; set; } = 100;

        public int pagenumber { get; set; } = 1;

        public string Order { get; set; } = "id desc";

        public bool iscache { get; set; } = false;

        public int TotalRecords { get; set; } = 10;

        public string Path { get; set; } = "";

        public string DefaultUrl { get; set; } = "";

        public string PaginationUrl { get; set; } = "";

        public string NoRecordFoundText { get; set; } = "";

        public string Action { get; set; } = "";

        public string Query { get; set; } = "";

        public List<BreadItem> BreadItems { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
