using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.BLL;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
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
    public class tagsController : ControllerBase
    {
        ApplicationDbContext _context;
        public tagsController(
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

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<TagEntity>(json);
            var _posts = await TagsBLL.LoadItems(_context, data);
            var _records = 0;
            if (data.id == 0)
                _records =  await TagsBLL.Count(_context, data);
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<TagEntity>>(json);
            var _posts = await TagsBLL.LoadItems(_context, data[0]);
            return Ok(new { post = _posts[0] });
        }

        [HttpPost("proc")]
        public ActionResult proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Tags>(json);
            
            if (data.title.Length < 3)
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_title"].Value });
            }

            if (UtilityBLL.isLongWordExist(data.title) || UtilityBLL.isLongWordExist(data.title))
            {
                return Ok(new { status = "error",message = SiteConfig.generalLocalizer["_invalid_title"].Value });
            }

            dynamic CategoryContentType = 1;
            if (data.id > 0)
            {
                // Update Operation
                TagsBLL.Update(_context, data);
            }
            else
            {
                // Add Operation
                TagsBLL.Add(_context, data.title, (TagsBLL.Types)data.type, data.records, data.term);
            }

            return Ok(new { status = "success", id = data.id, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<TagEntity>>(json);

            TagsBLL.ProcessAction(_context, data);

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
