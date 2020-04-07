using Jugnoon.Models;
using System.Collections.Generic;


namespace Jugnoon.Meta
{
    /// <summary>
    /// This class separates page meta titles in single location that you can be easily customize or manage
    /// </summary>
    public class PageMeta
    {
        /// <summary>
        /// Core function for handling page meta for all dynamic pages within application
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Meta returnPageMeta(PageQuery query)
        {
            query.controller = query.controller.ToLower();
            query.index = query.index.ToLower();

            var Pages = new List<Page>();
            switch(query.controller)
            {               
                case "videos":
                    Pages = Videos.Meta.Prepare();
                    break;
            }

            var _Meta = processMeta(Pages, query);
           
            _Meta.title = processTitle(_Meta.title, query);
            _Meta.description = processTitle(_Meta.description, query);
            _Meta.keywords = processTitle(_Meta.keywords, query);
            _Meta.imageurl = processTitle(_Meta.imageurl, query);
            _Meta.BreadItems = Sitemap.processSitmap(query);

            return _Meta;
        }

        private static Meta processMeta(List<Page> pages, PageQuery query)
        {
            var meta = new Meta();
            foreach (var page in pages)
            {
                if (page.index == query.index)
                {
                    if (page.date == query.filter && query.filter != "")
                    {
                        if (page.child != null)
                        {
                            return processMeta(page.child, query);
                        }
                        else
                        {
                            meta.title = page.title;
                            meta.description = page.description;
                            meta.keywords = page.keywords;
                            meta.imageurl = page.imageurl;

                            return meta;
                        }
                    }
                    else if (page.order == query.order && query.order != "")
                    {
                        if (page.child != null && query.filter != "")
                        {
                            return processMeta(page.child, query);
                        }
                        else
                        {
                            meta.title = page.title;
                            meta.description = page.description;
                            meta.keywords = page.keywords;
                            meta.imageurl = page.imageurl;
                            return meta;
                        }
                    }
                    else if (page.order == null && page.date == null)
                    {
                        if (page.child != null)
                        {
                            return processMeta(page.child, query);
                        }
                        else
                        {
                            meta.title = page.title;
                            meta.description = page.description;
                            meta.keywords = page.keywords;
                            meta.imageurl = page.imageurl;
                            return meta;
                        }
                    }
                }

                /*else if (page.child != null)
                {
                    return processMeta(page.child, query);
                }
                else
                {
                    meta.title = page.title;
                    meta.description = page.description;
                    meta.keywords = page.keywords;
                    meta.imageurl = page.imageurl;
                    return meta;
                }*/
            }
            return meta;
        }


        public static string processTitle(string title, PageQuery query)
        {
            if (title != null && title != "")
            {
                if (query.matchterm != "")
                    title = title.Replace("{MAT1}", query.matchterm.ToLower());

                if (query.matchterm2 != "")
                    title = title.Replace("{MAT2}", query.matchterm2.ToLower());

                if (query.pagenumber > 1)
                    title = title.Replace("{PG}", ", page " + query.pagenumber);
                else
                    title = title.Replace("{PG}", "");

                if (query.type != "")
                    title = title.Replace("{TP}", query.type);
            }
            return title;
        }


    }

    public class Meta
    {
        public string title { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string imageurl { get; set; }
        public List<BreadItem> BreadItems { get; set; }
    }

    public class PageQuery
    {
        public string controller { get; set; } = "";
        public string index { get; set; } = "";
        public string order { get; set; } = "";
        public string type { get; set; } = "";
        public string filter { get; set; } = "";
        public string matchterm { get; set; } = "";
        public string matchterm2 { get; set; } = "";

        public int pagenumber { get; set; } = 1;
    }

    public class Page : Meta
    {
        // query params
        public string controller { get; set; }
        public string index { get; set; }
        public string order { get; set; }
        public string date { get; set; }

        /// <summary>
        /// Specifically used for static pages mapping within controller / index
        /// </summary>
        public string pagename { get; set; }
        /// <summary>
        /// Name of view used for mapping custom view within controller for root / static pages
        /// </summary>
        public string viewname { get; set; }
        /// <summary>
        /// Check whether current custom view / page / have additional style used, if yes it will map partial page to load styles
        /// </summary>
        public bool style_exists { get; set; }
        /// <summary>
        /// Check whether current custom view / page / have additional style used, if yes it will map partial page to load scripts
        /// </summary>
        public bool script_exists { get; set; }

        public List<Page> child { get; set;}
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
