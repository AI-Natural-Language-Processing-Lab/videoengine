using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.BLL;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Jugnoon.Utility;
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
    public class mailtemplatesController : ControllerBase
    {
        ApplicationDbContext _context;
        public mailtemplatesController(
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

        [HttpPost("index")]
        public async Task<ActionResult> Index()
        {
            var data = new MailTemplateEntity()
            {
                pagenumber = 1,
                type = "-1",
                pagesize = 20,
                order = "id desc"
            };
            var _posts = await MailTemplateBLL.Load(_context, data);;
            return Ok(new { posts = _posts, records = 434 });
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<MailTemplateEntity>(json);
            var _posts = await MailTemplateBLL.Load(_context, data);;
            var _records = 0;
            if (data.id == 0)
                _records = await MailTemplateBLL.Count(_context, data);
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<MailTemplateEntity>>(json);
            var _posts = await MailTemplateBLL.Load(_context, data[0]);
            if (_posts.Count > 0)
                return Ok(new { status = "success", posts = _posts[0] });
            else
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
        }

        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_MailTemplates>(json);

            if (data.id > 0)
            {
                // Update Operation
                MailTemplateBLL.Update_Record(_context, data.id,data.subject,data.description, data.contents,data.tags,data.subjecttags);
            }
            else
            {
                // check whether template already exist
                if (MailTemplateBLL.CheckTemplate(_context, data.templatekey))
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_record_exist"].Value });
                }
                // Add Operation
                data = await MailTemplateBLL.Add(_context, data);
            }

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<MailTemplateEntity>>(json);

            MailTemplateBLL.ProcessAction(_context, data);

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
