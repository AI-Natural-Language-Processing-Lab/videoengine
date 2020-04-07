using System;
using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Settings;
using Jugnoon.Utility;
using Jugnoon.BLL;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using VideoEngine.Models;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class commentsController : ControllerBase
    {
        ApplicationDbContext _context;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public commentsController(
            IOptions<SiteConfiguration> settings,
            IMemoryCache memoryCache,
            ApplicationDbContext context,
             IStringLocalizer<GeneralResource> generalLocalizer,
            IWebHostEnvironment _environment,
            IOptions<General> generalSettings,
            IOptions<Media> mediaSettings,
            IOptions<Aws> awsSettings
        )
        {
            // readable configuration
            Configs.GeneralSettings = generalSettings.Value;
            Configs.AwsSettings = awsSettings.Value;
            Configs.MediaSettings = mediaSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            _context = context;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CommentEntity>(json);
            var _posts = await CommentsBLL.LoadItems(_context, data);
            
            var _records = 0;
            if (data.id == 0)
                _records = await CommentsBLL.Count(_context, data);

            return Ok(new { posts = _posts, records = _records });
        }

       
        [HttpPost("proc")]
        public ActionResult proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Comments>(json);

            data = CommentsBLL.Process(_context, data);

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_record_created"].Value });
        }

     
        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<CommentEntity>>(json);

            CommentsBLL.ProcessAction(_context, data);

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
