
using Jugnoon.Framework;
namespace Jugnoon.Models
{
    public class CommentViewModel
    {  
        /// <summary>
        ///  0: Disabled comments on selected post, 1: Enabled comments on selected post
        /// </summary>
        public int isComment { get; set; } = 0;

        /// <summary>
        ///  Post identity (ID)
        /// </summary>
        public long ContentID { get; set; } = 0;
        /// <summary>
        ///  Content Author Info Object
        /// </summary>
        public ApplicationUser Author {get;set;}

        /// <summary>
        ///  Canonical Url of Post
        /// </summary>
        public string Url { get; set; } = "";
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
