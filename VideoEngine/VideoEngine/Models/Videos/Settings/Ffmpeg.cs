
namespace Jugnoon.Videos.Settings
{
    public class Ffmpeg
    {
        /// <summary>
        /// Toggle on | off ffmpeg functionality for videos
        /// </summary>
        public bool enable { get; set; }

        /// <summary>
        /// Toggle on | off ffmpeg video processing for administrators (control panels)
        /// </summary>
        public bool enable_admin { get; set; }

        /// <summary>
        /// Setup publishing approach -  0: Sheduled Publishing, 1: Direct Publishing
        /// </summary>
        public byte video_Publishing_Type { get; set; }

        /// <summary>
        /// Ffmpeg commands specific to mp4 240p transcoding
        /// </summary>
        public string mp4_240p_Settings { get; set; }

        /// <summary>
        /// Ffmpeg commands specific to mp4 360p transcoding
        /// </summary>
        public string mp4_360p_Settings { get; set; }

        /// <summary>
        /// Ffmpeg commands specific to mp4 480p transcoding
        /// </summary>
        public string mp4_480p_Settings { get; set; }

        /// <summary>
        /// Ffmpeg commands specific to mp4 720p transcoding
        /// </summary>
        public string mp4_720p_Settings { get; set; }

        /// <summary>
        /// Ffmpeg commands specific to mp4 1080p transcoding
        /// </summary>
        public string mp4_1080p_Settings { get; set; }

        /// <summary>
        /// Preferred encoding options for encoding (0: 240p, 1: 360p, 2: 480p, 3: 720p, 4: 1080p)
        /// </summary>
        public short encoding_options { get; set; }

        /// <summary>
        /// Ffmpeg utility path for transcoding
        /// </summary>
        public string ffmpeg_path { get; set; }

        /// <summary>
        /// Mp4box utility path for adding meta information to mp4 videos
        /// </summary>
        public string mp4box_path { get; set; }
        
        /// <summary>
        /// Toggle on | off publishing video clip for preview purpose
        /// </summary>
        public bool enable_clips { get; set; }

        /// <summary>
        /// Setup clip length in seconds for videos
        /// </summary>
        public short clip_length { get; set; }

        /// <summary>
        /// Toggle on | off generating small preview video
        /// </summary>
        public bool enable_preview_video { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
