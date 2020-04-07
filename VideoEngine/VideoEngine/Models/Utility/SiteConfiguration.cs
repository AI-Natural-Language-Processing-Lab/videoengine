
using Jugnoon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Jugnoon.Localize;

namespace Jugnoon.Utility
{
    public class SiteConfig
    {
        public static SiteConfiguration Config  { get; set; }
        public static IMemoryCache Cache { get; set; }
        public static UserManager<ApplicationUser> userManager { get; set; }
        public static RoleManager<ApplicationRole> roleManager { get; set; }
        public static IStringLocalizer<GeneralResource> generalLocalizer { get;set;  }
        public static IStringLocalizer<VideoResource> videoLocalizer { get; set; }
        public static IWebHostEnvironment Environment { get; set; }
        public static IHttpContextAccessor HttpContextAccessor { get; set; }

    }

    public class SiteConfiguration
    {
        public static string URL { get; set; } = "";
    }

    public class SystemDirectoryPaths
    {
        /// <summary>
        /// Default directory path for saving user uploaded media contents
        /// </summary>
        public static string UserDirectory { get; set; } = "/wwwroot/contents/member/[USERNAME]/";

        /// <summary>
        /// Default directory url for users uploaded media contents
        /// </summary>
        public static string UserUrlPath { get; set; } = "contents/member/[USERNAME]/";

        /// <summary>
        /// Default directory path for saving uploaded category images
        /// </summary>
        public static string CategoryPhotosDirectoryPath { get; set; } = "/wwwroot/contents/category/";

        /// <summary>
        /// Default directory path for saving user photos
        /// </summary>
        public static string UserPhotosDirPath { get; set; } = "/wwwroot/contents/member/[USERNAME]/photos/";

        /// <summary>
        /// Application directory path where account application hosted and published (Angular App)
        /// </summary>
        public static string MyAccountAppPath { get; set; } = "/wwwroot/app/account/dist/";

        /// <summary>
        /// Application directory path where admin control panel application hosted and published (Angular App)
        /// </summary>
        public static string AdminAppPath { get; set; } = "/wwwroot/app/admin/dist/";

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
