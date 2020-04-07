using System;
using System.Linq;
using System.Threading.Tasks;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// Business Layer: For processing favorited user contents
/// </summary>
namespace Jugnoon.BLL
{
    public class FavoriteBLL
    {
        // Note: Important Terms
        public enum Types
        {
            Videos = 0
        };
        public static async Task<bool> Add(ApplicationDbContext context,string userid, long contentid, int mediatype, int type)
        {
            context.Entry(new JGN_Favorites()
            {
                contentid = contentid,
                userid = userid,
                created_at = DateTime.Now,
                type = (byte)type
            }).State = EntityState.Added;

            await context.SaveChangesAsync();


            Update_Fav_Stats(context, userid, mediatype, type, 0);
            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context, long contentid, string userid, byte mediatype, int type)
        {
            var all = from c in context.JGN_Favorites where c.contentid == contentid && c.userid == userid && c.type == type select c;
            context.JGN_Favorites.RemoveRange(all);
            await context.SaveChangesAsync();

            /*if (userid != "")
            {
               Update_Fav_Stats(context, userid, mediatype, type, 1);
            }*/

            return true;
        }

        private static void Update_Fav_Stats(ApplicationDbContext context, string username, int mediatype, int type, int action)
        {
            // Removed saving favorites stats in database
            /*if (type != 2)
            {
                // changes happend
                string _field = "stat_favorites";
                if (mediatype == 1)
                    _field = "stat_audiofavorites";
                else if (type == 1)
                    _field = "stat_qafavorites";

                int count = Convert.ToInt32(UserBLL.Return_Value(context, username, _field));
                if (action == 0)
                    count++;
                else
                    count--;
                if(type == 1)
                   UserBLL.Update_Field_V3(context, username, _field, (byte)count);
                else
                   UserBLL.Update_Field_V3(context, username, _field, (byte)count);
            }*/

        }

        public static async Task<bool> Check(ApplicationDbContext context,string userid, long contentid, int type)
        {
            bool flag = false;

                if (await context.JGN_Favorites.Where(p => p.contentid == contentid && p.userid == userid && p.type == type).CountAsync() > 0)
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
