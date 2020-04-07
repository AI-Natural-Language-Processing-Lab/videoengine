using Jugnoon.Meta;
using Jugnoon.Utility;
using System.Collections.Generic;

namespace Jugnoon.Videos
{
    public class Meta
    {

        /// <summary>
        /// It is responsible for managing videos controller both static and dynamic pages meta data 
        /// </summary>
        /// <returns></returns>
        public static List<Page> Prepare()
        {
            var Pages = new List<Page>
            {
                // video controller
                new Page {
                    controller = "videos",
                    index = "index",
                    child = new List<Page>
                    {
                        new Page {
                            order = "recent",
                            index = "index",
                            title = SiteConfig.videoLocalizer["_meta_recently_added_videos"].Value + "{PG}",
                            description = "",
                            imageurl = "",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_recently_added_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_recently_added_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_recently_added_videos_month"].Value + "{PG}"
                                }
                            }
                        },
                        new Page {
                            order = "mostviewed",
                            index = "index",
                            title = SiteConfig.videoLocalizer["_meta_viewed_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_viewed_videos_today"].Value + "{PG}",
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_viewed_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_viewed_videos_month"].Value + "{PG}"
                                },
                            }
                        },
                        new Page {
                            order = "toprated",
                            index = "index",
                            title = SiteConfig.videoLocalizer["_meta_top_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_top_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_top_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_top_videos_month"].Value + "{PG}"
                                },
                            }
                        },
                        new Page {
                            order = "featured",
                            index = "index",
                            title = SiteConfig.videoLocalizer["_meta_featured_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_featured_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_featured_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "index",
                                    title = SiteConfig.videoLocalizer["_meta_featured_videos_month"].Value + "{PG}"
                                },
                            }
                        }
                    }
                },
                // Category Pages Adjustments
                new Page {
                    controller = "videos",
                    index = "category",
                    child = new List<Page>
                    {
                        new Page {
                            order = "recent",
                            index = "category",
                            title = SiteConfig.videoLocalizer["_meta_category_recent_videos"].Value + "{PG}",
                            description = "",
                            imageurl = "",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_recent_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_recent_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_recent_videos_month"].Value + "{PG}"
                                },
                            }
                        },
                        new Page {
                            order = "mostviewed",
                            index = "category",
                            title = SiteConfig.videoLocalizer["_meta_category_viewed_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_viewed_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_viewed_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_viewed_videos_month"].Value + "{PG}"
                                },
                            }
                        },
                        new Page {
                            order = "toprated",
                            index = "category",
                            title = SiteConfig.videoLocalizer["{MAT1} - Top Rated Videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_toprated_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thisweek",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_toprated_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_toprated_videos_month"].Value + "{PG}"
                                },
                            }
                        },
                        new Page {
                            order = "featured",
                            index = "category",
                            title = SiteConfig.videoLocalizer["_meta_category_featured_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page {
                                    date = "today",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_featured_videos_today"].Value + "{PG}"
                                },
                                new Page {
                                    index = "category",
                                    date = "thisweek",
                                    title = SiteConfig.videoLocalizer["_meta_category_featured_videos_week"].Value + "{PG}"
                                },
                                new Page {
                                    date = "thismonth",
                                    index = "category",
                                    title = SiteConfig.videoLocalizer["_meta_category_featured_videos_month"].Value + "{PG}"
                                },
                            }
                        }
                    }
                },
                // Tags or Label Pages Adjustments
                new Page {
                    controller = "videos",
                    index = "label",
                    child = new List<Page>
                    {
                        new Page {
                            order = "recent",
                            index = "label",
                            title = SiteConfig.videoLocalizer["_meta_label_recent_videos"].Value + "{PG}",
                            description = "",
                            imageurl = "",
                            child = new List<Page>
                            {
                                new Page { date = "today", index = "label", title = SiteConfig.videoLocalizer["_meta_label_recent_videos_today"].Value + "{PG}" },
                                new Page { date = "thisweek", index = "label", title = SiteConfig.videoLocalizer["_meta_label_recent_videos_week"].Value + "{PG}" },
                                new Page { date = "thismonth", index = "label", title = SiteConfig.videoLocalizer["_meta_label_recent_videos_month"].Value + "{PG}" }
                            }
                        },
                        new Page {
                            order = "mostviewed",
                            index = "label",
                            title = SiteConfig.videoLocalizer["_meta_label_mostviewed_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page { date = "today", index = "label", title = SiteConfig.videoLocalizer["_meta_label_mostviewed_videos_today"].Value + "{PG}" },
                                new Page { date = "thisweek", index = "label", title = SiteConfig.videoLocalizer["_meta_label_mostviewed_videos_week"].Value + "{PG}" },
                                new Page { date = "thismonth", index = "label", title = SiteConfig.videoLocalizer["_meta_label_mostviewed_videos_month"].Value + "{PG}" },
                            }
                        },
                        new Page {
                            order = "toprated",
                            index = "label",
                            title = SiteConfig.videoLocalizer["_meta_label_toprated_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page { date = "today",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_toprated_videos_today"].Value  + "{PG}"},
                                new Page { date = "thisweek",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_toprated_videos_week"].Value  + "{PG}"},
                                new Page { date = "thismonth",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_toprated_videos_month"].Value  + "{PG}"},
                            }
                        },
                        new Page {
                            order = "featured",
                             index = "label",
                            title = SiteConfig.videoLocalizer["_meta_label_featured_videos"].Value + "{PG}",
                            child = new List<Page>
                            {
                                new Page { date = "today",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_featured_videos_today"].Value  + "{PG}"},
                                new Page { date = "thisweek",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_featured_videos_week"].Value  + "{PG}"},
                                new Page { date = "thismonth",  index = "label", title = SiteConfig.videoLocalizer["_meta_label_featured_videos_month"].Value  + "{PG}"},
                            }
                        }
                    }
                },
                new Page {
                    controller = "videos",
                    index = "archive",
                    title = SiteConfig.videoLocalizer["_meta_archive_videos"].Value + "{PG}",
                    description = "",
                    imageurl = ""
                },
                new Page {
                    controller = "videos",
                    index = "categories",
                    title = SiteConfig.videoLocalizer["_meta_video_categories"].Value + "{PG}",
                    description = "",
                    imageurl = ""
                },
                new Page {
                    controller = "videos",
                    index = "archivelist",
                    title = SiteConfig.videoLocalizer["_meta_video_archive_list"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    controller = "videos",
                    index = "labels",
                    child = new List<Page>
                    {
                        new Page { order = "normal", title = SiteConfig.videoLocalizer["_meta_video_label_list"].Value + "{PG}" },
                        // labels with search term (search label functionality)
                        new Page { order = "search", title = SiteConfig.videoLocalizer["_meta_video_label_list_search"].Value + "{PG}" }
                    }
                },
                
                new Page {
                    controller = "videos",
                    index = "search",
                    title = SiteConfig.videoLocalizer["_meta_video_search"].Value + "{PG}"
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
