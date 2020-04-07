using Jugnoon.Framework;
using System;
using System.Collections.Generic;

namespace Jugnoon.Models
{
    public class InfoViewModel
    {
        public string PostedCaption { get; set; } = "Posted on";
        /// <summary>
        /// Filter data based on 0: Video, 1: Audio
        /// </summary>
        public int MediaType { get; set; } = 0;

        /// <summary>
        /// Id of actual post (video | photo | audio etc)
        /// </summary>
        public long ContentID { get; set; } = 0;

        /// <summary>
        /// Content author info object
        /// </summary>
        public ApplicationUser Author { get; set; }

        /// <summary>
        /// Content category info object (Included detail information for all categories associated with content)
        /// </summary>
        public List<JGN_CategoryContents> Category { get; set; }

        /// <summary>
        /// Content detail description / information
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Content tags information (comma separated list of tags or labels associated with content)
        /// </summary>
        public string Tags { get; set; } = "";

        /// <summary>
        /// Content creation data information
        /// </summary>
        public DateTime Created_at { get; set; } = new DateTime();

        /// <summary>
        /// Content rating stats (Liked)
        /// </summary>
        public int Liked { get; set; } = 0;

        /// <summary>
        /// Content rating stats (DisLiked)
        /// </summary>
        public int Disliked { get; set; } = 0;

        /// <summary>
        /// Content rating stats (Average Rating) If enabled instead of liked / disliked
        /// </summary>
        public float Average_Rating { get; set; } = 0;

        /// <summary>
        /// Content rating stats (Total Ratings) If enabled instead of liked / disliked
        /// </summary>
        public int Total_Ratings { get; set; } = 0;

        /// <summary>
        /// Content directory path (routing paths) used for redirect to content specific pages e.g /videos/ for video categories and so on
        /// </summary>
        public string Path { get; set; } = "";

    }
}


/*
    * This file is subject to the terms and conditions defined in
    * file 'LICENSE.md', which is part of this source code package.
    * Copyright 2007 - 2020 MediaSoftPro
    * For more information email at support@mediasoftpro.com
 */
