using System.Collections.Generic;
using Jugnoon.BLL;
using Jugnoon.Entity;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Jugnoon.Utility;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Jugnoon.Settings;
using Jugnoon.Localize;

// This is control panel specific role management (Not core asp.net identity role management)
namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class roleController : ControllerBase
    {
        ApplicationDbContext _context;
        public roleController(
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
            var entity = new RoleEntity()
            {
                order = "id desc",
                pagesize = 4000,
                pagenumber = 1
            };
            var _posts = await RoleBLL.LoadItems(_context, entity);
            var _records = 0;
            if (entity.id == 0)
                _records = await RoleBLL.Count(_context, entity);
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<RoleEntity>(json);
            var _posts = await RoleBLL.LoadItems(_context, data);

            if (_posts.Count == 0)
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }

            foreach(var post in _posts)
            {
                post.permissions = await RolePermission.LoadItems(_context, new RoleDPermissionEntity()
                {
                    roleid = post.id,
                    order = "id desc",
                    pagesize = 5000,
                    pagenumber = 1
                });
            }
            return Ok(new { status = "success", posts = _posts[0] });
        }

        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var item = JsonConvert.DeserializeObject<JGN_Roles>(json);

            item = await RoleBLL.Add(_context, item);

            return Ok(new { status = "success", record = item, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<RoleEntity>>(json);

            RoleBLL.ProcessAction(_context, data);

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
