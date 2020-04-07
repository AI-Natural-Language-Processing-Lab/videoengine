using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VideoEngine.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Jugnoon.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Jugnoon.Models;
using Jugnoon.Services;
using Microsoft.AspNetCore.Localization;
using Jugnoon.Settings;
using Jugnoon.Meta;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Jugnoon.Localize;

namespace VideoEngine.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public HomeController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
            IStringLocalizer<GeneralResource> generalLocalizer,
           IEmailSender emailSender,
           IOptions<General> generalSettings,
           IOptions<Features> featureSettings,
           IOptions<Smtp> smtpSettings,
           IOptions<Media> mediaSettings,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           SignInManager<ApplicationUser> signInManager
           )
        {
            _context = context;
            _emailSender = emailSender;
            _signInManager = signInManager;

            // readable configuration
            Configs.GeneralSettings = generalSettings.Value;
            Configs.SmtpSettings = smtpSettings.Value;
            Configs.FeatureSettings = featureSettings.Value;
            Configs.MediaSettings = mediaSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        public async Task<ActionResult> Index(string page, string returnUrl = null)
        {
            // check for installation flag
            if (!Configs.GeneralSettings.init_wiz)
            {
                return Redirect("/installation/configs/");
            }
                      

            /* Update culture */
            if (HttpContext.Request.Query["lng"].Count > 0)
            {
                Response.Cookies.Append(
                   CookieRequestCultureProvider.DefaultCookieName,
                   CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(HttpContext.Request.Query["lng"].ToString())),
                   new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
               );
               return Redirect(Config.GetUrl());
            }


            /* Update demo themes */
            if (HttpContext.Request.Query["theme"].Count > 0)
            {
                Jugnoon.Utility.Helper.Cookie.WriteCookie("VSKTheme", HttpContext.Request.Query["theme"].ToString());

                return Redirect(Config.GetUrl());
            }


            ViewData["ReturnUrl"] = returnUrl;
            if (page == null)
                page = "index";
            else
            {
                if (page == "404")
                    Response.StatusCode = 404;

                ViewData["Page"] = page;
            }

            ViewData["Page"] = page;

            // authorization check
            if (page != null)
            {
                var pages = Data.ReturnStaticPagesData();
                foreach (var pg in pages)
                {
                    if (pg.pagename == page)
                    {
                       
                    }
                }
            }

            // sign out
            if (page == "signout")
            {
                await _signInManager.SignOutAsync();
                return Redirect("/signout-confirm");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult contact(ContactUsViewModel model, string action)
        {
            ViewData["Page"] = "contact";
            if (ModelState.IsValid)
            {
                _emailSender.ContactUsEmailAsync(_context, Configs.GeneralSettings.admin_mail, model);

                model.Message = SiteConfig.generalLocalizer["_message_sent"].Value;
                model.AlertType = AlertTypes.Success;

                return View("~/Views/Home/index.cshtml", model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult validateadult(string submit)
        {
            switch (submit)
            {
                case "enter":
                    return (Enter());
                case "canel":
                    return (Cancel());
                default:
                    return (View());
            }
        }

        private ActionResult Enter(string surl = null)
        {
            if (surl != null)
                return Redirect(surl);
            else
                return Redirect(Config.GetUrl());
        }

        private ActionResult Cancel()
        {
            return Redirect(Config.GetUrl());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
