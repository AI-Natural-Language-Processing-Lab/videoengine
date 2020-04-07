namespace Jugnoon.Settings
{
    public class Media
    {
        /// <summary>
        /// Set thumbnail width for user avators
        /// </summary>
        public short user_thumbnail_width { get; set; }

        /// <summary>
        /// Set thumbnail height for user avators
        /// </summary>
        public short user_thumbnail_height { get; set; }

        /// <summary>
        /// Set category thumbnail width
        /// </summary>
        public short category_thumbnail_width { get; set; }

        /// <summary>
        /// Set category thumbnail height
        /// </summary>
        public short category_thumbnail_height { get; set; }

        /// <summary>
        /// Set gamify badge width
        /// </summary>
        //public short gamify_badge_width { get; set; }

        /// <summary>
        /// Set gamify badge height
        /// </summary>
        //public short gamify_badge_height { get; set; }

        /// <summary>
        /// Setup thumbnail quality in percentage e.g 70%
        /// </summary>
        public short quality { get; set; }

        /// <summary>
        /// Setup default logo path
        /// </summary>
        public string logo_path { get; set; }

        /// <summary>
        /// Setup default user avator path (to be used if user not updated its avator)
        /// </summary>
        public string user_default_path { get; set; }

        /// <summary>
        /// Setup default category image path (to be used if category have no photo)
        /// </summary>
        public string category_default_path { get; set; }

        /// <summary>
        /// Setup default gamify badge image path (to be used if badge have no image)
        /// </summary>
        // public string gamify_default_path { get; set; }

        /// <summary>
        /// Allowed photo extension to be uploaded to website
        /// </summary>
        public string photo_extensions { get; set; }

        /// <summary>
        /// Maximum allowed image size (in mb e.g 11mb)
        /// </summary>
        public string photo_max_size { get; set; }
    }
}


/*
    * This file is subject to the terms and conditions defined in
    * file 'LICENSE.md', which is part of this source code package.
    * Copyright 2007 - 2020 MediaSoftPro
    * For more information email at support@mediasoftpro.com
 */
