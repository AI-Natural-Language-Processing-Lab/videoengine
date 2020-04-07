/* If AWS Settings Enabled, It will be shared and used by Movie Functionality by Default */

namespace Jugnoon.Videos.Settings
{
    public class Direct
    {
        /// <summary>
        /// Toggle on | off direct video functionality for videos (for normal users)
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off direct video processing for administrators (control panels)
        /// </summary>
        public bool enable_admin { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
