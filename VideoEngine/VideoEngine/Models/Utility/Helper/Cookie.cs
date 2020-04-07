

namespace Jugnoon.Utility.Helper
{
    public class Cookie
    {
        public static void WriteCookie(string key, string value)
        {
            SiteConfig.HttpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
        }
        
        public static string ReadCookie(string name)
        {
            if(SiteConfig.HttpContextAccessor.HttpContext.Request.Cookies[name] != null)
            {
                return SiteConfig.HttpContextAccessor.HttpContext.Request.Cookies[name];
            }
          
            return null;
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
