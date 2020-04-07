using System.Collections.Generic;
using Jugnoon.Settings;
using Jugnoon.Utility;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Jugnoon.Framework;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Jugnoon.Attributes;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class attrController : ControllerBase
    {
        ApplicationDbContext _context;

        public attrController(
          IOptions<SiteConfiguration> settings,
          ApplicationDbContext context,
           IStringLocalizer<GeneralResource> generalLocalizer,
          IWebHostEnvironment _environment,
          IHttpContextAccessor _httpContextAccessor,
          IOptions<General> generalSettings
        )
        {
            // readable configuration
            Configs.GeneralSettings = generalSettings.Value;
           
            // other injectors
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
            var entity = JsonConvert.DeserializeObject<AttrTemplateEntity>(json);

            if (!entity.skip_template)
            {
                var _templates = await AttrTemplatesBLL.LoadItems(_context, entity);
                foreach (var item in _templates)
                {
                    item.sections = await AttrTemplatesSectionsBLL.LoadItems(_context, new AttrTemplateSectionEntity() { templateid = item.id, order = "priority desc" });
                }

                return Ok(new { posts = _templates });
            }
            else
            {
                var _sections = await AttrTemplatesSectionsBLL.LoadItems(_context, new AttrTemplateSectionEntity() { attr_type = entity.attr_type, order = "priority desc" });
                return Ok(new { posts = _sections });
            }
        }

        [HttpPost("load_attr")]
        public async Task<ActionResult> load_attr()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<AttrAttributeEntity>(json);
            data.nofilter = false;
            data.order = "priority desc";
            var _posts = await AttrAttributeBLL.LoadItems(_context, data);

            var _records = 0;

            return Ok(new { posts = _posts, records = _records });
        }


        [HttpPost("proc_template")]
        public async Task<ActionResult> proc_template()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Attr_Templates>(json);

            if (data.id > 0)
                await AttrTemplatesBLL.Update(_context, data);
            else
            {
                data = await AttrTemplatesBLL.Add(_context, data);
            }

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_record_created"].Value });
        }

        [HttpPost("proc_section")]
        public async Task<ActionResult> proc_section()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var attr = JsonConvert.DeserializeObject<JGN_Attr_TemplateSections>(json);

            if (attr.id > 0)
            {
                // update attribute 
                await AttrTemplatesSectionsBLL.Update(_context, attr);
            }
            else
            {
                attr = await AttrTemplatesSectionsBLL.Add(_context, attr);
            }

            return Ok(new { status = "success", record = attr, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("delete_template")]
        public async Task<ActionResult> delete_template()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<JGN_Attr_Templates>>(json);

            foreach (var item in items)
            {
                await AttrTemplatesBLL.Delete(_context, item.id);
            }

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_deleted"].Value });
        }

        [HttpPost("delete_section")]
        public async Task<ActionResult> delete_section()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<JGN_Attr_TemplateSections>>(json);

            foreach (var item in items)
            {
                await AttrTemplatesSectionsBLL.Delete(_context, item.id);
            }

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_deleted"].Value });
        }


        [HttpPost("add_attr")]
        public async Task<ActionResult> add_attr()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var attr = JsonConvert.DeserializeObject<JGN_Attr_Attributes>(json);

            // attr.attr_type = (byte)AttrAttributeBLL.Attr_Type.Ad;

            if (attr.id > 0)
            {
                // update attribute 
                await AttrAttributeBLL.Update(_context, attr);
            }
            else
            {
                attr = await AttrAttributeBLL.Add(_context, attr);
            }

            return Ok(new { status = "success", record = attr, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("delete_attr")]
        public async Task<ActionResult> delete_attr()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var attributes = JsonConvert.DeserializeObject<List<JGN_Attr_Attributes>>(json);

            foreach (var attr in attributes)
            {
                await AttrAttributeBLL.Delete(_context, attr.id);
            }

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_deleted"].Value });
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
