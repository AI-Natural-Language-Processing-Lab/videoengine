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
    public class ipController : ControllerBase
    {
        ApplicationDbContext _context;
        public ipController(
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
            var data = JsonConvert.DeserializeObject<BlockIPEntity>(json);
           
            var _posts = await BlockIPBLL.LoadItems(_context, data);;
            var _records = 0;
            if (data.id == 0)
                _records = await BlockIPBLL.Count(_context, data);

            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<BlockIPEntity>>(json);

            var _posts = await BlockIPBLL.LoadItems(_context, data[0]);

            return Ok(new { posts = _posts[0] });
        }

        [HttpPost("proc")]
        public ActionResult proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var item = JsonConvert.DeserializeObject<JGN_BlockIP>(json);

            if (item.id > 0)
            {
                // Update Operation
                BlockIPBLL.Update_Field_V3(_context, item.id, item.ipaddress, "ipaddress");
            }
            else
            {
                // Add Operation
                item = BlockIPBLL.Add_IP(_context, item);
            }

            return Ok(new { status = "success", record = item, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<BlockIPEntity>>(json);

            BlockIPBLL.ProcessAction(_context, data);

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
