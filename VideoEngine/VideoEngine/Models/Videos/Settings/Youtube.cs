
namespace Jugnoon.Videos.Settings
{
    public class Youtube
    {
        /// <summary>
        /// Toggle on | off youtube functionality for videos
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off youtube video processing for administrators (control panel)
        /// </summary>
        public bool enable_admin { get; set; }

        /// <summary>
        /// Register youtube api key generated via google developer console.
        /// </summary>
        public string key { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
