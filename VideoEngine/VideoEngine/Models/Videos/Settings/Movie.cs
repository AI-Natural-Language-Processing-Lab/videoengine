
/* If AWS Settings Enabled, It will be shared and used by Movie Functionality by Default */

namespace Jugnoon.Videos.Settings
{
    public class Movie
    {
        /// <summary>
        /// Toggle on | off direct video functionality for videos
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off movie video processing for administrators (control panel)
        /// </summary>
        public bool enable_admin { get; set; }

        /// <summary>
        /// Toggle on | off direct embed video functionality for videos
        /// </summary>
        public bool enabled_embed { get; set; }
        
        /// <summary>
        /// Toggle on | off direct embed video functionality for administrators (control panel)
        /// </summary>
        public bool enable_embed_admin { get; set; }
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
