
namespace Jugnoon.Models
{
    public class NavViewModel
    {
        public bool hideSearch { get; set; }
        public bool hideCategories { get; set; }
        public bool hideTags { get; set; }
        public bool hideAds { get; set; }
        public bool hideArchives { get; set; }

        public int CategoryType { get; set; }
        public int TagType { get; set; }
        public int ArchiveType { get; set; }
        public string Path { get; set; }

        // Adult content notification to navigation (for advertisement toggles)
        public bool isAdultContent { get; set; }
        
        // search params
        public string searchTitle { get; set; }
        public string searchAction { get; set; }
        public string searchPlaceHolder { get; set; }
        public string searchAdvanceUrl { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
