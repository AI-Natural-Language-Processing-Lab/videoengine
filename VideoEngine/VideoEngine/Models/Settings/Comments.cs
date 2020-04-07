
namespace Jugnoon.Settings
{
    public class Comments
    {
        /// <summary>
        /// Enable comment application for posts within application
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Choose your comment app from list of comment applications (0: website own, 1: disqus, 2: facebook comments)
        /// </summary>
        public byte comment_option { get; set; }

        /// <summary>
        /// If disqus option enabled, please put your discus src your setup via Disqus Dashboard
        /// </summary>
        public string discus_src { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
