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
using System.Threading.Tasks;
using Jugnoon.Settings;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class dictionaryController : ControllerBase
    {
        ApplicationDbContext _context;
        public dictionaryController(
            IOptions<SiteConfiguration> settings,
            ApplicationDbContext context,
             IStringLocalizer<GeneralResource> generalLocalizer,
            IOptions<General> generalSettings
        )
        {
            // site specific settings
            Configs.GeneralSettings = generalSettings.Value;

            SiteConfig.Config = settings.Value;
            _context = context;
           
             SiteConfig.generalLocalizer = generalLocalizer;
        }
        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<DictionaryEntity>(json);
            var _posts = await DictionaryBLL.LoadItems(_context, data);;
            var _records = 0;
            if (data.id == 0)
                _records = await DictionaryBLL.Count(_context, data);
            return Ok(new { posts = _posts, records = _records });
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<DictionaryEntity>>(json);
            var _posts = await DictionaryBLL.LoadItems(_context, data[0]);
            return Ok(new { posts = _posts[0] });
        }

        [HttpPost("proc")]
        public ActionResult proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Dictionary>(json);
            if (DictionaryBLL.CheckValue(_context, data.value, data.type))
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_record_exist"].Value });
            }
            if (data.id > 0)
            {
                // Update Operation
                DictionaryBLL.Update(_context, data);
            }
            else
            {
                // Add Operation
                data = DictionaryBLL.Add(_context, data);
            }

            return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("action")]
        public ActionResult action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<DictionaryEntity>>(json);

            DictionaryBLL.ProcessAction(_context, data);

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
