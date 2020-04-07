using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.BLL;
using System.Text;
using Jugnoon.Settings;
using Jugnoon.Scripts;
using System.IO;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Jugnoon.Models;
using VideoEngine.Models;
using System.Threading.Tasks;
using Jugnoon.Videos;
using Jugnoon.Videos.Models;
using Jugnoon.Meta;
using Jugnoon.Localize;

namespace VideoEngine.Controllers
{
    public class videosController : Controller
    {
        ApplicationDbContext _context; 
        public videosController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
           IStringLocalizer<GeneralResource> generalLocalizer,
           IStringLocalizer<VideoResource> videoLocalizer,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           IOptions<General> generalSettings,
           IOptions<Media> mediaSettings,
           IOptions<Features> featureSettings,
           IOptions<Smtp> smtpSettings,
           IOptions<Jugnoon.Videos.Settings.General> generalVideoSettings
           )
        {
            // database context
            _context = context;

            // general settings
            Jugnoon.Settings.Configs.GeneralSettings = generalSettings.Value;
            Jugnoon.Settings.Configs.FeatureSettings = featureSettings.Value;
            Jugnoon.Settings.Configs.SmtpSettings = smtpSettings.Value;
            Jugnoon.Settings.Configs.MediaSettings = mediaSettings.Value;
            // video settings specific
            Jugnoon.Videos.Configs.GeneralSettings = generalVideoSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            _context = context;

            SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.videoLocalizer = videoLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        #region Feed
        public async Task<IActionResult> atom()
        {
            string sXml = await VideoFeeds.generateATOM(_context, new VideoEntity()
            {
                type = MediaType.Videos,
                pagenumber = 1,
                ispublic = true,
                order = "video.created_at desc",
                pagesize = 10
            }, Config.GetUrl("vidoes/"));

            return this.Content(sXml, "text/xml");
        }

        public async Task<IActionResult> rss()
        {
            string sXml = await VideoFeeds.generateRSS(_context, new VideoEntity()
            {
                type = MediaType.Videos,
                pagenumber = 1,
                ispublic = true,
                order = "video.created_at desc",
                pagesize = 10
            });

            return this.Content(sXml, "text/xml");
        }


        public async Task<IActionResult> videos(int? type, int? page, int? rt)
        {
            int tp = 0;
            // Tags Type:
            // ........... 0: Most Viewed
            // ........... 1: Top Rated
            // ........... 2: Recently Added
            if (type != null)
                tp = (int)type;

            int pagenumber = 1;
            if (page != null)
                pagenumber = (int)page;
            string sXml = "";
            int responsetype = 0; // 0: google, 1: bing
            if (rt != null)
                responsetype = (int)rt;
            switch (responsetype)
            {
                case 0:
                    sXml = await VideoFeeds.generateGoogleSitemap(_context, new VideoEntity()
                    {
                        type = (MediaType)tp,
                        pagenumber = pagenumber,
                        pagesize = 50000,
                        order = "video.created_at desc"
                    });
                    break;
                case 1:
                    sXml = await VideoFeeds.GenerateBingSitemap(_context, new VideoEntity()
                    {
                        type = (MediaType)tp,
                        pagenumber = pagenumber,
                        pagesize = 50000,
                        order = "video.created_at desc"
                    });
                    break;
            }

            return this.Content(sXml, "text/xml");
        }

        #endregion

        #region Public Region

        // GET: videos
        public async Task<IActionResult> Index(string order, string filter, int? pagenumber)
        {
            if (pagenumber == null)
                pagenumber = 1;

            string _order = "video.created_at desc";
            var _dateFilter = DateFilter.AllTime;
            var _featuredFilter = FeaturedTypes.All;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var pageOrder = "recent";
            var pageFilter = "";
            if (order != null)
                pageOrder = order;
            if (filter != null)
                pageFilter = filter;
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                order = pageOrder,
                filter = pageFilter,
                pagenumber = (int)pagenumber
            });
            
            // pagination
            var DefaultUrl = Config.GetUrl("videos/");
            var PaginationUrl = Config.GetUrl("videos/page/[p]/");

            // order
            var selectedOrder = SiteConfig.generalLocalizer["_recent"].Value;
            var selectedFilter  = SiteConfig.generalLocalizer["_all_time"].Value;
            

