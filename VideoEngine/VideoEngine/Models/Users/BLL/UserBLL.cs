using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Settings;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LinqKit;
using Jugnoon.Models;

/// <summary>
/// Business Layer : For processing user accounts
/// </summary>
namespace Jugnoon.BLL
{
    public class UserBLL
    {
             
        #region Data Manipulation

        public static async Task<ApplicationUser> Update_User_Profile(ApplicationDbContext context, ApplicationUser entity, bool isAdmin = true)
        {
            ApplicationUser user;
            if (entity.Id != null && entity.Id != "")
            {
                user = await context.AspNetusers
                    .Where(p => p.Id == entity.Id)
                    .FirstOrDefaultAsync();
            } 
            else
            {
                user = await context.AspNetusers
                   .Where(p => p.UserName == entity.UserName)
                   .FirstOrDefaultAsync();
            }
             
            if(user != null)
            {
                user.firstname = UtilityBLL.processNull(entity.firstname, 0);
                user.lastname = UtilityBLL.processNull(entity.lastname, 0);
              
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

                if ( isAdmin )
                {
                    // update settings
                    entity.settings.userid = entity.Id;
                    await UserSettingsBLL.Update(context, entity.settings);
                    // update account
                    entity.account.userid = entity.Id;
                    await UserAccountBLL.Update(context, entity.account);
                }
               
            }

            return entity;
        }

        /// <summary>
        /// Return specified field by using User (UserName) as identity
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static string Return_Value_UserName(ApplicationDbContext context, string username, string fieldname)
        {
            string Value = "";
            // var context = SiteConfig.dbContext;
            var item = context.AspNetusers
                .Where(p => p.UserName == username)
                .FirstOrDefault<ApplicationUser>();

            if(item != null)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (prop.Name.ToLower() == fieldname.ToLower())
                    {
                        if (prop.GetValue(item, null) != null)
                            Value = prop.GetValue(item, null).ToString();
                    }
                }
            }

            return Value;
        }

        /// <summary>
        /// Return specified field by using User (UserId or Id) as identity
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static string Return_Value_UserId(ApplicationDbContext context, string userid, string fieldname)
        {
            string Value = "";
            // var context = SiteConfig.dbContext;
            var item = context.AspNetusers
                .Where(p => p.Id == userid)
                .FirstOrDefault<ApplicationUser>();

            if (item != null)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (prop.Name.ToLower() == fieldname.ToLower())
                    {
                        if (prop.GetValue(item, null) != null)
                            Value = prop.GetValue(item, null).ToString();
                    }
                }
            }

            return Value;
        }

        /// <summary>
        /// Update field data in user table by using (UserName) as identifier
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="fieldname"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Update_Field_UserName(ApplicationDbContext context,string username, string fieldname, dynamic value)
        {
            if (username != null && username != "")
            {
                var item = context.AspNetusers
                    .Where(p => p.UserName == username)
                    .FirstOrDefault();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == fieldname.ToLower())
                        {
                            prop.SetValue(item, value);
                        }
                    }
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update field data in user table by using (Id or UserId) as identifier
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="fieldname"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Update_Field_Id(ApplicationDbContext context, string userid, string fieldname, dynamic value)
        {
            if (userid != null && userid != "")
            {
                var item = context.AspNetusers
                 .Where(p => p.Id == userid)
                 .FirstOrDefault();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == fieldname.ToLower())
                        {
                            prop.SetValue(item, value);
                        }
                    }
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        public static async Task Update_Field_IdAsync(ApplicationDbContext context, string userid, string fieldname, dynamic value)
        {
            if (userid != null && userid != "")
            {
                var item = await context.AspNetusers
                 .Where(p => p.Id == userid)
                 .FirstOrDefaultAsync();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == fieldname.ToLower())
                        {
                            prop.SetValue(item, value);
                        }
                    }
                    context.Entry(item).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Update field data in user table by using (Email) as identifier
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="fieldname"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void Update_Field_Email(ApplicationDbContext context, string email, string fieldname, dynamic value)
        {
            if (email != null && email != "")
            {
                var item = context.AspNetusers
                 .Where(p => p.Email == email)
                 .FirstOrDefault();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == fieldname.ToLower())
                        {
                            prop.SetValue(item, value);
                        }
                    }
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }


        #endregion

        #region Premium UserBLLhip Authorization Checks
        // 0: Normal Member
        // 1: Administrators
        // 2: Premium Users
        public static int Get_MemberType(ApplicationDbContext context, string UserName)
        {
            return Get_MemberType_No_Session(context, UserName);
        }

        public static int Get_MemberType_No_Session(ApplicationDbContext context, string UserName)
        {
            return context.AspNetusers.Where(p => p.UserName == UserName).Select(u => u.type).SingleOrDefault();
        }

        /// <summary>
        ///  Check MembershipStatus status, true (Membership still active), false( Membership expired)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool Check_membership_Status(ApplicationDbContext context, string UserName)
        {
            // temporary removed
            return false;
        }

        public static bool Validate_User_Key(ApplicationDbContext context, string Id, string code)
        {
            bool flag = false;
            IQueryable<ApplicationUser> query = context.AspNetusers.Where(p => p.Id == Id && p.val_key == code);

            if (query.Count() > 0)
                flag = true;

            return flag;
        }

        #endregion

        #region Core Loading Script

        public static Task<List<ApplicationUser>> LoadItems(ApplicationDbContext context, MemberEntity entity)
        {
            if (!entity.iscache 
                || Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return FetchItems(context, entity);
            }
            else
            {
                string key = GenerateKey("lg_channel_", entity);
                var data = new List<ApplicationUser>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = FetchItems(context, entity).Result;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<ApplicationUser>)SiteConfig.Cache.Get(key);
                }

                return Task.Run(() => data);
            }
        }

        private static Task<List<ApplicationUser>> FetchItems(ApplicationDbContext context, MemberEntity entity)
        {
            var collectionQuery = context.AspNetusers.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadSummaryList(collectionQuery);
        }

        public static int Count(ApplicationDbContext context,MemberEntity entity)
        {
            if (!entity.iscache 
                || Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_members", entity);
                int records = 0;
                if (!SiteConfig.Cache.TryGetValue(key, out records))
                {
                    records = CountRecords(context,entity);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, records, cacheEntryOptions);
                }
                else
                {
                    records = (int)SiteConfig.Cache.Get(key);
                }
                return records;
            }
        }

        private static int CountRecords(ApplicationDbContext context,MemberEntity entity)
        {
            return context.AspNetusers.Where(returnWhereClause(entity)).Count();
        }

        private static string GenerateKey(string key, MemberEntity entity)
        {
            return key + entity.accounttype + entity.datefilter + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + entity.pagenumber + "" +
                entity.term + "" + entity.character + "" + entity.gender + "" + entity.countryname + "" + entity.havephoto + "" + entity.advancelist + "" + entity.pagesize;

        }

        private static Task<List<ApplicationUser>> LoadSummaryList(IQueryable<ApplicationUser> query)
        {
            return query.Select(p => new ApplicationUser
            {
                Id = p.Id,
                UserName = p.UserName,
                views = p.views,
                picturename = p.picturename,
                created_at = (DateTime)p.created_at,
                isenabled = p.isenabled,
                EmailConfirmed = p.EmailConfirmed,
                LockoutEnabled = p.LockoutEnabled,
                firstname = p.firstname,
                lastname =  p.lastname,
            }).ToListAsync();
        }
  
        private static IQueryable<ApplicationUser> processOptionalConditions(IQueryable<ApplicationUser> collectionQuery, MemberEntity query)
        {
            if (query.order != "")
            {
                var orderlist = query.order.Split(char.Parse(","));
                foreach (var orderItem in orderlist)
                {
                    if (orderItem.Contains("asc") || orderItem.Contains("desc"))
                    {
                        var ordersplit = query.order.Split(char.Parse(" "));
                        if (ordersplit.Length > 1)
                        {
                            collectionQuery = AddSortOption(collectionQuery, ordersplit[0], ordersplit[1]);
                        }
                    }
                    else
                    {
                        collectionQuery = AddSortOption(collectionQuery, orderItem, "");
                    }
                }

            }

            if (query.userid == "")
            {
                // validation check (if not set, it will return zero records that will make it difficult to debug the code)
                if (query.pagesize == 0)
                    query.pagesize = 18;
                // skip logic
                if (query.pagenumber > 1)
                    collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
                // take logic
                if (!query.loadall)
                    collectionQuery = collectionQuery.Take(query.pagesize);
            }

            return collectionQuery;
        }

        private static IQueryable<ApplicationUser> AddSortOption(IQueryable<ApplicationUser> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<ApplicationUser>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<ApplicationUser, bool>> returnWhereClause(MemberEntity entity)
        {
            var where_clause = PredicateBuilder.New<ApplicationUser>(true);

            where_clause = where_clause.And(p => p.isenabled != 3);

            if (entity.id != "")
                where_clause = where_clause.And(p => p.Id == entity.id);

            if (entity.username != "" && entity.username != null)
                where_clause = where_clause.And(p => p.UserName == entity.username);

            if (!entity.nofilter)
            {
                if (entity.term != "" && entity.term != null)
                    where_clause = where_clause.And(p => p.firstname.Contains(entity.term) || p.lastname.Contains(entity.term) || p.UserName.Contains(entity.term));

                if (entity.month > 0 && entity.year > 0)
                    where_clause = where_clause.And(p => p.created_at.Month == entity.month && p.created_at.Year == entity.year);
                else if (entity.year > 0)
                    where_clause = where_clause.And(p => p.created_at.Year == entity.year);
                else if (entity.month > 0)
                    where_clause = where_clause.And(p => p.created_at.Month == entity.month);

                if (entity.emailconfirmed != 2)
                {
                    var _emailconfirm = false;
                    if (entity.emailconfirmed == 1)
                        _emailconfirm = true;
                    where_clause = where_clause.And(p => p.EmailConfirmed == _emailconfirm);
                }

                if (entity.lockoutenabled != 2)
                {
                    var _lockoutenabled = false;
                    if (entity.lockoutenabled == 1)
                        _lockoutenabled = true;
                    where_clause = where_clause.And(p => p.LockoutEnabled == _lockoutenabled);
                }

                if (entity.isenabled != EnabledTypes.All)
                    where_clause = where_clause.And(p => p.isenabled == (byte)entity.isenabled);

                if (entity.havephoto)
                    where_clause = where_clause.And(p => p.picturename != "none");

                if (entity.character != "" && entity.character != null)
                    where_clause = where_clause.And(p => p.UserName.StartsWith(entity.character));

                if (entity.type != 3)
                    where_clause = where_clause.And(p => p.type == entity.type);

                if (entity.datefilter != DateFilter.AllTime)
                {
                    switch (entity.datefilter)
                    {
                        case DateFilter.Today:
                            // today record
                            where_clause = where_clause.And(p => p.created_at >= DateTime.Now.AddDays(-1));
                            break;
                        case DateFilter.ThisWeek:
                            // this week record
                            where_clause = where_clause.And(p => p.created_at >= DateTime.Now.AddDays(-7));
                            break;
                        case DateFilter.ThisMonth:
                            // this month record
                            where_clause = where_clause.And(p => p.created_at >= DateTime.Now.AddDays(-31));
                            break;
                    }
                }
            }

            return where_clause;
        }

        #endregion

        #region Report Script
        public static async Task<GoogleChartEntity> LoadReport(ApplicationDbContext context, MemberEntity entity)
        {
            if (entity.reporttype == DefaultReportTypes.Yearly)
                return await UserReports.YearlyReport(context, entity);
            else if (entity.reporttype == DefaultReportTypes.CurrentMonth)
                return await UserReports.CurrentMonthReport(context, entity);
            else
                return await UserReports.Last12MonthsReport(context, entity);
        }
        #endregion


        public static string prepareUserName(ApplicationUser user)
        {
            if (user.firstname != "" || user.lastname != "")
            {
                var name = user.firstname + " " + user.lastname;
                return name.Trim();
            }
            else if (!user.UserName.Contains("@"))
            {
                return user.UserName;
            }
            else
            {
                return "";
            }
        }
        public static async Task<string> ProcessAction(ApplicationDbContext context,List<MemberEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id != "")
                {
                    var user = await SiteConfig.userManager.FindByIdAsync(entity.id);
                    if (user != null)
                    {
                        switch (entity.actionstatus)
                        {
                            case "enable":
                                // enforcing (disabling user all active contents within website)
                                Update_Field_Id(context, entity.userid, "isenabled", (byte)1);
                                break;
                            case "disable":
                                // enforcing (disabling user all contents within website)
                                Update_Field_Id(context, entity.userid, "isenabled", (byte)0);
                                break;
                            case "admin":
                                await SiteConfig.userManager.AddToRoleAsync(user, "Admin");
                                // Update_Field_V3(context, entity.userid, "type", (byte)1);
                                break;
                            case "paid":
                                if (await SiteConfig.userManager.IsInRoleAsync(user, "Admin"))
                                   await SiteConfig.userManager.RemoveFromRoleAsync(user, "Admin");
 
                                await SiteConfig.userManager.AddToRoleAsync(user, "Manager");
                                //Update_Field_V3(context, entity.userid, "type", (byte)2);
                                break;
                            case "normal":
                                if (await SiteConfig.userManager.IsInRoleAsync(user, "Admin"))
                                   await SiteConfig.userManager.RemoveFromRoleAsync(user, "Admin");
                                if (await SiteConfig.userManager.IsInRoleAsync(user, "Manager"))
                                    await SiteConfig.userManager.RemoveFromRoleAsync(user, "Manager");
                                await SiteConfig.userManager.AddToRoleAsync(user, "Member");
                                // Update_Field_V3(context, entity.userid, "type", (byte)0);
                                break;
                            case "delete":
                                /* archive user instead of deleting completely */
                                Update_Field_Id(context, entity.userid, "isenabled", (byte)3);
                                break;
                        }
                    }
                   
                }
            }
            return "OK";
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
