using Jugnoon.Meta;
using Jugnoon.Utility;
using System.Collections.Generic;

namespace Jugnoon.Videos
{
    public class Sitemap
    {
        /// <summary>
        /// Prepare core sitemap for videos urls
        /// </summary>
        /// <returns></returns>
        public static List<SitemapMeta> Prepare()
        {
            var url = SiteConfiguration.URL;
            var Pages = new List<SitemapMeta>
            {
                 new SitemapMeta {
                     controller = "videos",
                     index = "index",
                     title = SiteConfig.videoLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             order = "recent",
                             title = SiteConfig.generalLocalizer["_recent"].Value,
                             url = url + "videos/",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     date = "today",
                                     title = SiteConfig.generalLocalizer["_today"].Value,
                                     url = url + "videos/recent/today"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thisweek",
                                     title = SiteConfig.generalLocalizer["_this_week"].Value,
                                     url = url + "videos/added/thisweek"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thismonth",
                                     title = SiteConfig.generalLocalizer["_this_month"].Value,
                                     url = url + "videos/added/thismonth"
                                 }
                             }
                         },
                         new SitemapMeta
                         {
                             order = "mostviewed",
                             title = SiteConfig.generalLocalizer["_most_viewed"].Value,
                             url = url + "videos/mostviewed",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     date = "today",
                                     title = SiteConfig.generalLocalizer["_today"].Value,
                                     url = url + "videos/mostviewed/today"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thisweek",
                                     title = SiteConfig.generalLocalizer["_this_week"].Value,
                                     url = url + "videos/mostviewed/thisweek"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thismonth",
                                     title = SiteConfig.generalLocalizer["_this_month"].Value,
                                     url = url + "videos/mostviewed/thismonth"
                                 }
                             }
                         },
                         new SitemapMeta
                         {
                             order = "toprated",
                             title = SiteConfig.generalLocalizer["_top_rated"].Value,
                             url = url + "videos/toprated",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     date = "today",
                                     title = SiteConfig.generalLocalizer["_today"].Value,
                                     url = url + "videos/toprated/today"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thisweek",
                                     title = SiteConfig.generalLocalizer["_this_week"].Value,
                                     url = url + "videos/toprated/thisweek"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thismonth",
                                     title = SiteConfig.generalLocalizer["_this_month"].Value,
                                     url = url + "videos/toprated/thismonth"
                                 }
                             }
                         },
                         new SitemapMeta
                         {
                             order = "featured",
                             title = SiteConfig.generalLocalizer["_featured"].Value,
                             url = url + "videos/featured",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     date = "today",
                                     title = SiteConfig.generalLocalizer["_today"].Value,
                                     url = url + "videos/featured/today"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thisweek",
                                     title = SiteConfig.generalLocalizer["_this_week"].Value,
                                     url = url + "videos/featured/thisweek"
                                 },
                                 new SitemapMeta
                                 {
                                     date = "thismonth",
                                     title = SiteConfig.generalLocalizer["_this_month"].Value,
                                     url = url + "videos/featured/thismonth"
                                 }
                             }
                         },
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "category",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             index = "category",
                             title = SiteConfig.generalLocalizer["_categories"].Value,
                             url = url + "videos/categories",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     order = "recent",
                                     title = SiteConfig.generalLocalizer["_recent"].Value,
                                     url = url + "videos/category/[MAT1]",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/category/filter/[MAT1]/today",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thisweek",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thismonth",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "mostviewed",
                                     title = SiteConfig.generalLocalizer["_most_viewed"].Value,
                                     url = url + "videos/category/[MAT1]/mostviewed",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/category/filter/[MAT1]/today/mostviewed",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thisweek/mostviewed",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thismonth/mostviewed",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "toprated",
                                     title = SiteConfig.generalLocalizer["_top_rated"].Value,
                                     url = url + "videos/category/[MAT1]/toprated",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/category/filter/[MAT1]/today/toprated",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thisweek/toprated",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thismonth/toprated",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "featured",
                                     title = SiteConfig.generalLocalizer["_featured"].Value,
                                     url = url + "videos/category/[MAT1]/featured",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/category/filter/[MAT1]/today/featured",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thisweek/featured",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/category/filter/[MAT1]/thismonth/featured",
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "label",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                          new SitemapMeta
                         {
                             index = "label",
                             title = SiteConfig.generalLocalizer["_labels"].Value,
                             url = url + "videos/labels",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     order = "recent",
                                     title = SiteConfig.generalLocalizer["_recent"].Value,
                                     url = url + "videos/label/[MAT1]",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/label/filter/[MAT1]/today",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thisweek",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thismonth",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "mostviewed",
                                     title = SiteConfig.generalLocalizer["_most_viewed"].Value,
                                     url = url + "videos/label/[MAT1]/mostviewed",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/label/filter/[MAT1]/today/mostviewed",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thisweek/mostviewed",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thismonth/mostviewed",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "toprated",
                                     title = SiteConfig.generalLocalizer["_top_rated"].Value,
                                     url = url + "videos/label/[MAT1]/toprated",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/label/filter/[MAT1]/today/toprated",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thisweek/toprated",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thismonth/toprated",
                                         }
                                     }
                                 },
                                 new SitemapMeta
                                 {
                                     order = "featured",
                                     title = SiteConfig.generalLocalizer["_featured"].Value,
                                     url = url + "videos/label/[MAT1]/featured",
                                     child = new List<SitemapMeta>
                                     {
                                         new SitemapMeta
                                         {
                                             date = "today",
                                             title = SiteConfig.generalLocalizer["_today"].Value,
                                             url = url + "videos/label/filter/[MAT1]/today/featured",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thisweek",
                                             title = SiteConfig.generalLocalizer["_this_week"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thisweek/featured",
                                         },
                                         new SitemapMeta
                                         {
                                             date = "thismonth",
                                             title = SiteConfig.generalLocalizer["_this_month"].Value,
                                             url = url + "videos/label/filter/[MAT1]/thismonth/featured",
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "archive",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             index = "archive",
                             title = SiteConfig.generalLocalizer["_archive_list"].Value,
                             url = url + "videos/archivelist",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     index = "Archive",
                                     title = "[MAT1] [MAT2]",
                                     url = url + "videos/archive/[MAT1]/[MAT2]"
                                 }
                             }
                         }
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "categories",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             index = "categories",
                             title = SiteConfig.generalLocalizer["_categories"].Value,
                             url = url + "videos/categories"
                         }
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "archivelist",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             index = "archivelist",
                             title = SiteConfig.generalLocalizer["_archive_list"].Value,
                             url = url + "videos/archivelist"
                         }
                     }
                 },
                 new SitemapMeta
                 {
                     controller = "videos",
                     index = "labels",
                     title = SiteConfig.generalLocalizer["_videos"].Value,
                     url = url + "videos/",
                     child = new List<SitemapMeta>
                     {
                         new SitemapMeta
                         {
                             index = "labels",
                             title = SiteConfig.generalLocalizer["_labels"].Value,
                             url = url + "videos/labels",
                             child = new List<SitemapMeta>
                             {
                                 new SitemapMeta
                                 {
                                     index = "labels",
                                     order = "search",
                                     title = "[MAT1]",
                                     url = url + "videos/labels/search/[MAT1]"
                                 }
                             }
                         }
                     }
                 }
             };

            return Pages;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