            if (order != null)
            {                
                DefaultUrl = Config.GetUrl("videos/" + order.ToLower().Trim());
                PaginationUrl = Config.GetUrl("videos/" + order.ToLower().Trim() + "/[p]/");
                switch (order)
                {
                    case "mostviewed":
                        selectedOrder = SiteConfig.generalLocalizer["_most_viewed"].Value;
                        _order = "video.views desc, video.created_at desc";
                        break;
                    case "toprated":
                        selectedOrder   = SiteConfig.generalLocalizer["_top_rated"].Value;
                        _order = "video.avg_rating desc, video.views desc";
                        break;
                    case "featured":
                        selectedOrder   = SiteConfig.generalLocalizer["_featured"].Value;
                        _order = "video.created_at desc";
                        _featuredFilter = FeaturedTypes.Featured;
                        break;
                }
            }
            
            if (filter != null)
            {
                // pagination setting
                if (filter == "today" || filter == "thisweek" || filter == "thismonth")
                {
                    DefaultUrl = Config.GetUrl("videos/added/" + filter.ToLower().Trim());
                    PaginationUrl = Config.GetUrl("videos/added/" + filter.ToLower().Trim() + "/[p]/");
                }

                switch (filter)
                {                   
                    case "today":
                        selectedFilter = SiteConfig.generalLocalizer["_today"].Value;
                        _dateFilter = DateFilter.Today;
                        break;
                    case "thisweek":
                        selectedFilter = SiteConfig.generalLocalizer["_this_week"].Value;
                        _dateFilter = DateFilter.ThisWeek;
                        break;
                    case "thismonth":
                        selectedFilter = SiteConfig.generalLocalizer["_this_month"].Value;
                        _dateFilter = DateFilter.ThisMonth;
                        break;
                }
            }
           
            /* List Initialization */
            var ListEntity = new VideoListViewModel()
            {
                isListStatus = true,
                isListNav = true,
                // setup list navigation urls
                Navigation = prepareFilterLinks("/videos/", selectedOrder, selectedFilter),
                QueryOptions = new VideoEntity()
                {
                    type = MediaType.Videos,
                    pagenumber = (int)pagenumber,
                    term = "",
                    iscache = true,
                    ispublic = true,
                    isfeatured = _featuredFilter, 
                    pagesize = Jugnoon.Settings.Configs.GeneralSettings.pagesize,
                    datefilter = _dateFilter,
                    order = _order,
                },
                ListObject = new ListItems()
                {
                    TitleLength = 40,
                    ColWidth = "col-md-4",
                    ListType = ListType.Grid,
                    showRating = true,
                    showViews = true,
                    showDate = true
                },
                DefaultUrl = DefaultUrl,
                PaginationUrl = PaginationUrl,
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                HeadingTitle = _meta.title,
                BreadItems = _meta.BreadItems
            };
            
            ListEntity.TotalRecords = await VideoBLL.Count(_context, ListEntity.QueryOptions);
            if(ListEntity.TotalRecords > 0)
                ListEntity.DataList = await VideoBLL.LoadItems(_context, ListEntity.QueryOptions);
            
            // Page Meta Description
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;
            ViewBag.keywords = _meta.keywords;
            ViewBag.imageurl = _meta.imageurl;

