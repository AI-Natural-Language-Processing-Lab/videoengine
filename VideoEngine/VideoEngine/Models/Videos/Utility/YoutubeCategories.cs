using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using Jugnoon.BLL;
using Google.Apis.YouTube.v3.Data;
using Jugnoon.Framework;

namespace Jugnoon.Videos
{
    public class YoutubeCategories
    {
        public List<Yt_Category> GetVideoCategories(ApplicationDbContext context)
        {
            var _list = new List<Yt_Category>();
            YouTubeService objYouTubeService = default(YouTubeService);
           
            try
            {                
                objYouTubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = Videos.Configs.YoutubeSettings.key,
                    ApplicationName = this.GetType().ToString()
                });
            }
            catch (Exception ex)
            {
                ErrorLgBLL.Add(context, "Exception Youtube Service Init", "", ex.Message);
            }

            VideoCategoryListResponse objCategories = null;
            try
            {
                var objRequest = objYouTubeService.VideoCategories.List("id,snippet");
                objRequest.Hl = "en_US";
                objRequest.RegionCode = "US";
                objCategories = objRequest.Execute();
            }
            catch (Exception ex)
            {
                ErrorLgBLL.Add(context, "Exception Youtube List Request", "", ex.Message);
            }
            
            foreach (var obj in objCategories.Items)
            {
                _list.Add(new Yt_Category()
                {
                    ID = obj.Id,
                    Title = obj.Snippet.Title
                });

            }
            return _list;
        }
    }

    public class Yt_Category
    {
        public string ID { get; set; }
        public string Title { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
