using System;
using System.Collections.Generic;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Dynamic Attributes Processing Business Layer
/// </summary>
namespace Jugnoon.Attributes
{
    public class AttrTemplatesSectionsBLL
    {

        #region Action Script

        public static async Task<JGN_Attr_TemplateSections> Add(ApplicationDbContext context, JGN_Attr_TemplateSections entity)
        {
            var ent = new JGN_Attr_TemplateSections()
            {
                title = UtilityBLL.processNull(entity.title, 0),
                templateid = entity.templateid,
                priority = entity.priority,
                attr_type = entity.attr_type,
                showsection = entity.showsection
            };
            context.Entry(ent).State = EntityState.Added;

            await context.SaveChangesAsync();
            entity.id = ent.id;
            return entity;
        }

        public static async Task<bool> Update(ApplicationDbContext context, JGN_Attr_TemplateSections entity)
        {
            if (entity.id > 0)
            {
                var item = await context.JGN_Attr_TemplateSections
                   .Where(p => p.id == entity.id)
                   .FirstOrDefaultAsync();
                if (item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    item.priority = entity.priority;
                    item.showsection = entity.showsection;
                    context.Entry(item).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            return true;
        }

        public static async Task<bool> Delete(ApplicationDbContext context, long id)
        {
            if (id > 0)
            {
                context.JGN_Attr_TemplateSections.RemoveRange(context.JGN_Attr_TemplateSections.Where(x => x.id == id));
                await context.SaveChangesAsync();
            }
            return true;
        }

        #endregion

        #region Core Loading Script

        public static async Task<List<JGN_Attr_TemplateSections>> LoadItems(ApplicationDbContext context, AttrTemplateSectionEntity entity)
        {
            if (!entity.iscache
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return await FetchItems(context, entity);
            }
            else
            {
                string key = GenerateKey("ld_ad_attr_temp_", entity);
                var data = new List<JGN_Attr_TemplateSections>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = await FetchItems(context, entity);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<JGN_Attr_TemplateSections>)SiteConfig.Cache.Get(key);
                }
                return data;
            }
        }
        private static async Task<List<JGN_Attr_TemplateSections>> FetchItems(ApplicationDbContext context, AttrTemplateSectionEntity entity)
        {
            var collectionQuery = context.JGN_Attr_TemplateSections.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);

            return await LoadCompleteList(collectionQuery);
        }

        public static string GenerateKey(string key, AttrTemplateSectionEntity entity)
        {
            var str = new StringBuilder();
            str.AppendLine(key + "_" + "" + entity.templateid + "" + entity.attr_type + "" +  entity.pagenumber + "" + entity.pagesize);
           

            return str.ToString();
        }

        public static async Task<int> Count(ApplicationDbContext context, AttrTemplateSectionEntity entity)
        {
            return await context.JGN_Attr_TemplateSections.Where(returnWhereClause(entity)).CountAsync();
        }

        public static Task<List<JGN_Attr_TemplateSections>> LoadCompleteList(IQueryable<JGN_Attr_TemplateSections> query)
        {
            return query.Select(p => new JGN_Attr_TemplateSections
            {
                id = p.id,
                title = p.title,
                templateid = p.templateid,
                priority = p.priority,
                showsection = p.showsection
            }).ToListAsync();
        }

        private static IQueryable<JGN_Attr_TemplateSections> processOptionalConditions(IQueryable<JGN_Attr_TemplateSections> collectionQuery, AttrTemplateSectionEntity query)
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
            // skip logic
            if (query.pagenumber > 1)
                collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
            // take logic
            if (!query.loadall)
                collectionQuery = collectionQuery.Take(query.pagesize);


            return collectionQuery;
        }

        private static IQueryable<JGN_Attr_TemplateSections> AddSortOption(IQueryable<JGN_Attr_TemplateSections> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Attr_TemplateSections>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Attr_TemplateSections, bool>> returnWhereClause(AttrTemplateSectionEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Attr_TemplateSections>(true);

            where_clause = where_clause.And(p => p.attr_type == (byte)entity.attr_type);

            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);
            else
            {
                if (entity.excludedid > 0)
                    where_clause = where_clause.And(p => p.id != entity.excludedid);

                if (entity.templateid > 0)
                    where_clause = where_clause.And(p => p.templateid == entity.templateid);

                if (entity.term != "")
                    where_clause = where_clause.And(p => p.title.Contains(entity.term));

            }

            return where_clause;
        }

        #endregion

        public static async Task<string> ProcessAction(ApplicationDbContext context, List<AttrTemplateSectionEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
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
