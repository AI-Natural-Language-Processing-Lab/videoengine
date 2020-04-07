namespace Jugnoon.Models
{
    public class PaginationViewModel
    {
        public int pagenumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalRecords { get; set; } = 0;
        public string Default_Url { get; set; } = "";
        public string Pagination_Url { get; set; } = "";
        public bool isFilter { get; set; } = false;
        public string Filter_Default_Url { get; set; } = "";
        public string Filter_Pagination_url { get; set; } = "";
        public bool ShowFirst { get; set; } = true;
        public bool ShowLast { get; set; } = true;
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
