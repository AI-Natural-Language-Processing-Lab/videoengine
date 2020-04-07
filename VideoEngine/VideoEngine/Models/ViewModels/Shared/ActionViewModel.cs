namespace Jugnoon.Models
{
    /// <summary>
    /// Action Module Model View
    /// </summary>
    public class ActionViewModel
    {
        public bool isAuthenticated { get; set; } = false;
        public int ToolbarSize { get; set; } = 1; // 0: mini, 1: normal, 2: large
        public int ToolbarTheme { get; set; } = 0; // 0: default, 1: primary, 2: warning,3: danger, 4: info, 5: inverse, 5: success
        public int Ratingtype { get; set; } = 0; // 0: like / dislike, 1: start rating
        public bool isEmbed { get; set; } = true;
        public bool isPhotoEmbed { get; set; } = false;
        public bool isAlbumEmbed { get; set; } = false;
        public string PhotoUrl { get; set; } = "";
        public bool isPlaylist { get; set; } = true;
        public bool isFavorites { get; set; } = true;
        public bool isShare { get; set; } = true;
        public bool isFlag { get; set; } = true;
        public bool isDownload { get; set; } = true;
        public bool isViews { get; set; } = true;
        public bool isDetailViewStats { get; set; } = true;

        public int isRating = 1; // 1: Rating On, 0: Rating Off
        public long ContentID { get; set; } = 0;
        public string UserName { get; set; } = ""; // userid or username (author)
        public string Current_UserName { get; set; } = ""; // userid or username (logged in user)
        public int Liked { get; set; } = 0;
        public int Disliked { get; set; } = 0;
        /// <summary>
        /// Content type e.g videos, audio (0: video, audio, 1: comment adv points, 2: photo rating , 3: blog rating etc)
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// Media type  // 0: video, 1: audio (only used in video or audio section)
        /// </summary>
        public int MediaType { get; set; } = 0;
        public int Favorites { get; set; } = 0;
        public string VideoPath { get; set; } = "";
        // Filename or downloadable path in case of download 
        public string FileName { get; set;  } = "";
        public string Content_Type { get; set; } = "Video";
        public string Embed_Script { get; set; } = "";
        // Star Ratings
        public double Avg_Ratings { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;
        public double Ratings { get; set; } = 0;
        public int Comments { get; set; } = 0;
        public int Views { get; set; } = 0;
             
        // --> Custom Button Options --> Only required for photo galleries
        public bool PhotoGalleryOptions { get; set; } = false; // Enable photo gallery custom option buttons, (normal, original, slideshow)
        public bool ShowOriginalButton { get; set; } = false;
        public bool ShowNormalButton { get; set; } = false;
        public bool ShowSlideShowButton { get; set; } = false;
        public string GalleryDefaultLink { get; set; } = "";

        // --> Custom Button Options --> Only required for photo preview page
        public bool PhotoOptions { get; set; } = false;
        public string PhotoDownloadLink { get; set; } = "";

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
