using Jugnoon.Models;
using Jugnoon.Utility;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jugnoon.Meta
{
    public class Sitemap
    {
        public static List<BreadItem> processSitmap(PageQuery query)
        {
            var _breadItems = new List<BreadItem>
            {
                new BreadItem { url = SiteConfiguration.URL, title = SiteConfig.generalLocalizer["_home"].Value }
            };
            var pages = new List<SitemapMeta>();
            switch (query.controller)
            {
                case "videos":
                    pages = Videos.Sitemap.Prepare();
                    break;
            }

            var processed_items = processItems(pages, query);
            foreach(var item in processed_items)
            {
                item.title = processData(item.title, query);
                item.url = processData(item.url, query);
            }
            foreach (var item in processed_items)
            {
                _breadItems.Add(item);
            }
            if (query.pagenumber > 1)
                _breadItems.Add(new BreadItem { isActive = true, title = SiteConfig.generalLocalizer["_page"] + " " + query.pagenumber });

            return _breadItems;
        }

        public static string processData(string data, PageQuery query)
        {
            if (data != null && data != "")
            {
                if (query.matchterm != "")
                    data = Regex.Replace(data, "\\[MAT1\\]", query.matchterm);

                if (query.matchterm2 != "")
                    data = Regex.Replace(data, "\\[MAT2\\]", query.matchterm2);

            }
            return data;
        }
        private static List<BreadItem> processItems(List<SitemapMeta> pages, PageQuery query)
        {
            var items = new List<BreadItem>();
            foreach (var page in pages)
            {
                if (page.index == query.index)
                {
                    items.Add(new BreadItem() { url = page.url, title = page.title });
                    if (page.child != null)
                    {
                        var child_items = processItems(page.child, query);
                        if (child_items.Count > 0)
                        {
                            foreach(var item in child_items) {
                                items.Add(item);
                            }
                        }
                    }
                }
                else if (page.order == query.order && query.order != "")
                {
                    items.Add(new BreadItem() { url = page.url, title = page.title });
                    if (page.child != null)
                    {
                        var child_items = processItems(page.child, query);
                        if (child_items.Count > 0)
                        {
                            foreach (var item in child_items)
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
                else if (page.date == query.filter && query.filter != "")
                {
                    items.Add(new BreadItem() { url = page.url, title = page.title });
                    if (page.child != null)
                    {
                        var child_items = processItems(page.child, query);
                        if (child_items.Count > 0)
                        {
                            foreach (var item in child_items)
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            return items;
        }

    
    }

    public class MetaUrl
    {
        public string title { get; set; }
        public string url { get; set; }
        public string p_url { get; set; }
    }
    public class SitemapMeta : MetaUrl
    {
        // query params
        public string controller { get; set; }
        public string index { get; set; }
        public string order { get; set; }
        public string date { get; set; }
        
        public List<SitemapMeta> child { get; set; }
    }
}
/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
