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
    public enum Attr_Type
    {
        Ad = 0,
        Agency = 1,
        Artist = 2,
        UserProfile = 3
    };
    public class AttrAttributeBLL
    {
      

        #region Action Script

        public static async Task<JGN_Attr_Attributes> Add(ApplicationDbContext context, JGN_Attr_Attributes entity)
        {
            var ent = new JGN_Attr_Attributes()
            {
                title = UtilityBLL.processNull(entity.title, 0),
                value = UtilityBLL.processNull(entity.value, 0),
                options = UtilityBLL.processNull(entity.options, 0),
                sectionid = entity.sectionid,
                priority = entity.priority,
                attr_type = entity.attr_type,
                element_type = entity.element_type,
                isrequired = entity.isrequired,
                variable_type = entity.variable_type,
                min = entity.min,
                max = entity.max,
                helpblock = UtilityBLL.processNull(entity.helpblock, 0),
                icon = entity.icon
            };
            context.Entry(ent).State = EntityState.Added;

            await context.SaveChangesAsync();
            entity.id = ent.id;
            return entity;
        }


        public static async Task<bool> Update(ApplicationDbContext context,JGN_Attr_Attributes entity)
        {
            if (entity.id > 0)
            {
                var item = context.JGN_Attr_Attributes
                   .Where(p => p.id == entity.id)
                   .FirstOrDefault();
                if (item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    item.value = string.Join(",", entity.value);
                    item.options = UtilityBLL.processNull(entity.options, 0);
                    item.priority = (short)entity.priority;
                    item.element_type = entity.element_type;
                    item.isrequired = entity.isrequired;
                    item.variable_type = entity.variable_type;
                    item.icon = entity.icon;
                    item.min = entity.min;
                    item.max = entity.max;
                    item.helpblock = UtilityBLL.processNull(entity.helpblock, 0);
                    context.Entry(item).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context,long id)
        {
            if (id > 0)
            {
                context.JGN_Attr_Attributes.RemoveRange(context.JGN_Attr_Attributes.Where(x => x.id == id));
                await context.SaveChangesAsync();
            }
            return true;
        }
                       
        #endregion

        #region Core Loading Script

        public static Task<List<JGN_Attr_Attributes>> LoadItems(ApplicationDbContext context, AttrAttributeEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return FetchItems(context,entity);
            }
            else
            {
                string key = GenerateKey("ld_atr_attr_1", entity);
                var data = new List<JGN_Attr_Attributes>();
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
                    data = (List<JGN_Attr_Attributes>)SiteConfig.Cache.Get(key);
                }
                return Task.Run(() => data);
            }
        }

        private static Task<List<JGN_Attr_Attributes>> FetchItems(ApplicationDbContext context,AttrAttributeEntity entity)
        {
            var collectionQuery = context.JGN_Attr_Attributes.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
        }
        public static async Task<int> Count(ApplicationDbContext context,AttrAttributeEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_atr_attr_1", entity);
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

        private static async Task<int> CountRecords(ApplicationDbContext context,AttrAttributeEntity entity)
        {
           return await context.JGN_Attr_Attributes.Where(returnWhereClause(entity)).CountAsync();
        }

        public static string GenerateKey(string key, AttrAttributeEntity entity)
        {
            var str = new StringBuilder();
            str.AppendLine(key + "_" + "" + entity.sectionid + entity.term + "" + entity.type + "" + entity.attr_type +  entity.pagenumber + "" + entity.pagesize);
           
            return str.ToString();
        }

        public static Task<List<JGN_Attr_Attributes>> LoadCompleteList(IQueryable<JGN_Attr_Attributes> query)
        {
            return query.Select(p => new JGN_Attr_Attributes
            {
                id = p.id,
                title = p.title,
                value = p.value,
                sectionid = p.sectionid,
                options = p.options,
                priority = p.priority,
                attr_type = p.attr_type,
                element_type = p.element_type,
                isrequired = p.isrequired,
                variable_type = p.variable_type,
                helpblock = p.helpblock,
                min = p.min,
                max = p.max,
                icon = p.icon
            }).ToListAsync();
        }

        private static IQueryable<JGN_Attr_Attributes> processOptionalConditions(IQueryable<JGN_Attr_Attributes> collectionQuery, AttrAttributeEntity query)
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

            // skip logic (page size filter not required in dynamic values)
            //if (query.pagenumber > 1)
            //     collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
            // take logic
            //if (!query.loadall)
            //    collectionQuery = collectionQuery.Take(query.pagesize);


            return collectionQuery;
        }

        private static IQueryable<JGN_Attr_Attributes> AddSortOption(IQueryable<JGN_Attr_Attributes> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Attr_Attributes>)collectionQuery.Sort(field, reverse);
        }

        private static System.Linq.Expressions.Expression<Func<JGN_Attr_Attributes, bool>> returnWhereClause(AttrAttributeEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Attr_Attributes>(true);
            
            if (entity.excludedid > 0)
                 where_clause = where_clause.And(p => p.id != entity.excludedid);

            if (entity.id > 0)
                 where_clause = where_clause.And(p => p.id == entity.id);
            
            if (!entity.nofilter) { 

                if (entity.sectionid > 0)
                    where_clause = where_clause.And(p => p.sectionid == entity.sectionid);

                if (entity.term != "")
                     where_clause = where_clause.And(p => p.title.Contains(entity.term));
            }

            return where_clause;
        }

        #endregion
    
        public static void Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
            if (ID > 0)
            {
                var item = context.JGN_Attr_Attributes
                   .Where(p => p.id == ID)
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
                    context.SaveChanges();

                }
            }
        }

        public static string Get_Field_Value(ApplicationDbContext context,long ID, string FieldName)
        {
            string Value = "";
            if (ID > 0)
            {
                var item = context.JGN_Attr_Attributes
                    .Where(p => p.id == ID)
                    .FirstOrDefault();

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
            }
            
            return Value;
        }

        public static async Task<string> ProcessAction(ApplicationDbContext context,List<AttrAttributeEntity> list)
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
                            await Delete(context, (short)entity.id);
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
