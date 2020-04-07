using Jugnoon.Scripts;
using System.Collections.Generic;

namespace Jugnoon.Models
{
    public class ListViewModel
    {
        public string HeadingTitle { get; set; } = "";
        public bool isListStatus { get; set; } = true;
        public bool isListNav { get; set; } = false;
        public string NoRecordFoundText { get; set; } = "";
        public int PageSize { get; set; } = 20;
        public string DefaultUrl { get; set; } = "";
        public string PaginationUrl { get; set; } = "";
        public string BrowseText { get; set; } = "";
        public string BrowseUrl { get; set; } = "";
        public ListItems ListObject { get; set; } = new ListItems();
        public List<BreadItem> BreadItems { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
