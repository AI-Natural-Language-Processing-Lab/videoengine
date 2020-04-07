using Jugnoon.Framework;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Jugnoon.BLL;
using System.Text;
using Jugnoon.Utility;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Models;
/// <summary>
/// Category Filtering API - Business Layer Designed for Videos | Audio Files. 
/// It support filtering videos or audio files based on category name, categoryid, or array of category lists.
/// It supports only public videos but you can extend its filter logics based on your requirements.
/// </summary>
namespace Jugnoon.Videos
{
    public class CategorizeVideos
    {       
        public static async Task<List<JGN_Videos>> LoadItems(ApplicationDbContext context, VideoEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0 
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return await Load_Raw(context, entity);
            }
            else
            {
                string key = VideoBLL.GenerateKey("lg_video_cat_", entity);
                var data = new List<JGN_Videos>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = await Load_Raw(context, entity);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<JGN_Videos>)SiteConfig.Cache.Get(key);
                }
                return data;
            }
        }
        private static Task<List<JGN_Videos>> Load_Raw(ApplicationDbContext context, VideoEntity entity)
        {
            return VideoBLL.processOptionalConditions(prepareQuery(context, entity), entity)
                   .Select(VideoBLL.prepareSummaryList()).ToListAsync();
        }

        public static Task<int> Count(ApplicationDbContext context, VideoEntity entity)
        {
            return prepareQuery(context, entity).CountAsync();
        }

        private static IQueryable<VideoQueryEntity> prepareQuery(ApplicationDbContext context, VideoEntity entity)
        {
            return context.JGN_Videos
             .Join(context.AspNetusers,
                  video => video.userid,
                  user => user.Id, (video, user) => new { video, user })
             .Join(context.JGN_CategoryContents,
                 video => video.video.id,
                 video_category => video_category.contentid, (video, video_category) => new { video, video_category })
             .Join(context.JGN_Categories,
                 video_category => video_category.video_category.categoryid,
                 category => category.id, (video_category, category) => 
                 new VideoQueryEntity { 
                     video = video_category.video.video, 
                     video_category = video_category.video_category,
                     category = category,
                     user = video_category.video.user
                 })
             .Where(VideoBLL.returnWhereClause(entity));
        }

    }

   
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
