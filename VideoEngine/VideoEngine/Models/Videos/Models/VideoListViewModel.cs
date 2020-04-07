using Jugnoon.Framework;
using Jugnoon.Models;
using System.Collections.Generic;

namespace Jugnoon.Videos.Models
{
    public class VideoListViewModel : ListViewModel
    {
        public int TotalRecords { get; set; }

        public List<JGN_Videos> DataList { get; set; }

        public VideoEntity QueryOptions { set; get; }

        public VideoListFilterViewModel Navigation { get; set; }
        
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
