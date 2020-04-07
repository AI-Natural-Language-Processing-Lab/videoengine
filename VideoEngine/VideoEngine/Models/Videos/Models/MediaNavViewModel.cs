
using Jugnoon.Models;

namespace Jugnoon.Videos.Models
{   
    public class MediaNavViewModel: NavViewModel
    {
        public int MediaType { get; set; }
        public string Term { get; set; } // work on tags
        public string[] Categories { get; set; }
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
