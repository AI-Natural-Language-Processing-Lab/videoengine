using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;

/// <summary>
/// Dynamic Attributes Processing Business Layer
/// </summary>
namespace Jugnoon.Attributes
{
    public class AttrValueBLL
    {

        #region Action Script
        
        public static async Task<JGN_Attr_Values> Add(ApplicationDbContext context,JGN_Attr_Values entity)
        {
            var ent = new JGN_Attr_Values()
            {
                userid = entity.userid,
                contentid = entity.contentid,
                attr_id = entity.attr_id,
                title = UtilityBLL.processNull(entity.title, 0),
                value = UtilityBLL.processNull(entity.value, 0),
                attr_type = entity.attr_type,
                priority = entity.priority,
            };
            context.Entry(ent).State = EntityState.Added;
            await context.SaveChangesAsync();
            entity.id = ent.id;

            return entity;
        }

        public static async Task<bool> Update(ApplicationDbContext context,JGN_Attr_Values entity)
        {
            if (entity.id > 0)
            {
                var item = context.JGN_Attr_Values
                    .Where(p => p.id == entity.id)
                    .FirstOrDefault();
                if (item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    item.value = string.Join(",", entity.value);
                    item.priority = (short)entity.priority;
                    context.Entry(item).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            return true;
        }

        public static void Delete(ApplicationDbContext context,long id)
        {      
            if (id > 0)
            {
                context.JGN_Attr_Values.RemoveRange(context.JGN_Attr_Values.Where(x => x.id == id));
                context.SaveChanges();
            }
        }

        #endregion

        #region Core Loading Script

        public static Task<List<JGN_Attr_Values>> LoadItems(ApplicationDbContext context,AttrValueEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return FetchItems(context,entity);
            }
            else
            {
                string key = GenerateKey("ld_artist_value_1", entity);
                var data = new List<JGN_Attr_Values>();
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
                    data = (List<JGN_Attr_Values>)SiteConfig.Cache.Get(key);
                }
                return Task.Run(() => data);
            }
        }

        private static Task<List<JGN_Attr_Values>> FetchItems(ApplicationDbContext context,AttrValueEntity entity)
        {          
            var collectionQuery = context.JGN_Attr_Values.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
        }

        public static int Count(ApplicationDbContext context,AttrValueEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_classified_value_1", entity);
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

        private static int CountRecords(ApplicationDbContext context,AttrValueEntity entity)
        {
            return context.JGN_Attr_Values.Where(returnWhereClause(entity)).Count();
        }

        public static string GenerateKey(string key, AttrValueEntity entity)
        {
            var str = new StringBuilder();
            str.AppendLine(key + "_" + "" + entity.term + "" + entity.contentid + "" + entity.attr_type + "" + entity.pagenumber + "" + entity.pagesize);
            if (entity.term != "")
                str.AppendLine(UtilityBLL.ReplaceSpaceWithHyphin(entity.term.ToLower()));

            return str.ToString();
        }
    

        public static Task<List<JGN_Attr_Values>> LoadCompleteList(IQueryable<JGN_Attr_Values> query)
        {
            return query.Select(p => new JGN_Attr_Values
            {
                id = p.id,
                attr_id = p.attr_id,
                title = p.title,
                value = p.value,
                contentid = p.contentid,
                priority = p.priority
            }).ToListAsync();
        }

        private static IQueryable<JGN_Attr_Values> processOptionalConditions(IQueryable<JGN_Attr_Values> collectionQuery, AttrValueEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_Attr_Values>)collectionQuery.Sort(query.order);
            // skip logic (page size filter not required in dynamic values)
            //if (query.pagenumber > 1)
            //     collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
            // take logic
            //if (!query.loadall)
            //    collectionQuery = collectionQuery.Take(query.pagesize);


            return collectionQuery;
        }

        private static System.Linq.Expressions.Expression<Func<JGN_Attr_Values, bool>> returnWhereClause(AttrValueEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Attr_Values>(true);

            where_clause = where_clause.And(p => p.attr_type == (byte)entity.attr_type);

            if (entity.id > 0)
                 where_clause = where_clause.And(p => p.id == entity.id);

            if (!entity.nofilter)
            {
              
                if (entity.contentid > 0)
                    where_clause = where_clause.And(p => p.contentid == entity.contentid);

                if (entity.userid != "")
                    where_clause = where_clause.And(p => p.userid == entity.userid);

                if (entity.term != "")
                     where_clause = where_clause.And(p => p.title.Contains(entity.term));
            }

            return where_clause;
        }

        #endregion


        /* Utility Functions */
        // Linq Version
        public static bool Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
                var item = context.JGN_Attr_Values
                     .Where(p => p.id == ID)
                     .FirstOrDefault<JGN_Attr_Values>();

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

        public static string Get_Field_Value(ApplicationDbContext context,long ID, string FieldName)
        {
            string Value = "";
            // var context = SiteConfig.dbContext;
           
            
                var item = context.JGN_Attr_Values
                     .Where(p => p.id == ID)
                     .FirstOrDefault<JGN_Attr_Values>();

                if (item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == FieldName.ToLower())
                        {
                            if (prop.GetValue(item, null) != null)
                                Value = prop.GetValue(item, null).ToString();
                        }
                    }
                }

            return Value;
        }

        public static string ProcessAction(ApplicationDbContext context,List<AttrValueEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "enable":
                            Update_Field_V3(context, entity.id, (byte)1, "isenabled");
                            break;

                        case "disable":
                            Update_Field_V3(context, entity.id, (byte)0, "isenabled");
                            break;

                        case "delete":
                            Delete(context, (short)entity.id);
                            break;
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
