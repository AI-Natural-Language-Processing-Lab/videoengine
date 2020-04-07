using Jugnoon.Models;

namespace Jugnoon.Utility
{
    public class UserUrlConfig
    {
        /// <summary>
        /// Generate user profile link
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string ProfileUrl(ApplicationUser entity, byte option)
        {
            if (entity == null)
                return "#";

            var username = entity.Id;
            if (option == 0)
            {
                if (entity.UserName != null)
                    username = entity.UserName.ToLower();
            }

            if (username == "")
                username = "user" + entity.Id;

            return Config.GetUrl() + "user/" + username.ToLower();
        }

        /// <summary>
        /// Generate user profile sub section links
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="option"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static string ProfileUrl(ApplicationUser entity, byte option, string section)
        {
            var username = entity.Id;
            if (option == 0)
                username = entity.UserName.ToLower();

            if (username == "")
                username = "user" + entity.Id;

            if (section != "")
                section = section + "/";

            return Config.GetUrl() + "user/" + section + "" + username.ToLower();
        }

        /// <summary>
        /// Prepare user name based on registeration options set
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string PrepareUserName(ApplicationUser entity, byte option)
        {
            var _name = "User";
            if (entity.firstname != "")
                _name = entity.firstname + " " + entity.lastname;
            else
            {
                if (option == 0)
                    _name = entity.UserName;
                else
                {
                    _name = "User";
                }
            }
            return _name;
        }

        /// <summary>
        /// Prepare and return user profile photo link
        /// </summary>
        /// <param name="username"></param>
        /// <param name="picturename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ProfilePhoto(string username, string picturename, int type)
        {
            if (username == null)
                return "";

            string URL = "";
            if (picturename == null || picturename == "" || picturename == "none")
                URL = GetDefaultPhoto();
            else if (picturename.StartsWith("http")) // media stored in cloud storage
                URL = PrepareHttpPhotoUrl(picturename, type);
            else
            {
                // type = 0: thumb, 1: mid thumb, 2: original
                string Imagetype = ""; // original
                switch (type)
                {
                    case 0:
                        Imagetype = "thumbs/";
                        break;
                    case 1:
                        Imagetype = "midthumbs/";
                        break;
                }
                URL = Config.GetUrl(UtilityBLL.ParseUsername(SystemDirectoryPaths.UserUrlPath, username)) + "/ " + Imagetype + "" + picturename;
            }

            return URL;
        }

        /// <summary>
        /// Prepare and set user profile link
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string PrepareHttpPhotoUrl(string filename, int type)
        {
            string URL = "";
            switch (type)
            {
                case 0:
                    // thumb path
                    URL = filename;
                    break;
                case 1:
                    URL = filename;
                    break;
                case 2:
                    URL = filename.Replace("thumbs/", "");
                    break;
                case 3:
                    URL = filename.Replace("thumbs/", "midthumbs/");
                    break;
            }

            return URL;
        }

        /// <summary>
        /// Prepare user default profile image link
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPhoto()
        {
            return Config.GetUrl() + Settings.Configs.MediaSettings.user_default_path;
        }
    }

}