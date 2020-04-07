using System;
using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Jugnoon.BLL;

namespace Jugnoon.Videos
{
    public class PlayListBLL
    {
        // Note: PlayList Important Terms

        // isEnabled:
        // ........... 0: Disabled PlayList
        // ........... 1: Enable PlayList

        // isapproved:
        // ........... 0: Not approved
        // ........... 1: Approved

        // Privacy:
        // ........... 0: Private
        // ........... 1: Public


        // Videos:
        // Store Statistic for no of videos available in playlist

        public static async Task<bool> Add(ApplicationDbContext context,string userid, string title)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                title = DictionaryBLL.Process_Screening(context, title);
            }

            var _entity = new JGN_User_Playlists()
            {
                userid = userid,
                title = title,
                created_at = DateTime.Now
            };

            context.Entry(_entity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return true;
        }

        public static async Task<bool> Update(ApplicationDbContext context,long PlayListID, string Title, string Description, string Tags, int Privacy)
        {
            var item = context.JGN_User_Playlists
                    .Where(p => p.id == PlayListID)
                    .FirstOrDefault<JGN_User_Playlists>();

            if(item != null)
            {
                item.title = UtilityBLL.processNull(Title, 0);
                item.description = UtilityBLL.processNull(Description, 0);
                item.tags = Tags;
                item.privacy = (byte)Privacy;

                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context, long playlid_id)
        {
            await PlayList_VideosBLL.Delete(context, playlid_id);
            var entity = new JGN_User_Playlists { id = playlid_id };
            context.JGN_User_Playlists.Attach(entity);
            context.JGN_User_Playlists.Remove(entity);
            await context.SaveChangesAsync();
            
            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context, long playlid_id, string userid)
        {
            var entity = new JGN_User_Playlists { id = playlid_id, userid = userid };
            context.JGN_User_Playlists.Attach(entity);
            context.JGN_User_Playlists.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public static string GetTitle(ApplicationDbContext context, long playlid_id)
        {
            string Value = "";
            var item = context.JGN_User_Playlists
                    .Where(p => p.id == playlid_id)
                    .FirstOrDefault<JGN_User_Playlists>();

            Value = item.title;

            return Value;

        }

        // update videos statistics, used to show no of videos available in current play list.
        public static bool Update_Video_Stat(ApplicationDbContext context, long playlid_id, int Videos)
        {
            var item = context.JGN_User_Playlists
                    .Where(p => p.id == playlid_id)
                    .FirstOrDefault<JGN_User_Playlists>();

            if(item != null)
            {
                item.videos = Videos;
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return true;
        }

        // enable, disable playlist
        public static bool Update_isEnabled(ApplicationDbContext context, long playlid_id, int value)
        {
            var item = context.JGN_User_Playlists
                    .Where(p => p.id == playlid_id)
                    .FirstOrDefault<JGN_User_Playlists>();

            if(item != null)
            {
                item.isenabled = (byte)value;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
         
            return true;
        }

        // enable, disable playlist
        public static bool Update_isApproved(ApplicationDbContext context, long playlid_id, int value)
        {
                var item = context.JGN_User_Playlists
                      .Where(p => p.id == playlid_id)
                      .FirstOrDefault<JGN_User_Playlists>();

                if(item != null)
                {
                    item.isapproved = (byte)value;

                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
            return true;
        }


        // Load Playlist Information
        public static Task<List<JGN_User_Playlists>> Fetch_PlayList(ApplicationDbContext context, long playlid_id)
        {
            return context.JGN_User_Playlists
                     .Where(p => p.id == playlid_id)
                     .Select(p => new JGN_User_Playlists()
                     {
                         title = p.title,
                         description = p.description,
                         tags = p.tags,
                         privacy = p.privacy,
                         videos = p.videos,
                         userid = p.userid,
                         created_at = (DateTime)p.created_at,
                     }).ToListAsync();
        }

        // Load user playlists
        public static Task<List<JGN_User_Playlists>> Load_PlayLists(ApplicationDbContext context, string userid)
        {
           return context.JGN_User_Playlists
                     .Where(p => p.userid == userid && p.isenabled == 1 && p.isapproved == 1)
                     .Select(p => new JGN_User_Playlists()
                     {
                         id = p.id,
                         title = p.title
                     }).ToListAsync();
            
         }

        // Load user playlists // video preview section
        public static Task<List<JGN_User_Playlists>> Load_Preview_PlayLists(ApplicationDbContext context, string userid)
        {
            return context.JGN_User_Playlists
                     .Where(p => p.userid == userid && p.isenabled == 1 && p.isapproved == 1)
                     .Select(p => new JGN_User_Playlists()
                     {
                         id = p.id,
                         title = p.title,
                         videos = p.videos
                     }).ToListAsync();
        }
    }
}
/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
