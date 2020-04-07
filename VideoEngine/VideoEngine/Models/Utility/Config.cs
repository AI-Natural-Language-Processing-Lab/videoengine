using System.Text;


namespace Jugnoon.Utility
{
    public class Config
    {

        #region General Settings
        /// <summary>
        /// Generate current website url
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return SiteConfiguration.URL;
        }

        /// <summary>
        /// Generate current website url
        /// </summary>
        /// <returns></returns>
        public static string GetUrl(string section)
        {
            string _url = GetUrl();
            if (_url.EndsWith("/") || _url.EndsWith(@"\"))
                return _url + "" + section;
            else
                return _url + "/" + section;
        }

        public static int Calculate_Ratings(double rating)
        {
            return (int)rating * 24;
        }
     
        #endregion

        #region Message Layouts

        /// <summary>
        /// Display message with close link, by default 
        /// </summary>
        public static string SetHiddenMessage_v2(string message, string elementid, bool isvisible, int messagetype)
        {
            // type: 4 normal
            var str = new StringBuilder();
            if (messagetype == 4)
            {
                str.AppendLine("<div class=\"panel panel-default panelsm\">\n<div class=\"panel-body\">\n");
                str.AppendLine(message);
                str.AppendLine("</div></div>\n");
            }
            else
            {
                string css = "ajax_out_box ui-corner-all";
                switch (messagetype)
                {
                    case 0:
                        css = "alert alert-danger alert-dismissible";
                        break;
                    case 1:
                        css = "alert alert-success alert-dismissible";
                        break;
                    case 2:
                        css = "alert alert-info alert-dismissible";
                        break;
                    case 3:
                        css = "alert alert-block";
                        break;
                }
               
                string hidden = "";
                if (isvisible == false)
                    hidden = "style=\"display:none;\"";
                                
                str.AppendLine("<div id=\"" + elementid + "\" " + hidden + " class=\"" + css + "\" role =\"alert\">");
                str.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>");
                str.AppendLine(message);
                str.AppendLine("</div>");
            }
            return str.ToString();
        }

        #endregion
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
