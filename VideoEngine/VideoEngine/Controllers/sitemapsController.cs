using Jugnoon.BLL;
using System;
using Microsoft.AspNetCore.Mvc;
using Jugnoon.Entity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Jugnoon.Utility;
using VideoEngine.Models;
using System.Threading.Tasks;
using Jugnoon.Settings;
using Jugnoon.Localize;

namespace VideoEngine.Controllers
{
    public class sitemapsController : Controller
    {
        ApplicationDbContext _context;
        public sitemapsController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
           IStringLocalizer<GeneralResource> generalLocalizer,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           IOptions<General> generalSettings,
           IOptions<Features> featureSettings
           )
        {

            _context = context;
            // general settings
            Configs.GeneralSettings = generalSettings.Value;
            Configs.FeatureSettings = featureSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        public async Task<IActionResult> categories(string rt = null, string type = null)
        {
            int tp = 0;
            if (type != null)
                tp = Convert.ToInt32(type);

            string sXml = "";
            int responsetype = 0; // 0: google, 1: bing
            if (rt != null)
                responsetype = Convert.ToInt32(rt);

            switch (responsetype)
            {
                case 0:
                    sXml = await XMLBLL.Google_CategorySitemap(_context, new CategoryEntity()
                    {
                        ispublic = true,
                        type = tp,
                        pagesize = 50000,
                        order= "id desc"
                    });
                    break;
                case 1:
                    sXml = await XMLBLL.Bing_CategorySitemap(_context, new CategoryEntity()
                    {
                        ispublic = true,
                        type = tp,
                        pagesize = 50000,
                        order = "id desc"
                    });
                    break;
            }

            return this.Content(sXml, "text/xml");
        }


        public IActionResult tags(int? type = null, int? page = null, int? rt = null)
        {
            int pagenumber = 1;

            int tp = 0;
            if (type != null)
                tp = (int)type;

            if (page != null)
                pagenumber = (int)page;

            string sXml = "";
            int responsetype = 0; // 0: google, 1: bing
            if (rt != null)
                responsetype = (int)rt;

            switch (responsetype)
            {
                case 0:
                    sXml = XMLBLL.Google_TagySitemap(_context, new TagEntity()
                    {
                        ispublic = true,
                        type  = (TagsBLL.Types)tp,
                        pagesize = 50000,
                        pagenumber = pagenumber,
                        order = "id desc"
                    });
                    break;
                case 1:
                    sXml = XMLBLL.Bing_TagSitemap(_context, new TagEntity()
                    {
                        ispublic = true,
                        type = (TagsBLL.Types)tp,
                        pagesize = 50000,
                        pagenumber = pagenumber,
                        order = "id desc"
                    });
                    break;
            }

            return this.Content(sXml, "text/xml");
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
