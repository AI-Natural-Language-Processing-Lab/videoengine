
namespace Jugnoon.Videos.Settings
{
    public class Aws
    {        
        /// <summary>
        /// Toggle on | off aws video processing for normal users
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off aws video processing for administrators (control panels)
        /// </summary>
        public bool enable_admin { get; set; }

        /// <summary>
        /// Setup bucketname for storing photos
        /// </summary>
        public string bucket { get; set; }

        /// <summary>
        /// Setup directory for saving uploaded videos e.g videos/. It will be mostly used for directory where no aws events attached like Elastic Transcoder for auto publishing
        /// </summary>
        public string source_directory_path { get; set; }

        /// <summary>
        /// Setup directory (within bucket) for saving published videos e.g published/
        /// </summary>
        public string publish_directory_path { get; set; }

        /// <summary>
        /// Setup directory (within bucket) for saving generated video thumbnails e.g thumbnails/
        /// </summary>
        public string thumbnail_directory_path { get; set; }

        /// <summary>
        /// Setup directory for using uploading source videos that trigger elastic transcoder even for publishing videos.
        /// </summary>
        public string elastic_transcoder_directory { get; set; }

        /// <summary>
        /// Setup public video url (cloudfront url)
        /// </summary>
        public string public_url { get; set; }

        /// <summary>
        /// Setup private video url (cloudfront url)
        /// </summary>
        public string private_url { get; set; }

        /// <summary>
        /// Setup cloudfront keypair required for generating signed url for protected videos
        /// </summary>
        public string cloudFront_keypair { get; set; }

        /// <summary>
        /// Setup cloudfront keypair filename path required for generating signed url for protected videos
        /// </summary>
        public string cloudFront_keyfilename { get; set; }

     

        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
