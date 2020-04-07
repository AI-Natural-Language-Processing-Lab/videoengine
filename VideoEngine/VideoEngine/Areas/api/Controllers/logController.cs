using System.Collections.Generic;
using Jugnoon.BLL;
using Jugnoon.Entity;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Jugnoon.Utility;
using Jugnoon.Framework;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Jugnoon.Settings;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class logController : ControllerBase
    {
        ApplicationDbContext _context;
        public logController(
           IOptions<SiteConfiguration> settings,
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
            _context = context;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }
        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<LogEntity>(json);
            var _posts = await ErrorLgBLL.Load(_context, data);;
            var _records = 0;
            if (data.id == 0)
                _records = await ErrorLgBLL.Count(_context, data);
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<LogEntity>>(json);
            var _posts = await ErrorLgBLL.Load(_context, data[0]);
            return Ok(new { posts = _posts[0] });
        }

        [HttpPost("proc")]
        public ActionResult proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Log>(json);

            // Add Operation
            ErrorLgBLL.Add(_context, data.description, data.url, data.stack_trace);

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<LogEntity>>(json);

            ErrorLgBLL.ProcessAction(_context, data);

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
