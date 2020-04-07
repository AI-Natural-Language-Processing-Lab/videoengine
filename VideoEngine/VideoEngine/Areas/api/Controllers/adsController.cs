using Jugnoon.Utility;
using System.Collections.Generic;
using Jugnoon.BLL;
using Jugnoon.Entity;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class adsController : ControllerBase
    {
        ApplicationDbContext _context;

        public adsController(
        IOptions<SiteConfiguration> settings,
        IMemoryCache memoryCache,
        ApplicationDbContext context,
         IStringLocalizer<GeneralResource> generalLocalizer,
        IHttpContextAccessor _httpContextAccessor
        )
        {
            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            _context = context;
           
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<AdEntity>(json);
            var _posts = await AdsBLL.Load(_context, data);
            var _records = 0;
            if (data.id == 0)
                _records = await AdsBLL.Count(_context, data);
            
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<AdEntity>>(json);
            var _posts = await AdsBLL.Load(_context, data[0]);
            return Ok(new { posts = _posts[0] });
        }

        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Ads>(json);
            if (data.id > 0)
            {
                // Update Operation
                AdsBLL.Update_Field_V3(_context, data.id, data.adscript.ToString(), "adscript");
            }
            else
            {
                // Add Operation
                await AdsBLL.Add_Script(_context, data.name, data.adscript, data.type);
            }
            return Ok(new { status = "success", id = 0, SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<AdEntity>>(json);

            AdsBLL.ProcessAction(_context, data);

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
