using System;
using Jugnoon.Entity;
using Jugnoon.Utility;
/// <summary>
/// Core Query Builder Entity - Designed for Videos and Audio Files. 
/// </summary>
namespace Jugnoon.Videos
{
    public class VideoEntity : ContentEntity
    {      

        // <summary>
        ///  Filter (videos | audio) based on type flag ( 0: videos, 1: audio, 2: both) (default value = 0 (videos))
        /// </summary>
        public MediaType type { get; set; } = MediaType.Videos;

        // <summary>
        ///  Filter (videos | audio) grouped by album id albumid ( albumid > 0) (default value = 0 (no filter))
        /// </summary>
        public long albumid { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on isprivate flag (0: public, 1: private, 2: unlisted, 3: all) (default value = 0 (only public videos))
        /// </summary>
        public PrivacyActionTypes isprivate { get; set; } = PrivacyActionTypes.Public;

        /// <summary>
        ///  Filter (videos | audio) based on iscomments flag (0: disabled comments, 1: enabled comments, 2: all) (default value = 2 (all videos))
        /// </summary>
        public CommentActionTypes iscomments { get; set; } = CommentActionTypes.All;

        /// <summary>
        ///  Filter (videos | audio) based on israting flag (0: disabled ratings, 1: enabled ratings, 2: all) (default value = 2 (all videos))
        /// </summary>
        public RatingActionTypes israting { get; set; } = RatingActionTypes.All;

        /// <summary>
        ///  Filter (videos | audio) based on ispublished flag (0: not published, 1: published) (default value = 1 (only published videos))
        /// </summary>
        public PublishActionType ispublished { get; set; } = PublishActionType.Published; // 0: not published, 1: published

        /// <summary>
        ///  Filter (videos | audio) based on isexternal flag (0: own videos, 1: external videos)
        /// </summary>                                   
        public MediaSourceType isexternal { get; set; } = MediaSourceType.All; // 0: own videos, 1: external videos

        /// <summary>
        ///  Filter (videos | audio) based on movie type ( 0: videos, 1: movies, 2: all  (default))
        /// </summary>
        public  MovieType movietype { get; set; } = MovieType.All; // 0: videos, 1: movies, 2: all

        /// <summary>
        ///  Filter (videos | audio) based on price ( price > 0, default = 0 (no filter))
        /// </summary>
        public double price { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on associated actors (comma separated list, default "" (no filter))
        /// </summary>
        public string actors { get; set; } = "";

        /// <summary>
        ///  Filter (videos | audio) based on associated actresses (comma separated list,  default "" (no filter))
        /// </summary>
        public string actresses { get; set; } = "";

        /// <summary>
        ///  Filter (videos | audio playlists) based on playlist id (playlistid > 0,  default 0 (no filter))
        /// </summary>
        public int playlistid { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on maxid id (maxid < 0,  default 0 (no filter))
        /// </summary>
        public long maxid { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on minid id (minid > 0,  default 0 (no filter))
        /// </summary>
        public long minid { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on errorcode flag generated at time of publishing videos, 0 for no error (default value = "" (no filter))
        /// </summary>
        public int errorcode { get; set; } = 0;

        /// <summary>
        ///  Filter (videos | audio) based on third party video scripts embed_script (default value = "" (no filter))
        /// </summary>
        public string embed_script { get; set; } = "";

        /// <summary>
        ///  Redirect dataacces layer routing to load playlist records instead of normal records
        ///  Usage case: Load user playlists (videos or audio etc)
        /// </summary>
        public bool loadplaylist { get; set; } = false;

    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
