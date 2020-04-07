using Jugnoon.Utility;
using System.Collections.Generic;
using Jugnoon.Models;

namespace Jugnoon.Meta
{
    public class Data
    {
        /// <summary>
        /// It is responsible for managing static pages meta data, You can register and setup meta information for static pages here. 
        /// </summary>
        /// <returns></returns>
        public static List<Page> ReturnStaticPagesData()
        {

            var Pages = new List<Page>
            {
                new Page {
                    pagename = "index",
                    viewname = "index",
                    title = SiteConfig.generalLocalizer["_meta_home"].Value,
                    description = SiteConfig.generalLocalizer["_meta_home_desc"].Value,
                    imageurl = "",
                    style_exists = true,
                    script_exists = true
                },
                new Page {
                    pagename = "signup",
                    viewname = "signup",
                    title = SiteConfig.generalLocalizer["_meta_signup"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = true
                },
                new Page {
                    pagename = "signin",
                    viewname = "signin",
                    title = SiteConfig.generalLocalizer["_meta_signin"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "forgot-password",
                    viewname = "forgot_password",
                    title = SiteConfig.generalLocalizer["_meta_forgot_password"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "forgot-password-confirmation",
                    viewname = "forgot_password_confirmation",
                    title = SiteConfig.generalLocalizer["_meta_forgot_password_confirmation"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "access-denied",
                    viewname = "access_denied",
                    title = SiteConfig.generalLocalizer["_meta_access_denied"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "activate",
                    viewname = "activate",
                    title = SiteConfig.generalLocalizer["_meta_activate_account"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "change-email",
                    viewname = "change_email",
                    title = SiteConfig.generalLocalizer["_meta_change_email"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "confirm-email",
                    viewname = "confirm_email",
                    title = SiteConfig.generalLocalizer["_meta_confirm_email"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "external-login",
                    viewname = "external_login",
                    title = SiteConfig.generalLocalizer["_meta_external_login_provider"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "lockout",
                    viewname = "lockout",
                    title = SiteConfig.generalLocalizer["_meta_lock_out"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "loginwith2fa",
                    viewname = "loginwith2fa",
                    title = SiteConfig.generalLocalizer["_meta_2_factor_auth"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "loginwithrecoverycode",
                    viewname = "loginwithrecoverycode",
                    title = SiteConfig.generalLocalizer["_meta_login_with_recovery_code"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "reset-password",
                    viewname = "reset_password",
                    title = SiteConfig.generalLocalizer["_meta_reset_password"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "reset-password-confirmation",
                    viewname = "reset_password_confirmation",
                    title = SiteConfig.generalLocalizer["_meta_reset_password_confirmation"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page {
                    pagename = "signout",
                    viewname = "signout",
                    title = SiteConfig.generalLocalizer["_meta_sign_out"].Value,
                    description = "",
                    imageurl = "",
                    style_exists = false,
                    script_exists = false
                },
                new Page()
                {
                    pagename = "signout-confirm",
                    viewname = "signout_confirm",
                    title = SiteConfig.generalLocalizer["_meta_sign_out"].Value,
                    description = "",
                    keywords = ""
                },
                new Page {
                    pagename = "contact",
                    viewname = "contact",
                    title = SiteConfig.generalLocalizer["_meta_contact_us"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    pagename = "privacy-policy",
                    viewname = "privacy",
                    title = SiteConfig.generalLocalizer["_meta_privacy_policy"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    pagename = "terms-of-use",
                    viewname = "terms",
                    title = SiteConfig.generalLocalizer["_meta_terms_of_use"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    pagename = "about-us",
                    viewname = "about",
                    title = SiteConfig.generalLocalizer["_meta_about_us"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    pagename = "sitemap",
                    viewname = "sitemap",
                    title = SiteConfig.generalLocalizer["_meta_sitemap"].Value,
                    description = "",
                    imageurl = ""
                },
                new Page {
                    pagename = "security",
                    viewname = "security",
                    title = SiteConfig.generalLocalizer["_meta_security"].Value,
                },
                new Page {
                    pagename = "gdpr",
                    viewname = "gdpr",
                    title = SiteConfig.generalLocalizer["_meta_gdpr"].Value,
                },
                new Page {
                    pagename = "cookies",
                    viewname = "cookies",
                    title = SiteConfig.generalLocalizer["_meta_cookies"].Value,
                },
                new Page {
                    pagename = "thirdparties",
                    viewname = "thirdparties",
                    title = SiteConfig.generalLocalizer["_meta_third_party"].Value,
                },
                new Page {
                    pagename = "validate-adult",
                    index = "validate_adult",
                    title = SiteConfig.generalLocalizer["_meta_adult_warning"].Value
                },
            };

            return Pages;
        }

    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
