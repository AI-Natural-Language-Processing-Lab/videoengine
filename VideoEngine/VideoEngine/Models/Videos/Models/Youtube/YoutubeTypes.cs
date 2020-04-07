
using System.Collections.Generic;

namespace Jugnoon.Videos.Models
{
    public class YoutubeTypes
    {
        public static Dictionary<string, string> OrderList()
        {
            var aTypes = new Dictionary<string, string>();
            aTypes.Add("Relevance", "Relevance");
            aTypes.Add("ViewCount", "ViewCount");
            aTypes.Add("Published", "Date");
            aTypes.Add("Rating", "Rating");
            aTypes.Add("Title", "Title");
            return aTypes;
        }

        public static Dictionary<string, string> DateList()
        {
            var aTypes = new Dictionary<string, string>();
            aTypes.Add("All Time", "3");
            aTypes.Add("Today", "0");
            aTypes.Add("This Week", "1");
            aTypes.Add("This Month", "2");
            return aTypes;
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */



