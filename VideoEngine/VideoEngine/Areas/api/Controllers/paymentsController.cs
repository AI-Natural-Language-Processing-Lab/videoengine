using Jugnoon.BLL;
using Jugnoon.Entity;
using Jugnoon.Framework;
using Jugnoon.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Jugnoon.Settings;
using Jugnoon.Models;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class paymentsController : ControllerBase
    {
        ApplicationDbContext _context;
        public paymentsController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
            IStringLocalizer<GeneralResource> generalLocalizer,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           IOptions<General> generalSettings
       )
        {
            // content specific settings
            Configs.GeneralSettings = generalSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            _context = context;
           
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }
        [HttpPost("packages")]
        public async Task<ActionResult> packages()
        {
            int packagetype = 1; // paid packages only
            if (Jugnoon.Settings.Configs.PremiumSettings.premium_option == 1)
                packagetype = 2; // renew account packages

            var Packages = await PackagesBLL.Load(_context, new PackageEntity()
            {
                id = packagetype
            });

            return Ok(new { status = "success", packages = Packages });
        }

        [HttpPost("history")]
        public async Task<ActionResult> history()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<ApplicationUser>(json);

            var History = await UserPackagesBLL.Load_User_Payment_History(_context, data.UserName, true, 100); // fix top 100 records (no pagination);

            return Ok(new { status = "success", history = History });
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
