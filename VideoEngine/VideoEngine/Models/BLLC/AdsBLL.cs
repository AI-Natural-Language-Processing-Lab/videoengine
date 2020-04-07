using System;
using Jugnoon.Utility;
using System.Text;
using Jugnoon.Entity;
using System.Collections.Generic;
using Jugnoon.Framework;
using System.Linq;
using Jugnoon.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;

/// <summary>
/// Business Layer: For processing advertisements, adsense ads
/// </summary>
namespace Jugnoon.BLL
{
    public enum Adtype
    {
        Adult = 1,
        NonAdult = 0,
        All = 2
    };

    public class AdsBLL
    {
        public static async Task<bool> Add_Script(ApplicationDbContext context, string name, string adscript, int type)
        {
            context.Entry(new JGN_Ads()
            {
                name = name,
                adscript = adscript,
                type = (byte)type
            }).State = EntityState.Added;

            await context.SaveChangesAsync();
            
            return true;
        }

        public static bool Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
            var item = context.JGN_Ads
                    .Where(p => p.id == ID)
                    .FirstOrDefault<JGN_Ads>();

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
                context.SaveChanges();
            }
               
            
            return true;
        }
     
        public static async Task<bool> Count_Script(ApplicationDbContext context)
        {
            bool flag = false;
            if (await context.JGN_Ads.CountAsync() > 0)
                 flag = true;

            return flag;
        }

        public static string Fetch_Ad_Script(ApplicationDbContext context, int id)
        {
            string Value = "";
            var item = context.JGN_Ads
                     .Where(p => p.id == id)
                     .FirstOrDefault<JGN_Ads>();

                if(item != null)
                   Value = item.adscript;

            return Value;
        }

        public static string Return_Ad_Script(ApplicationDbContext context, int id)
        {
            string cache_key = "ads_" + id;
            var value = "";
            if (!SiteConfig.Cache.TryGetValue(cache_key, out value))
            {
                value = Fetch_Ad_Script(context, id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                // Save data in cache.
                SiteConfig.Cache.Set(cache_key, value, cacheEntryOptions);
            }
            else
            {
                value = SiteConfig.Cache.Get(cache_key).ToString();
            }
            
            return value;
        }

        public static Task<List<JGN_Ads>> Load(ApplicationDbContext context,AdEntity entity)
        {
            var collectionQuery = context.JGN_Ads.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
        }

        public static async Task<int> Count(ApplicationDbContext context,AdEntity entity)
        {
            if (!entity.iscache || 
                Configs.GeneralSettings.cache_duration == 0 
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_ads", entity);
                int records = 0;
                if (!SiteConfig.Cache.TryGetValue(key, out records))
                {
                    records = await CountRecords(context,entity);

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

        private static async Task<int> CountRecords(ApplicationDbContext context,AdEntity entity)
        {
            return await context.JGN_Ads.Where(returnWhereClause(entity)).CountAsync();
        }

        private static string GenerateKey(string key, AdEntity entity)
        {
            return key + entity.type;
        }
        
        private static Task<List<JGN_Ads>> LoadCompleteList(IQueryable<JGN_Ads> query)
        {
            return query.Select(p => new JGN_Ads
            {
                id = (int)p.id,
                name = p.name,
                adscript = p.adscript,
                type = p.type
            }).ToListAsync();
        }      
        private static IQueryable<JGN_Ads> processOptionalConditions(IQueryable<JGN_Ads> collectionQuery, AdEntity query)
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

        private static IQueryable<JGN_Ads> AddSortOption(IQueryable<JGN_Ads> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Ads>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Ads, bool>> returnWhereClause(AdEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Ads>(true);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);
            if (entity.type != Adtype.All)
                where_clause = where_clause.And(p => p.type == (byte)entity.type);
            if (entity.term != "")
                where_clause = where_clause.And(p => p.adscript.Contains(entity.term) || p.name.Contains(entity.term));

            return where_clause;
        }
        public static string ProcessAction(ApplicationDbContext context,List<AdEntity> list)
        {           
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
