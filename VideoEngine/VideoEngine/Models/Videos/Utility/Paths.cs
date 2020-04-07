
/// <summary>
/// Handling path for saving media files on local server system at time of uploading
/// </summary>
namespace Jugnoon.Videos
{
    public class DirectoryPaths
    {
        /// <summary>
        /// Default directory path for saving uploaded user videos
        /// </summary>
        public static string UserVideosDefaultDirectoryPath { get; set; } = "/wwwroot/contents/member/[USERNAME]/default/";

        /// <summary>
        /// Default directory path for saving uploaded video (generated thumbnails)
        /// </summary>
        public static string UserVideoThumbsDirectoryPath { get; set; } = "/wwwroot/contents/member/[USERNAME]/thumbs/";

    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

