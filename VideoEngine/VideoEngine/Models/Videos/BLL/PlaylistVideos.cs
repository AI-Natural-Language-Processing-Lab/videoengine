using Jugnoon.Entity;
using Jugnoon.Framework;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Jugnoon.Models;

/// <summary>
/// Playlist Filtering API - Business Layer Designed for Videos | Audio Files. 
/// It support filtering videos or audio files based on selected playlist information.
/// It supports only public videos but you can extend its filter logics based on your requirements.
/// </summary>
namespace Jugnoon.Videos
{
    public class PlaylistVideos
    {
        public static Task<List<JGN_Videos>> LoadItems(ApplicationDbContext context, VideoEntity entity)
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
             .Join(context.JGN_Playlist_Videos,
                 video => video.video.id,
                 playlist => playlist.contentid, (video, playlist) => new VideoQueryEntity { video = video.video, user = video.user, playlist = playlist })
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

