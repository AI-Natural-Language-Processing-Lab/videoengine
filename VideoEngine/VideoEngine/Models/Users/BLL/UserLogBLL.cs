using System;
using Jugnoon.Utility;
using System.Collections.Generic;
using System.Text;
using Jugnoon.Entity;
using System.Linq;
using Jugnoon.Framework;
using Jugnoon.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;

/// <summary>
/// Business Layer : (optional) For processing user signup logs / history
/// </summary>
namespace Jugnoon.BLL
{
    public class UserLogBLL
    {
        public static bool Add(ApplicationDbContext context,string userid, string ipaddress)
        {
            var _entity = new JGN_User_IPLogs()
            {
                userid = userid,
                ipaddress = ipaddress,
                created_at = DateTime.Now
            };

            context.Entry(_entity).State = EntityState.Added;
            context.SaveChanges();
            return true;
        }

        public static bool Delete(ApplicationDbContext context, string userid)
        {
            var id = Get_Old_SerialID(context, userid);
                      
            var entity = new JGN_User_IPLogs { id = id };
            context.JGN_User_IPLogs.Attach(entity);
            context.JGN_User_IPLogs.Remove(entity);
            context.SaveChanges();

            return true;
        }

        public static int Get_Old_SerialID(ApplicationDbContext context, string userid)
        {
            var ID = 0;
            var item = context.JGN_User_IPLogs
                    .Where(p => p.userid == userid).Take(1).OrderBy(p => p.id).ToList();

            if(item != null)
                ID = item[0].id;
            
            return ID;
        }

        public static int Count_Ipaddress(ApplicationDbContext context, string userid)
        {
           return context.JGN_User_IPLogs.Where(p => p.userid == userid).Count();
        }

        public static bool Process(ApplicationDbContext context, string username, string ipaddress)
        {
            int count = Count_Ipaddress(context, username);
            // keep top 5 login ip logs of each user
            if (count > 5)
            {
                // delete old ip address log
                Delete(context, username);
                // add ip address log
                Add(context, username, ipaddress);
            }
            else
            {
                Add(context, username, ipaddress);
            }
            return true;
        }

        public static Task<List<JGN_User_IPLogs>> LoadItems(ApplicationDbContext context,UserIPEntity entity)
        {
            if (!entity.iscache || Configs.GeneralSettings.cache_duration == 0  || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return FetchItems(context,entity);
            }
            else
            {
                string key = GenerateKey("lg_user_ipaddress_", entity);
                var data = new List<JGN_User_IPLogs>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = FetchItems(context,entity).Result;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<JGN_User_IPLogs>)SiteConfig.Cache.Get(key);
                }

                return Task.Run(() => data);
            }
        }

        private static Task<List<JGN_User_IPLogs>> FetchItems(ApplicationDbContext context,UserIPEntity entity)
        {
            var collectionQuery = context.JGN_User_IPLogs.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
        }

        public static int Count(ApplicationDbContext context,UserIPEntity entity)
        {
            if (!entity.iscache || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_user_ipaddress", entity);
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

        private static int CountRecords(ApplicationDbContext context,UserIPEntity entity)
        {
            return context.JGN_User_IPLogs.Where(returnWhereClause(entity)).Count();
        }
        private static string GenerateKey(string key, UserIPEntity entity)
        {
            var str = new StringBuilder();
            return key + entity.datefilter + "" + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + entity.userid + entity.pagenumber + "" + entity.term;
        }
    
        private static Task<List<JGN_User_IPLogs>> LoadCompleteList(IQueryable<JGN_User_IPLogs> query)
        {
            return query.Select(p => new JGN_User_IPLogs
            {
                id = p.id,
                userid = p.userid,
                ipaddress = p.ipaddress,
                created_at = (DateTime)p.created_at
            }).ToListAsync();
        }

        private static IQueryable<JGN_User_IPLogs> processOptionalConditions(IQueryable<JGN_User_IPLogs> collectionQuery, UserIPEntity query)
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

            if (query.id == 0)
            {
                // skip logic
                if (query.pagenumber > 1)
                    collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
                // take logic
                if (!query.loadall)
                    collectionQuery = collectionQuery.Take(query.pagesize);
            }
           


            return collectionQuery;
        }

        private static IQueryable<JGN_User_IPLogs> AddSortOption(IQueryable<JGN_User_IPLogs> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_User_IPLogs>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_User_IPLogs, bool>> returnWhereClause(UserIPEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_User_IPLogs>(true);
            
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.ipaddress.Contains(entity.term));

            if (entity.userid != null && entity.userid != "")
                where_clause = where_clause.And(p => p.userid == entity.userid);

            return where_clause;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
