using Jugnoon.Framework;
using Jugnoon.Utility;

namespace Jugnoon.Videos
{
    public class VideoUrlConfig
    {
        public static string Source_Video_Path()
        {
            return UrlConfig.Upload_Path("default");
        }
        public static string Source_Video_Path(string username)
        {
            return UrlConfig.Upload_Path(username, "default");
        }
        public static string Source_Video_Url(string username)
        {
            return UrlConfig.Upload_URL(username, "default");
        }

        public static string Published_Video_Path()
        {
            return UrlConfig.Upload_Path("flv");
        }
        public static string Published_Video_Path(string username)
        {
            return UrlConfig.Upload_Path(username, "flv");
        }

        public static string Published_Audio_Path(string username)
        {
            return UrlConfig.Upload_Path(username, "mp3");
        }

        public static string MP3_Path()
        {
            return UrlConfig.Upload_Path("mp3");
        }
        public static string MP3_Path(string username)
        {
            return UrlConfig.Upload_Path(username, "mp3");
        }
        public static string Thumbs_Path()
        {
            return UrlConfig.Upload_Path("thumbs");
        }
        public static string Thumbs_Path(string username)
        {
            return UrlConfig.Upload_Path(username, "thumbs");
        }

        public static string Thumb_Url(string username)
        {
            return UrlConfig.Upload_URL(username, "thumbs");
        }


        public static string PrepareUrl(JGN_Videos entity)
        {
            string _title = "";
            if (entity.title == null)
                entity.title = "";
            int maxium_length = Jugnoon.Settings.Configs.GeneralSettings.maximum_dynamic_link_length;
            if (entity.title.Length > maxium_length && maxium_length > 0)
                _title = entity.title.Substring(0, maxium_length);
            else if (entity.title.Length < 3)
                _title = "preview-video";
            else
                _title = entity.title;

            _title = UtilityBLL.ReplaceSpaceWithHyphin_v2(_title.Trim().ToLower());

            return Config.GetUrl("media/" + entity.id + "/" + _title);
        }

        /* Image path to load for unpublished videos */
        public static string get_Unpublished_Media_Url()
        {
            return Config.GetUrl(Jugnoon.Videos.Configs.GeneralSettings.default_path);
        }


        /// <summary>
        /// Generate and return flv video url
        /// </summary>
        public static string Return_FLV_Video_Url(string url, string username)
        {
            if (url == "none" || url == "")
                return Config.GetUrl() + "contents/member/" + username + "/FLV";
            else
            {
                if (Jugnoon.Settings.Configs.AwsSettings.enable)
                {
                    return CloudFront.CreateCannedPrivateURL(url, 60);
                }
                else
                {
                    return url;
                }
            }
        }
        /// <summary>
        /// Generate and return mp3 audio url
        /// </summary>
        public static string Return_MP3_Audio_Url(string url, string username)
        {
            if (url == "none" || url == "")
                return Config.GetUrl() + "contents/member/" + username + "/MP3";
            else
            {
                if (Jugnoon.Settings.Configs.AwsSettings.enable)
                {
                    return CloudFront.CreateCannedPrivateURL(url, 60);
                }
                else
                {
                    return url;
                }
            }
        }
        /// <summary>
        /// Generate and return video thumb url
        /// </summary>
        public static string Return_Video_Thumb_Url(string url, string username)
        {
            if (url == "none" || url == "")
                return Config.GetUrl() + "contents/member/" + username + "/Thumbs";
            else
                return url;
        }

        /// <summary>
        /// Generate and return encoded video physical path
        /// </summary>
        public static string Return_Video_physicalPath(string url, string username, string filename, int type)
        {
            string path = "flv\\";
            switch (type)
            {
                case 0:
                    // original video path
                    path = "default\\";
                    break;
                case 1:
                    // flv video path
                    path = "flv\\";
                    break;
                case 2:
                    // mp3 audio path
                    path = "mp3\\";
                    break;
                case 3:
                    // thumb path
                    path = "thumbs\\";
                    break;
            }
            if (url == "none" || url == "")
                return SiteConfig.Environment.ContentRootPath + "\\contents\\member\\" + username + "\\" + path + "" + filename;
            else
                return url;
        }

    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
