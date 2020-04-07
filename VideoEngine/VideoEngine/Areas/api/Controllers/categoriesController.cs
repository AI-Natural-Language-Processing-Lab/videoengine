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
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class categoriesController : ControllerBase
    {
        ApplicationDbContext _context;
       
        public categoriesController(
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
            var data = JsonConvert.DeserializeObject<CategoryEntity>(json);
            var _posts = await CategoryBLL.LoadItems(_context, data);
            foreach(var item in _posts)
            {
                if(item.picturename.StartsWith("http"))
                {
                    item.img_url = item.picturename;
                }
                else
                {
                    item.img_url = Config.GetUrl("contents/category/" + item.picturename);
                }
            }
            var _records = 0;
            if (data.id == 0)
                _records = await CategoryBLL.Count(_context, data);

            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("load_nm")]
        public ActionResult load_nm()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CategoryEntity>(json);
         
            var _list = CategoryBLL.LoadItems(_context, data);

            return Ok(new { categorylist = _list });
        }

        [HttpPost("load_dropdown")]
        public async Task<ActionResult> load_dropdown()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<CategoryEntity>(json);

            var _entity = new CategoryEntity();
            _entity.isdropdown = true;
            _entity.issummary = false;
            _entity.parentid = -1;
            _entity.order = "level asc";
            _entity.loadall = true;
            _entity.pagesize = 10000; // all categories
            _entity.type = data.type;
            _entity.isenabled = EnabledTypes.All;

            var _posts = await CategoryBLL.LoadItems(_context, _entity);
                       
            return Ok(new { posts = _posts });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<CategoryEntity>>(json);
            var _posts = await CategoryBLL.LoadItems(_context, data[0]);
            if (_posts.Count > 0)
            {
                _posts[0].img_url = CategoryUrlConfig.PrepareImageUrl(_posts[0]);
                return Ok(new { status = "success", post = _posts[0] });
            }
            else
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }
        }

        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Categories>(json);
            
            data.id = await CategoryBLL.Process(_context, data);

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_record_created"].Value });
        }
       
        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<CategoryEntity>>(json);

            CategoryBLL.ProcessAction(_context, data);

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
