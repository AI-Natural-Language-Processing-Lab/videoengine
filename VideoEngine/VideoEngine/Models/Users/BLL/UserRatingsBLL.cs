
using System.Linq;
using System.Threading.Tasks;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;

namespace Jugnoon.BLL
{
    public class UserRatingsBLL
    {

        // Note: Rating Important Terms

        // Rating:
        // ........... video | audio,  0: liked, 1: disliked
        public enum Types
        {
            Videos = 0,
        };

        public enum Ratings
        {
            Liked = 0,
            Disliked = 1
        };

        public static async Task<bool> Add(ApplicationDbContext context,string userid, long itemid, int type, int rating)
        {
            var _entity = new JGN_User_Ratings()
            {
                userid = userid,
                itemid = itemid,
                type = (byte)type,
                rating = (byte)rating
            };

            context.Entry(_entity).State = EntityState.Added;

            await context.SaveChangesAsync();

            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context, long itemid, string userid, byte type)
        {
            var all = from c in context.JGN_User_Ratings where c.itemid == itemid && c.userid == userid && c.type == type select c;
            context.JGN_User_Ratings.RemoveRange(all);
            await context.SaveChangesAsync();
            
            return true;
        }

        public static async Task<bool> Check(ApplicationDbContext context,string userid, long itemid, int type)
        {
            bool flag = false;
                if (await context.JGN_User_Ratings.Where(p => p.itemid == itemid && p.userid == userid && p.type == (byte)type).CountAsync() > 0)
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
