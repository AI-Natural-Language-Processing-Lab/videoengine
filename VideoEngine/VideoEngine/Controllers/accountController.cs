using Jugnoon.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoEngine.Controllers
{
    [Authorize]
    public class accountController : Controller
    {
        public accountController()
        {   }

        public IActionResult Index()
        {
            ViewBag.title = SiteConfig.generalLocalizer["_my_account"].Value;
            
            return View();
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
