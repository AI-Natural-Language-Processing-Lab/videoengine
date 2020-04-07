using Jugnoon.Framework;
using Jugnoon.Settings;

namespace Jugnoon.Utility
{
    public class CategoryUrlConfig
    {
        /// <summary>
        /// Prepare and return category page link
        /// </summary>
        /// <param name="term"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PrepareUrl(string term, string path)
        {
            if (term == null || term == "")
                return "#";

            string query = UtilityBLL.ReplaceSpaceWithHyphin_v2(term.Trim().ToLower());

            return Config.GetUrl(path + "category/" + query);
        }

        /// <summary>
        /// Prepare and return category image / thumbnail link
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string PrepareImageUrl(JGN_Categories entity)
        {
            if (entity.picturename == null || entity.picturename == "" || entity.picturename == "none")
                return getDefaultImageUrl();
            else if (entity.picturename.StartsWith("http"))
                return entity.picturename;
            else
                return Config.GetUrl("contents/category/" + entity.picturename);
        }

        /// <summary>
        /// Prepare and return category page link
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PrepareUrl(JGN_Categories entity, string path)
        {
            string _value = entity.term;
            if (_value == "")
                _value = entity.title;
            string query = UtilityBLL.ReplaceSpaceWithHyphin_v2(_value.Trim().ToLower());

            return Config.GetUrl(path + "category/" + query);
        }

        /// <summary>
        /// Prepare and return browse all link for categories
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string BrowseAllUrl(string path)
        {
            return Config.GetUrl(path + "categories");
        }

        /// <summary>
        /// Prepare and return default category thumbnail link
        /// </summary>
        /// <returns></returns>
        public static string getDefaultImageUrl()
        {
            return Config.GetUrl(Configs.MediaSettings.category_default_path);
        }
    }

    public class TagUrlConfig
    {
        /// <summary>
        /// Prepare tag or label page link
        /// </summary>
        /// <param name="term"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PrepareUrl(string term, string path)
        {
            if (term == null)
                return "#";

            string query = UtilityBLL.ReplaceSpaceWithHyphin_v2(term.Trim().ToLower());
            return Config.GetUrl(path + "label/" + query);
        }

        /// <summary>
        /// Prepare tag or label page link
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PrepareUrl(JGN_Tags entity, string path)
        {
            var _tag = entity.term;
            if (_tag == "")
                _tag = entity.title;
            string query = UtilityBLL.ReplaceSpaceWithHyphin_v2(_tag.Trim().ToLower());

            return Config.GetUrl(path + "label/" + query);
        }

        /// <summary>
        /// Prepare and return browse all link for tags
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string BrowseAllUrl(string path)
        {
            return Config.GetUrl(path + "labels");
        }

    }

    public class ArchiveUrlConfig
    {
        /// <summary>
        /// Prepare and return archive link
        /// </summary>
        /// <param name="monthname"></param>
        /// <param name="year"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PrepareUrl(string monthname, int year, string path = "")
        {
            return Config.GetUrl(path + "archive/" + monthname.ToLower() + "/" + year);
        }

        /// <summary>
        /// Prepare and return browse all link for archive
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string BrowseAllUrl(string path)
        {
            return Config.GetUrl(path + "archive");
        }

    }

    public class UrlConfig
    {
        public static string UnsubscribeUrl { get; set; } = SiteConfiguration.URL + "home/unsubscribe";

        public static string Upload_Path(string foldername)
        {
           return SiteConfig.Environment.ContentRootPath + "/wwwroot/contents/" + foldername;
        }
        public static string Upload_Path(string username, string foldername)
        {
            return SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, username) + foldername;
        }

        public static string Upload_URL(string username, string foldername)
        {
            return Config.GetUrl() + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserUrlPath, username) + foldername;
        }

        public static string Photo_Upload_Path(string username, int type)
        {
            // type: 0 --/ Default
            // type: 1 --/ Mid thumb
            // type: 2 --/ Thumb
            string pathsettings = "";
            switch (type)
            {
                case 1:
                    pathsettings = "midthumbs";
                    break;
                case 2:
                    pathsettings = "thumbs";
                    break;
            }
            return SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, username) + "photos" + pathsettings;
        }
    }
    
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
