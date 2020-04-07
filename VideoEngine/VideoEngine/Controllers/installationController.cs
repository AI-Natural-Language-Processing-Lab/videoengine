using Jugnoon.Settings;
using Jugnoon.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace VideoEngine.Controllers
{
    public class installationController : Controller
    {
        public installationController(IOptions<SiteConfiguration> settings,
             IOptions<General> generalSettings)
        {
            Jugnoon.Settings.Configs.GeneralSettings = generalSettings.Value;
            SiteConfig.Config = settings.Value;
        }

        public IActionResult Index()
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.init_wiz)
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Configs()
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.init_wiz)
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }
    }

}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
