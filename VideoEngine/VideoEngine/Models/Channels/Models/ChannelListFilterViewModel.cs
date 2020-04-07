
using Jugnoon.Models;

namespace Jugnoon.Channels.Models
{
    public class ChannelListFilterViewModel : ListFilterViewModel
    {
        // order dropdown urls
        public string order_recent_url { get; set; }
        public string order_view_url { get; set; }
        
        // filter dropdown urls
        public string filter_date_today_url { get; set; }
        public string filter_date_thisweek_url { get; set; }
        public string filter_date_thismonth_url { get; set; }
        public string filter_date_alltime_url { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
