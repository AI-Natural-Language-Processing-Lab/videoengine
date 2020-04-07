using System.Collections.Generic;

namespace Jugnoon.Utility.Helper
{
    public class IPagination
    {
        public int id { get; set; } = 0;
        public string css { get; set; } = "";
        public string url { get; set; } = "";
        public string tooltip { get; set; } = "";
        public string icon { get; set; } = "";

    }
    public class Pagination
    {
        public static List<IPagination> PrepareLinks(
            int TotalPages,
            string RootUrl,
            int PageNumber,
            int PageSize,
            int TotalRecords,
            bool isFilter,
            string FilterUrl,
            string DefaultUrl,
            string FilterPaginationUrl,
            string DefaultPaginationUrl,
            PaginationUtil.Types type)
        {
            var _list = new List<IPagination>();

            int firstbound = 0;
            int lastbound = 0;
            string ToolTip = "";
            var Links = PaginationUtil.preparePagination(TotalPages, 7, PageNumber, type);
            if (Links.Count > 0)
            {
                int i = 0;
                string LinkURL = "";
                foreach (int Item in Links)
                {
                    firstbound = ((Item - 1) * PageSize) + 1;
                    lastbound = firstbound + PageSize - 1;
                    if (lastbound > TotalRecords)
                    {
                        lastbound = TotalRecords;
                    }

                    ToolTip = "Showing " + firstbound + " - " + lastbound + " records of " + TotalRecords + " records";
                    // url settings
                    // normal search
                    if (Item == 1)
                    {
                        if (isFilter)
                        {
                            LinkURL = FilterUrl;
                        }
                        else
                        {
                            LinkURL = DefaultUrl;
                        }
                    }
                    else
                    {
                        if (isFilter)
                        {
                            LinkURL = UtilityBLL.Add_pagenumber(FilterPaginationUrl, Item.ToString());
                        }
                        else
                        {
                            LinkURL = UtilityBLL.Add_pagenumber(DefaultPaginationUrl, Item.ToString());
                        }
                    }
                    string _css = "";
                    if (Item == PageNumber)
                    {
                        _css = "active";
                    }
                    _list.Add(new IPagination()
                    {
                        css = _css,
                        id = Item,
                        url = LinkURL,
                        tooltip = ToolTip
                    });
                }
            }
            return _list;
        }

        public static List<IPagination> LastLinks(
            int TotalPages,
            string RootUrl,
            int PageNumber,
            int PageSize,
            int TotalRecords,
            bool isFilter,
            string FilterUrl,
            string DefaultUrl,
            string FilterPaginationUrl,
            string DefaultPaginationUrl,
            bool ShowLast)
        {
            var _list = new List<IPagination>();

            string LastNavigationUrl = "";
            string NextNavigationUrl = "";
            int _nextpage = PageNumber + 1;

            if (isFilter)
            {
                LastNavigationUrl = UtilityBLL.Add_pagenumber(FilterPaginationUrl, TotalPages.ToString());
                NextNavigationUrl = UtilityBLL.Add_pagenumber(FilterPaginationUrl, _nextpage.ToString());
            }
            else
            {
                LastNavigationUrl = UtilityBLL.Add_pagenumber(DefaultPaginationUrl, TotalPages.ToString());
                NextNavigationUrl = UtilityBLL.Add_pagenumber(DefaultPaginationUrl, _nextpage.ToString());
            }

            int firstbound = ((TotalPages - 1) * PageSize) + 1;
            int lastbound = firstbound + PageSize - 1;
            if (lastbound > TotalRecords)
            {
                lastbound = TotalRecords;
            }
            string ToolTip = "Showing " + firstbound + " - " + lastbound + " records of " + TotalRecords + " records";
            // Next Link
            int pid = (PageNumber + 1);
            if (pid > TotalPages)
            {
                pid = TotalPages;
            }

            _list.Add(new IPagination()
            {
                css = "",
                id = pid,
                url = NextNavigationUrl,
                tooltip = ToolTip,
                icon = "fa fa-angle-right"
            });

            // Last Link
            if (ShowLast)
            {
                ToolTip = "Showing " + firstbound + " - " + lastbound + " records of " + TotalRecords + " records";
                _list.Add(new IPagination()
                {
                    css = "",
                    id = TotalPages,
                    url = LastNavigationUrl,
                    tooltip = ToolTip,
                    icon = "fa fa-forward"
                });
            }
            return _list;
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
