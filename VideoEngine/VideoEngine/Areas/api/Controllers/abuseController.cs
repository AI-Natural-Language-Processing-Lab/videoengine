using Jugnoon.Utility;
using System.Collections.Generic;
using Jugnoon.BLL;
using Jugnoon.Entity;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
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
    public class abuseController : ControllerBase
    {
        ApplicationDbContext _context;

        public abuseController(
        IOptions<SiteConfiguration> settings,
        ApplicationDbContext context,
         IStringLocalizer<GeneralResource> generalLocalizer,
        IHttpContextAccessor _httpContextAccessor
        )
        {
            SiteConfig.Config = settings.Value;
            _context = context;

             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<AbuseEntity>(json);
            var _posts = await AbuseReport.LoadItems(_context, data);
            var _records = 0;
            if (data.id == 0)
                _records = await AbuseReport.Count(_context, data);

            return Ok(new { posts = _posts, records = _records });
        }


        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_AbuseReports>(json);
            if (data.id > 0)
            {
                // Update Operation
                await AbuseReport.Update(_context, data);
                return Ok(new { status = "success", id = 0, message = SiteConfig.generalLocalizer["_records_processed"].Value });
            }
            else
            {
                return Ok(new { status = "success", id = 0, message = SiteConfig.generalLocalizer["_records_processed"].Value });
            }
           
        }

        [HttpPost("action")]
        public async Task<ActionResult> action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<AbuseEntity>>(json);

            await AbuseReport.ProcessAction(_context, data);

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
