using System;
using System.Text;

namespace Jugnoon.Utility
{
    public enum AlertTypes
    {
        // Error
        Error = 0,
        // Success
        Success = 1,
        // Warning
        Warning = 2,
        // Info
        Info = 3
    }

    public class Alerts
    {
        /// <summary>
        /// Utility script to generate bootstrap alert via script dynamically
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Types"></param>
        /// <returns></returns>
        public static string Prepare(string Message, AlertTypes Types)
        {
            var str = new StringBuilder();
            string alertCss = "alert-danger";
            switch ((int)Types)
            {
                case 0:
                    // error
                    alertCss = "alert-danger";
                    break;
                case 1:
                    // success
                    alertCss = "alert-success";
                    break;
                case 2:
                    // warning
                    alertCss = "alert-warning";
                    break;
                case 3:
                    // info
                    alertCss = "alert-info";
                    break;
            }
            str.Append("<div class=\"alert " + alertCss + "\">\n");
            str.Append(Message);
            str.Append("</div>");

            return str.ToString();
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
