
namespace Jugnoon.Settings
{
    public class Aws
    {
        /// <summary>
        /// Toggle on | off aws cloud for storage, hosting and other purposes
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// AWS Access Key required for verification purpose
        /// </summary>
        public string accessKey { get; set; }

        /// <summary>
        /// AWS Secrete Key required for verification purpose
        /// </summary>
        public string secretKey { get; set; }

        /// <summary>
        /// AWS Region (Preferred geolocial location for your content to be stored and stream)
        /// </summary>
        public string region { get; set; }

        /// <summary>
        /// Setup bucketname for saving general media files e.g users or categories etc media
        /// </summary>
        public string bucket { get; set; }

        /// <summary>
        /// Setup directory (within bucket) for saving users avator photos
        /// </summary>
        public string user_photos_directory { get; set; }

        /// <summary>
        /// Setup directory (within bucket) for saving category avator photos
        /// </summary>
        public string category_photos_directory { get; set; }

        /// <summary>
        /// Setup directory (within bucket) for saving gamify badges if functionality available
        /// </summary>
        public string gamify_badges_directory { get; set; }

        /// <summary>
        /// Setup public accessible cloudfront distribution url for streaming photos
        /// </summary>
        public string cdn_URL { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
