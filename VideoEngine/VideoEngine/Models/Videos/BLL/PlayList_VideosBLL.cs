using System;
using System.Linq;
using System.Threading.Tasks;
using Jugnoon.Framework;
using Jugnoon.Utility;
using Microsoft.EntityFrameworkCore;

namespace Jugnoon.Videos
{
    public class PlayList_VideosBLL
    {
        public static async Task<bool> Add(ApplicationDbContext context, long playlid_id, long contentid)
        {
            var _entity = new JGN_Playlist_Videos()
            {
                id = playlid_id,
                contentid = contentid,
                created_at = DateTime.Now
            };

            context.Entry(_entity).State = EntityState.Added;

            await context.SaveChangesAsync();

            // update video statistics
            int count = await PlayList_VideosBLL.Count(context, playlid_id); // count total no of videos in current playlist
            PlayListBLL.Update_Video_Stat(context, playlid_id, count); // update video statistic of play list
            return true;
        }

        // Delete single video from playlist
        public static async Task<bool> Delete(ApplicationDbContext context, long PlayListID, long VideoID)
        {
            var all = from c in context.JGN_Playlist_Videos where c.id == PlayListID && c.contentid == VideoID select c;
            context.JGN_Playlist_Videos.RemoveRange(all);
            await context.SaveChangesAsync();

            // update video statistics
            int count = await PlayList_VideosBLL.Count(context, PlayListID); // count total no of videos in current playlist
            PlayListBLL.Update_Video_Stat(context, PlayListID, count); // update video statistic of play list
            return true;
        }

        // Delete all videos from playlist
        public static async Task<bool> Delete(ApplicationDbContext context, long PlayListID)
        {
            var entity = new JGN_Playlist_Videos { id = PlayListID };
            context.JGN_Playlist_Videos.Attach(entity);
            context.JGN_Playlist_Videos.Remove(entity);
            await context.SaveChangesAsync();

            PlayListBLL.Update_Video_Stat(context, PlayListID, 0); // update video statistic of play list
            return true;
        }

        public static async Task<int> Count(ApplicationDbContext context,long PlayListID)
        {
           return await context.JGN_Playlist_Videos.Where(p => p.id == PlayListID).CountAsync();
        }
        
        public static async Task<bool> Check(ApplicationDbContext context,long PlayListID, long VideoID)
        {
            bool flag = false;
            if (await context.JGN_Playlist_Videos.Where(p => p.id == PlayListID && p.contentid == VideoID).CountAsync() > 0)
               flag = true;

            return flag;
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
