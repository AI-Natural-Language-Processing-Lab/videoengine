using System.Linq;
using System.Threading.Tasks;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
/// <summary>
/// Business Layer : For processing user profile / account payment data e.g credits, balance, subscription etc
/// </summary>
namespace Jugnoon.BLL
{
    public class UserAccountBLL
    {
        public static async Task Add(ApplicationDbContext context, JGN_User_Account entity)
        {
            if (entity.userid != null && entity.userid != "")
            {
                var _entity = new JGN_User_Account()
                {
                    userid = entity.userid
                };

                context.Entry(_entity).State = EntityState.Added;

                await context.SaveChangesAsync();
            }
        }

        public static async Task Update(ApplicationDbContext context, JGN_User_Account entity)
        {

            var item = await context.JGN_User_Account
                    .Where(p => p.userid == entity.userid)
                    .FirstOrDefaultAsync();

            if (item != null)
            {
                item.credits = entity.credits;
                item.islifetimerenewal = entity.islifetimerenewal;

                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public static async Task Delete(ApplicationDbContext context, string userid)
        {
            var itemsToDelete = context.JGN_User_Account.Where(x => x.userid == userid);
            context.JGN_User_Account.RemoveRange(itemsToDelete);
            await context.SaveChangesAsync();
        }

        public static void Update_Field(ApplicationDbContext context, string userid, dynamic Value, string FieldName)
        {
            if (userid != null && userid != "")
            {
                var item = context.JGN_User_Account
                     .Where(p => p.userid == userid)
                     .FirstOrDefault();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == FieldName.ToLower())
                        {
                            prop.SetValue(item, Value);
                        }
                    }
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChangesAsync();
                }
            }
        }

        public static string Get_Field_Value(ApplicationDbContext context, string userid, string field_name)
        {
            string Value = "";
            if (userid != null && userid != "")
            {
                var item = context.JGN_User_Account
                 .Where(p => p.userid == userid)
                 .FirstOrDefault();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == field_name.ToLower())
                        {
                            if (prop.GetValue(item, null) != null)
                                Value = prop.GetValue(item, null).ToString();
                        }
                    }

                }
            }

            return Value;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