            return View(ListEntity);
        }


        // GET: videos/category
        public async Task<IActionResult> category(string title, string order, string filter, int? pagenumber)
        {
            if (title == null)
                return Redirect(Config.GetUrl("videos/"));

            if (pagenumber == null)
                pagenumber = 1;
            
            string _term = UtilityBLL.ReplaceHyphinWithSpace(title);
            string categoryName = UtilityBLL.UppercaseFirst(_term);

            string _order = "video.created_at desc";
            var _dateFilter = DateFilter.AllTime;
            var _featuredFilter = FeaturedTypes.All;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var pageOrder = "recent";
            var pageFilter = "";
            if (order != null)
                pageOrder = order;
            if (filter != null)
                pageFilter = filter;
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                order = pageOrder,
                filter = pageFilter,
                pagenumber = (int)pagenumber,
                matchterm = categoryName
            });

            /**********************************************/
            // Prepare Pagination
            /**********************************************/
            var DefaultUrl = Config.GetUrl("videos/category/" + title);
            var PaginationUrl = Config.GetUrl("videos/category/" + title + "/[p]/");

            // order
            var selectedOrder = SiteConfig.generalLocalizer["_recent"].Value;
            var selectedFilter  = SiteConfig.generalLocalizer["_all_time"].Value;

            if (order != null)
            {
                DefaultUrl = Config.GetUrl("videos/category/" + title + "/" + order.ToLower().Trim());
                PaginationUrl = Config.GetUrl("videos/category/" + title + "/" + order.ToLower().Trim() + "/[p]/");

                switch (order)
                {
                    case "mostviewed":
                        selectedOrder   = SiteConfig.generalLocalizer["_most_viewed"].Value;
                        _order = "video.views desc, video.created_at desc";
                        break;
                    case "toprated":
                        selectedOrder   = SiteConfig.generalLocalizer["_top_rated"].Value;
                        _order = "video.avg_rating desc, video.views desc";
                        break;
                    case "featured":
                        selectedOrder   = SiteConfig.generalLocalizer["_featured"].Value;
                        _order = "video.created_at desc";
                        _featuredFilter = FeaturedTypes.Featured;
                        break;
                }
            }
            if (filter != null)
            {
                // pagination setting
                if (filter == "today" || filter == "thisweek" || filter == "thismonth")
                {
                    if (order != null)
                    {
                        DefaultUrl = Config.GetUrl("videos/category/filter/" + title + "/" + filter.ToLower().Trim() + "/" + order);
                        PaginationUrl = Config.GetUrl("videos/category/filter/" + title + "/" + filter.ToLower().Trim() + "/" + order + "/[p]/");
                    }
                    else
                    {
                        DefaultUrl = Config.GetUrl("videos/category/filter/" + title + "/" + filter.ToLower().Trim());
                        PaginationUrl = Config.GetUrl("videos/category/filter/" + title + "/" + filter.ToLower().Trim() + "/[p]/");
                    }
                }

                switch (filter)
                {
                    case "today":
                        selectedFilter = SiteConfig.generalLocalizer["_today"].Value;
                        _dateFilter = DateFilter.Today;
                        break;
                    case "thisweek":
                        selectedFilter = SiteConfig.generalLocalizer["_this_week"].Value;
                         _dateFilter = DateFilter.ThisWeek;
                        break;
                    case "thismonth":
                        selectedFilter = SiteConfig.generalLocalizer["_this_month"].Value;
                         _dateFilter = DateFilter.ThisMonth;
                        break;
                }
            }

            /* List Initialization */
            var ListEntity = new VideoListViewModel()
            {
                isListStatus = true,
                isListNav = true,
                Navigation = prepareCategoryFilterLinks("/videos/category/", title, selectedOrder, selectedFilter),

                QueryOptions = new VideoEntity()
                {
                    type = MediaType.Videos,
                    pagenumber = (int)pagenumber,
                    categoryname = _term,
                    iscache = false,
                    term = "",
                    ispublic = true,
                    isfeatured = _featuredFilter,
                    pagesize = Jugnoon.Settings.Configs.GeneralSettings.pagesize,
                    datefilter = _dateFilter,
                    order = _order,
                },
                ListObject = new ListItems()
                {
                    TitleLength = 40,
                    ColWidth = "col-md-4",
                    ListType = ListType.Grid,
                    showRating = true,
                    showViews = true,
                    showDate = true
                },
                HeadingTitle = _meta.title,
                DefaultUrl = DefaultUrl,
                PaginationUrl = PaginationUrl,
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                BreadItems = _meta.BreadItems
            };

            ListEntity.TotalRecords = await CategorizeVideos.Count(_context, ListEntity.QueryOptions);
            if (ListEntity.TotalRecords > 0)
                ListEntity.DataList = await CategorizeVideos.LoadItems(_context, ListEntity.QueryOptions);

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;
            ViewBag.keywords = _meta.keywords;
            ViewBag.imageurl = _meta.imageurl;

            return View(ListEntity);
        }

        // GET: videos/tag
        public async Task<IActionResult> label(string title, string order, string filter, int? pagenumber)
        {
            if (title == null)
            {
                return Redirect(Config.GetUrl("videos/"));
            }
            if (pagenumber == null)
                pagenumber = 1;

            string _term = UtilityBLL.ReplaceHyphinWithSpace(title);
            string categoryName = UtilityBLL.UppercaseFirst(_term);

            string _order = "video.created_at desc";
            var _dateFilter = DateFilter.AllTime;
            var _featuredFilter = FeaturedTypes.All;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var pageOrder = "recent";
            var pageFilter = "";
            if (order != null)
                pageOrder = order;
            if (filter != null)
                pageFilter = filter;
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                order = pageOrder,
                filter = pageFilter,
                pagenumber = (int)pagenumber,
                matchterm = categoryName
            });

            // pagination
            var DefaultUrl = Config.GetUrl("videos/label/" + title);
            var PaginationUrl = Config.GetUrl("videos/label/" + title + "/[p]/");

            // order
            var selectedOrder = SiteConfig.generalLocalizer["_recent"].Value;
            var selectedFilter  = SiteConfig.generalLocalizer["_all_time"].Value;
                      
            if (order != null)
            {
                DefaultUrl = Config.GetUrl("videos/label/" + title + "/" + order.ToLower().Trim());
                PaginationUrl = Config.GetUrl("videos/label/" + title + "/" + order.ToLower().Trim() + "/[p]/");

                switch (order)
                {
                    case "mostviewed":
                        selectedOrder   = SiteConfig.generalLocalizer["_most_viewed"].Value;
                        _order = "video.views desc, video.created_at desc";
                        break;
                    case "toprated":
                        selectedOrder   = SiteConfig.generalLocalizer["_top_rated"].Value;
                        _order = "video.avg_rating desc, video.views desc";
                        break;
                    case "featured":
                        selectedOrder   = SiteConfig.generalLocalizer["_featured"].Value;
                        _order = "video.created_at desc";
                         _featuredFilter = FeaturedTypes.Featured;
                        break;
                }
            }
            if (filter != null)
            {
                // pagination setting
                if (filter == "today" || filter == "thisweek" || filter == "thismonth")
                {
                    if (order != null)
                    {
                        DefaultUrl = Config.GetUrl("videos/label/filter/" + title + "/" + filter.ToLower().Trim() + "/" + order);
                        PaginationUrl = Config.GetUrl("videos/label/filter/" + title + "/" + filter.ToLower().Trim() + "/" + order + "/[p]/");
                    }
                    else
                    {
                        DefaultUrl = Config.GetUrl("videos/label/filter/" + title + "/" + filter.ToLower().Trim());
                        PaginationUrl = Config.GetUrl("videos/label/filter/" + title + "/" + filter.ToLower().Trim() + "/[p]/");
                    }
                }

                switch (filter)
                {
                    case "today":
                        selectedFilter = SiteConfig.generalLocalizer["_today"].Value;
                        _dateFilter = DateFilter.Today;
                        break;
                    case "thisweek":
                        selectedFilter = SiteConfig.generalLocalizer["_this_week"].Value;
                         _dateFilter = DateFilter.ThisWeek;
                        break;
                    case "thismonth":
                        selectedFilter = SiteConfig.generalLocalizer["_this_month"].Value;
                         _dateFilter = DateFilter.ThisMonth;
                         break;
                }
            }

            /* List Initialization */
            var ListEntity = new VideoListViewModel()
            {
                isListStatus = true,
                isListNav = true,
                Navigation = prepareCategoryFilterLinks("/videos/label/", title, selectedOrder, selectedFilter),
                QueryOptions = new VideoEntity()
                {
                    type = MediaType.Videos,
                    pagenumber = (int)pagenumber,
                    tags = _term,
                    iscache = false,
                    term = "",
                    ispublic = true,
                    isfeatured = _featuredFilter,
                    pagesize = Jugnoon.Settings.Configs.GeneralSettings.pagesize,
                    datefilter = _dateFilter,
                    order = _order,
                },
                ListObject = new ListItems()
                {
                    ColWidth = "col-md-4",
                    TitleLength = 40,
                    ListType = ListType.Grid,
                    showRating = true,
                    showViews = true,
                    showDate = true
                },
                HeadingTitle = _meta.title,
                DefaultUrl = DefaultUrl,
                PaginationUrl = PaginationUrl,
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                BreadItems = _meta.BreadItems
            };

            ListEntity.TotalRecords = await VideoBLL.Count(_context, ListEntity.QueryOptions);
            if (ListEntity.TotalRecords > 0)
                ListEntity.DataList = await VideoBLL.LoadItems(_context, ListEntity.QueryOptions);

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;
            ViewBag.keywords = _meta.keywords;
            ViewBag.imageurl = _meta.imageurl;

            return View(ListEntity);
        }

        // GET: videos/archive
        public async Task<IActionResult> archive(string month, int year, string order, int? pagenumber)
        {
            if (pagenumber == null)
                pagenumber = 1;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                pagenumber = (int)pagenumber,
                matchterm = month,
                matchterm2 = year.ToString()
            });

            /* List Initialization */
            var ListEntity = new VideoListViewModel()
            {
                isListStatus = true,
                isListNav = false,
                QueryOptions = new VideoEntity()
                {
                    type = MediaType.Videos,
                    pagenumber = (int)pagenumber,
                    term = "",
                    month = UtilityBLL.ReturnMonth(month),
                    year =  year,
                    iscache = false,
                    ispublic = true,
                    pagesize = Jugnoon.Settings.Configs.GeneralSettings.pagesize,
                    order = "video.created_at desc"
                },
                ListObject = new ListItems()
                {
                    TitleLength = 30,
                    ColWidth = "col-md-4",
                    ListType = ListType.Grid,
                    showRating = true,
                    showViews = true,
                    showDate = true
                },
                HeadingTitle = _meta.title,
                DefaultUrl = Config.GetUrl("videos/archive/" + month + "/" + year + "/"),
                PaginationUrl = Config.GetUrl("videos/archive/" + month + "/" + year + "/[p]/"),
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                BreadItems = _meta.BreadItems
            };

            ListEntity.TotalRecords = await  VideoBLL.Count(_context, ListEntity.QueryOptions);
            if (ListEntity.TotalRecords > 0)
                ListEntity.DataList = await VideoBLL.LoadItems(_context, ListEntity.QueryOptions);

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;

            return View(ListEntity);
        }

        // GET: videos/categories
        public async Task<IActionResult> categories(int? pagenumber)
        {
            if (pagenumber == null)
                pagenumber = 1;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                pagenumber = (int)pagenumber,
            });
            
            /* List Initialization */
            var ListEntity = new CategoryListViewModel_v2()
            {
                isListStatus = true,
                isListNav = false,
                QueryOptions = new CategoryEntity()
                {
                    type = (int)CategoryBLL.Types.Videos,
                    pagenumber = (int)pagenumber,
                    ispublic = true,
                    term = "",
                    iscache = true,
                    pagesize = 30,
                    order = "title asc",
                },
                ListObject = new ListItems()
                {
                    ListType = ListType.Grid,
                    ColWidth = "col-md-6 col-sm-12",
                },
                Path = "videos/", // category url path
                DefaultUrl = Config.GetUrl("videos/categories"),
                PaginationUrl = Config.GetUrl("videos/categories/[p]"),
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                HeadingTitle = _meta.title,
                BreadItems = _meta.BreadItems
            };

            ListEntity.TotalRecords = await CategoryBLL.Count(_context, ListEntity.QueryOptions);
            if (ListEntity.TotalRecords > 0)
                ListEntity.DataList = await CategoryBLL.LoadItems(_context, ListEntity.QueryOptions);

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;

            return View(ListEntity);
        }

        // GET: videos/archivelist
        public IActionResult archivelist()
        {
            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName
            });

            /* List Initialization */
            var ListEntity = new ArchiveListModelView()
            {
                Type = 0, // represent videos
                Path = "videos/",
                HeadingTitle = _meta.title,
                isAll = true,
                BreadItems = _meta.BreadItems
            };

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;

            return View(ListEntity);
        }

        // GET: videos/labels
        public IActionResult labels(string term, int? pagenumber)
        {
            if (pagenumber == null)
                pagenumber = 1;

            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var order = "normal";
            if (term != null && term.Length > 0)
                order = "search";
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                order = order,
                pagenumber = (int)pagenumber
            });

            /* List Initialization */
            var ListEntity = new TagListModelView()
            {
                pagenumber = (int)pagenumber,
                TotalRecords = 100, // display 100 tags per page
                Type = (int)TagsBLL.Types.Videos, // represent videos
                Path = "videos/",
                DefaultUrl = Config.GetUrl("videos/labels"),
                PaginationUrl = Config.GetUrl("videos/labels/[p]/"),
                NoRecordFoundText = SiteConfig.generalLocalizer["_no_records"].Value,
                Action = "/videos/labels", // for search tags
                HeadingTitle = _meta.title,
                BreadItems = _meta.BreadItems
            };
          
            if (term != null && term.Length > 0)
            {
                ListEntity.Term = UtilityBLL.CleanSearchTerm(WebUtility.UrlDecode(term).Trim());
                ListEntity.DefaultUrl = Config.GetUrl("videos/labels/search/" + term);
                ListEntity.PaginationUrl = Config.GetUrl("videos/labels/search/" + term + "/[p]");
            }

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;

            return View(ListEntity);
        }

        private VideoListFilterViewModel prepareFilterLinks(string url, string selectedOrder, string selectedFilter)
        {
            return new VideoListFilterViewModel()
            {
                selectedFilter = selectedFilter,
                selectedOrder = selectedOrder,
                order_rating_url = url + "toprated",
                order_recent_url = url + "recent",
                order_view_url = url + "mostviewed",
                order_featured_url = url + "featured",
                filter_date_alltime_url = url,
                filter_date_thismonth_url = url + "added/thismonth",
                filter_date_thisweek_url = url + "added/thisweek",
                filter_date_today_url = url + "added/today"
            };
        }

        private VideoListFilterViewModel prepareCategoryFilterLinks(string url, string title, string selectedOrder, string selectedFilter)
        {
            return new VideoListFilterViewModel()
            {
                selectedFilter = selectedFilter,
                selectedOrder = selectedOrder,
                order_rating_url = url + "" + title + "/toprated",
                order_recent_url = url + "" + title + "/recent",
                order_view_url = url + "" + title + "/mostviewed",
                order_featured_url = url + "" + title + "/featured",
                filter_date_alltime_url = url + "" + title,
                filter_date_thismonth_url = url + "filter/" + title + "/thismonth",
                filter_date_thisweek_url = url + "filter/" + title + "/thisweek",
                filter_date_today_url = url + "filter/" + title + "/today"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult labels(TagListModelView model)
        {
            return Redirect(Config.GetUrl("videos/labels/search/" + WebUtility.UrlEncode(UtilityBLL.CleanSearchTerm(model.Query))));
        }
        #endregion

        #region Search Feature
        // GET: videos/search
        public async Task<IActionResult> search(string term, string filter, int? pagenumber)
        {            
            if (term == null)
                return Redirect("/videos/");
                       
            var _sanitize = new Ganss.XSS.HtmlSanitizer();
            term = UtilityBLL.ReplaceHyphinWithSpace(term); 
            
            /* ***************************************/
            // Process Page Meta & BreaCrumb 
            /* ***************************************/
            var _meta = PageMeta.returnPageMeta(new PageQuery()
            {
                controller = ControllerContext.ActionDescriptor.ControllerName,
                index = ControllerContext.ActionDescriptor.ActionName,
                pagenumber = (int)pagenumber,
                matchterm = term
            });

            if (Jugnoon.Settings.Configs.GeneralSettings.store_searches)
            {
                //*********************************************
                // User Search Tracking Script
                //********************************************
                if (!TagsBLL.Validate_Tags(term.Trim()) && !term.Trim().Contains("@"))
                {
                    // check if tag doesn't exist
                    var count_tags = await TagsBLL.Count(_context, new TagEntity()
                    {
                        type = TagsBLL.Types.General,
                        tag_type = TagsBLL.TagType.UserSearches,
                        isenabled = EnabledTypes.Enabled
                    });
                    if (count_tags == 0)
                        TagsBLL.Add(_context, term.Trim(), TagsBLL.Types.General, 0, TagsBLL.TagType.UserSearches, EnabledTypes.Enabled, term.Trim());

                }
            }

            /* List Initialization */
            var ListEntity = new VideoListViewModel()
            {
                QueryOptions = new VideoEntity()
                {
                    term = term
                },
                BreadItems = _meta.BreadItems
            };

            /**********************************************/
            // Page Meta Setup
            /**********************************************/
            ViewBag.title = _meta.title;
            ViewBag.description = _meta.description;

            return View(ListEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult queryresult(SearchListModel model)
        {
            return Redirect(Config.GetUrl("videos/search/" + WebUtility.UrlEncode(UtilityBLL.ReplaceSpaceWithHyphin(model.Query))));
        }
       
        #endregion

        #region Download Video


        public async Task<IActionResult> DownloadFile(long id)
        {
            if (id > 0)
            {
                var _lst = await VideoBLL.LoadItems(_context, new VideoEntity()
                {
                    id = id,
                    nofilter = true
                });
                if (_lst.Count > 0)
                {
                    try
                    {
                        string url = Return_Content_Url(_lst[0]);
                        if (url == "")
                        {
                            return this.Content("No File Exist", "text/plain");
                        }
                        // increment downloads
                        _lst[0].downloads = _lst[0].downloads + 1;
                        VideoBLL.Update_Field_V3(_context, id, _lst[0].downloads, "downloads");
                        // donwload file
                        FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
                        int length = Convert.ToInt32(fs.Length);
                        FileInfo info = new FileInfo(url);
                        string ContentType = ReturnContentType(info.Extension);
                        Request.HttpContext.Response.Headers.Add("Content-Type", ContentType);
                        string filename = _lst[0].videofilename;
                        if (_lst[0].title != null)
                        {
                            filename = UtilityBLL.ReplaceSpaceWithHyphin(_lst[0].title);
                            if (filename.Length > 30)
                                filename = filename.Substring(0, 30);

                            string website_caption = "";
                            if (Jugnoon.Settings.Configs.GeneralSettings.page_caption != "")
                                website_caption = "-[" + UtilityBLL.ReplaceSpaceWithHyphin(Jugnoon.Settings.Configs.GeneralSettings.page_caption) + "]";
                            filename = filename + "" + website_caption + "" + info.Extension;
                        }
                        Request.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=" + filename.ToLower() + "");
                        Request.HttpContext.Response.Headers.Add("Content-Length", length.ToString());
                        var data = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read);
                        return File(data, "video/mp4");//FileStreamResult
                    }
                    catch (Exception ex)
                    {
                        return this.Content(SiteConfig.videoLocalizer["video_download_01"].Value, "text/plain"); // Processing Error Occured
                    }
                }
                else
                {
                    return this.Content(SiteConfig.videoLocalizer["video_download_02"].Value, "text / plain"); // No Download Link Available
                }
            }
            return this.Content(SiteConfig.videoLocalizer["video_download_01"].Value, "text/plain");
        }
     
        private string Return_Content_Url(JGN_Videos vd)
        {
            string url = "";
            /*if (vd.pub_url == "Amazon")
            {
                // cloud storage
                if (vd.type == 0)
                {
                    // video content
                    url = "http://s3.amazonaws.com/" + CloudSettings.VideoBucketName + "/" + vd.videofilename;
                }
                else
                {
                    // audio content
                    url = "http://s3.amazonaws.com/" + CloudSettings.AudioBuketName + "/" + vd.videofilename;
                }
            }
            else
            { */
                // normal
                int ctype = 1; // published video
                if (vd.type == 0)
                    ctype = 1; // published video file
                else
                    ctype = 2; // audio file

                url = VideoUrlConfig.Return_Video_physicalPath(vd.pub_url, vd.userid, vd.videofilename, ctype);

                // validate content on url
                if (!System.IO.File.Exists(url))
                {
                    url = "";
                }
           // }
            return url;
        }

        private static string ReturnContentType(string extension)
        {
            string Contenttype = "application/octet-stream";
            switch (extension)
            {
                case ".mp4":
                    Contenttype = "video/mp4";
                    break;
                case ".flv":
                    Contenttype = "video/x-flv";
                    break;
                case ".wmv":
                    Contenttype = "video/x-ms-wm";
                    break;
                case ".3gp":
                    Contenttype = "video/3gpp";
                    break;
                case ".webm":
                    Contenttype = "video/webm";
                    break;
                case ".ogv":
                    Contenttype = "video/ogg";
                    break;
                case ".mpeg":
                    Contenttype = "video/mpeg";
                    break;
                case ".mov":
                    Contenttype = "video/quicktime";
                    break;
                case ".avi":
                    Contenttype = "video/x-msvideo";
                    break;
                case ".mp3":
                    Contenttype = "audio/mpeg";
                    break;
                case ".bmp":
                    Contenttype = "image/bmp";
                    break;
                case ".gif":
                    Contenttype = "image/gif";
                    break;
                case ".jpeg":
                    Contenttype = "image/jpeg";
                    break;
                case ".jpg":
                    Contenttype = "image/jpeg";
                    break;
                case ".png":
                    Contenttype = "image/png";
                    break;
                case ".rm":
                    Contenttype = "application/vnd.rn-realmedia";
                    break;


            }
            return Contenttype;
        }

        #endregion

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
