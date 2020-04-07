using System;
using Jugnoon.BLL;

namespace Jugnoon.Videos.Settings
{
    public class General
    {
        /// <summary>
        /// Setup video extensions to be allowd for uploading.
        /// </summary>
        public string extensions { get; set; }

        /// <summary>
        /// Setup maximum video size for uploading videos
        /// </summary>
        public string max_size { get; set; }

        /// <summary>
        /// Maximum no of videos uploaded at once
        /// </summary>
        public string max_concurrent_uploads { get; set; }
               

        /// <summary>
        /// Toggle on | off uploading original videos
        /// </summary>
        public bool delete_original { get; set; }

        /// <summary>
        /// Enable type of uploading option if available for users (0: ffmpeg, 1: youtube, 2: direct, 3: AWS)
        /// </summary>
        public byte videoUploader_Type { get; set; }

        /// <summary>
        /// nable normal users to upload videos from his / her own account or restrict this functionality to admin only
        /// </summary>
        public bool enable_public_uploads { get; set; }

        /// <summary>
        /// Toggle on | off video playlist functionality if module available
        /// </summary>
        public bool enable_playlists { get; set; }

        /// <summary>
        /// Toggle on | off video download functionality
        /// </summary>
        public bool enable_download { get; set; }

        /// <summary>
        /// Toggle on | off add to favorite functionality
        /// </summary>
        public bool enable_favorites { get; set; }

        /// <summary>
        /// Toggle on | off thumbnail rotator functionality ( 1: No Rotator, 0: Thumbs Rotator, 2: Video Preview Rotator)
        /// </summary>
        public byte thumbnail_rotator_option { get; set; }

        /// <summary>
        /// Setup default picture path that will be used as default image or video cover in listings if no photo available
        /// </summary>
        public string default_path { get; set; }

        /// <summary>
        /// Default thumbnail width for videos (used when generating photos or updating thumbnails)
        /// </summary>
        public short thumbnail_width { get; set; }

        /// <summary>
        /// Default thumbnail height for videos (used when generating photos or updating thumbnails)
        /// </summary>
        public short thumbnail_height { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
