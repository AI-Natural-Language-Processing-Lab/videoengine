using System.Text;
using Jugnoon.Utility;
using System.Net;
using Jugnoon.Framework;
using Jugnoon.Scripts;

namespace Jugnoon.Videos
{
    public class VideoUtil
    {

        // Main function used to process core video thumb stored on local server, remote server, cloud storage or third party storage e.g youtube video thumbs
        public static string ProcessVideoThumb(string url, JGN_Videos vd, bool isaudio, ListItems attr)
        {
            // media thumb section
            var str = new StringBuilder();
            var img_str = new StringBuilder();
            // isExternal = 3; // Default Value for Query Processing. If 3 is set then use default option
            if (vd.isexternal == 0 || vd.isexternal == 3)
            {
                if (vd.ispublished == 0 || vd.errorcode > 0 || vd.thumbfilename == "" || vd.thumbfilename == "none")
                {
                    // video is not yet published
                    // or published failed - any level of error raised
                    // or thumbfilename is empty
                    vd.thumb_url = SiteConfiguration.URL + Videos.Configs.GeneralSettings.default_path;
                    str.Append(Process_Embed_Video_Thumb(img_str, vd.thumb_url, attr));
                }
                else
                {
                    // normal video upload
                    str.Append(Process_Normal_Video_Thumb(img_str, vd.userid, vd.thumbfilename, vd.thumb_url, attr));
                }
            }
            else if (vd.isexternal == 1)
            {
                // embed video upload
                str.Append(Process_Embed_Video_Thumb(img_str, vd.thumb_url, attr));
            }
            else if (vd.isexternal == 2)
            {
                // process both embed and upload thumb
                if (vd.embed_script != "")
                    str.Append(Process_Embed_Video_Thumb(img_str, vd.thumb_url, attr));
                else
                {
                    if (vd.ispublished == 0 || vd.errorcode > 0 || vd.thumbfilename == "" || vd.thumbfilename == "none")
                    {
                        // video is not yet published
                        // or published failed - any level of error raised
                        // or thumbfilename is empty
                        vd.thumb_url = SiteConfiguration.URL + Videos.Configs.GeneralSettings.default_path;
                        str.Append(Process_Embed_Video_Thumb(img_str, vd.thumb_url, attr));
                    }
                    else
                    {
                        str.Append(Process_Normal_Video_Thumb(img_str, vd.userid, vd.thumbfilename, vd.thumb_url, attr));
                    }
                }
            }
            return str.ToString().Replace("\n", "").Replace("\r", "");
        }

        private static string Process_Embed_Video_Thumb(StringBuilder img_str, string thumburl, ListItems attr)
        {
            if (attr.isresize)
            {
                string _size = "800x600";
                if (attr.size != "")
                    _size = attr.size;

                img_str.Append(Config.GetUrl("handler/resize?file=" + WebUtility.UrlEncode(thumburl) + "&size=" + _size));
            }
            else
            {
                img_str.Append(thumburl);
            }

            return img_str.ToString();
        }

        private static string Process_Normal_Video_Thumb(StringBuilder img_str, string username, string mediapath, string thumburl, ListItems attr)
        {
            if (!attr.isresize)
            {
                // no resize needed
                string MediaName = mediapath;
                if (MediaName.Contains("_"))
                {
                    string thumb_start_index = MediaName.Remove(MediaName.LastIndexOf("_"));
                    string _thumb_url = VideoUrlConfig.Return_Video_Thumb_Url(thumburl, username);
                    string thumb_start_sequence_path = "";
                    string imagepath = "";
                    if (thumburl == "" || thumburl == "none")
                    {
                        // default thumb fetching
                        thumb_start_sequence_path = _thumb_url + "/" + thumb_start_index + "_";
                        imagepath = _thumb_url + "/" + MediaName;
                    }
                    else
                    {
                        // cloud thumb fetching
                        if (_thumb_url.Contains("_"))
                            thumb_start_sequence_path = _thumb_url.Remove(_thumb_url.LastIndexOf("_")) + "_";
                        imagepath = _thumb_url;
                    }

                    img_str.Append(imagepath);

                }
            }
            else
            {
                // resizable thumbs
                string _filename = "";
                int _iscloud = 0;
                if (thumburl != "")
                {
                    _iscloud = 1;
                    _filename = thumburl;
                }
                else
                {
                    // generate filename
                    string _MediaName = mediapath;
                    string thumb_start_index = _MediaName.Remove(_MediaName.LastIndexOf("_"));
                    string _thumb_url = VideoUrlConfig.Return_Video_Thumb_Url(thumburl, username);
                    string thumb_start_sequence_path = "";
                    if (_thumb_url.Contains("_"))
                        thumb_start_sequence_path = _thumb_url.Remove(_thumb_url.LastIndexOf("_")) + "_";
                    _filename = _thumb_url;
                }

                string _size = "800x600";
                if (attr.size != "")
                    _size = attr.size;

                img_str.Append(Config.GetUrl("videos/resize?u=" + username + "&file=" + WebUtility.UrlEncode(thumburl) + "&cloud=" + _iscloud + "&size=" + _size));
            }

            return img_str.ToString();
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

