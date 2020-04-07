
namespace Jugnoon.Scripts
{   
    public class ListItems
    {
        public bool showAlbumPreview { get; set; } = false;
        public bool isHover { get; set; } = false;
        public ItemType ItemType { get; set; } = ItemType.Div;
        public ListType ListType { get; set; } = ListType.Grid;
        public string ColWidth { get; set; } = "col-md-4";
        public bool showTitle { get; set; } = true;
        public bool showGender { get; set; } = false; 
        public bool showDetail { get; set; } = false;
        // // video / audio property
        public bool showDate { get; set; } = true;
        public bool showUser { get; set; } = true;
        public bool showRating { get; set; } = true;
        public bool showDescription { get; set; } = false;
        public bool showCategories { get; set; } = false;
        public bool showTags { get; set; } = false;
        public bool showViews { get; set; } = true;
        public bool showDownloads { get; set; } = false;
        public bool showDuration { get; set; } = false; // video / audio property
        public bool showLocationInfo { get; set; } = false; // member / user /channel property
        public int TitleLength { get; set; } = 0;
        public int descriptionlength { get; set; } = 0;
        public int CategoryLength { get; set; } = 0;
        public int TagLength { get; set; } = 0;
        public int DateFormat { get; set; } = 3; //  // 0:  21 May, 2011, 1: May 30th, 2011, 2: May 11 2011, 3: 2 days ago, 4: Today 10:54 PM
        public bool PhotoOnly { get; set; } = false;
        public bool isresize { get; set; } = false;
        public string size { get; set; } = "";
    }

    public enum ItemType
    {
        Div = 0,
        Link = 1
    }

    public enum ListType
    {
        Grid = 0,
        List = 1,
        Links = 2
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
