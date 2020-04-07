
namespace Jugnoon.Models
{
    public class SearchListModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HeadingTitle { get; set; }
        public string PlaceHolder { get; set; }
        public string AdvanceSearchUrl { get; set; }
        public string Query { get; set; }
        public string Type { get; set; } // used in albums e.g videos, photos, audio
        public int SearchType { get; set; } = 0;
        public string SearchIcon { get; set; } = "fa-video";
        public bool showAdvanceSearch { get; set; } = true;

        // additional button setup e.g upload button along with search bar.
        public bool showButton { get; set; } = false;
        public string ButtonText { get; set; } = "";
        public string ButtonUrl { get; set; } = "";
        public string ButtonIcon { get; set; } = "";
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
