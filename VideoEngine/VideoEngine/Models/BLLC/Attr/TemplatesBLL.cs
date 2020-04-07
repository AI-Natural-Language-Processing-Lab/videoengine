using System;
using System.Collections.Generic;
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
    public class AttrTemplatesBLL
    {

        #region Action Script

        public static async Task<JGN_Attr_Templates> Add(ApplicationDbContext context, JGN_Attr_Templates entity)
        {
            var ent = new JGN_Attr_Templates()
            {
                title = UtilityBLL.processNull(entity.title, 0),
                attr_type = entity.attr_type
            };
            context.Entry(ent).State = EntityState.Added;

            await context.SaveChangesAsync();
            entity.id = ent.id;
            return entity;
        }

        public static async Task<bool> Update(ApplicationDbContext context, JGN_Attr_Templates entity)
        {
            if (entity.id > 0)
            {
                var item = context.JGN_Attr_Templates
                   .Where(p => p.id == entity.id)
                   .FirstOrDefault();
                if (item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    
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
                context.JGN_Attr_Templates.RemoveRange(context.JGN_Attr_Templates.Where(x => x.id == id));
                await context.SaveChangesAsync();
                // remove all sections 
                context.JGN_Attr_TemplateSections.RemoveRange(context.JGN_Attr_TemplateSections.Where(x => x.templateid == id));
                await context.SaveChangesAsync();
            }
            return true;
        }

        #endregion

        #region Core Loading Script

        public static Task<List<JGN_Attr_Templates>> LoadItems(ApplicationDbContext context, AttrTemplateEntity entity)
        {
            var collectionQuery = context.JGN_Attr_Templates.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);

            return LoadCompleteList(collectionQuery);
        }
      

        public static int Count(ApplicationDbContext context, AttrTemplateEntity entity)
        {
            return context.JGN_Attr_Templates.Where(returnWhereClause(entity)).Count();
        }

        public static Task<List<JGN_Attr_Templates>> LoadCompleteList(IQueryable<JGN_Attr_Templates> query)
        {
            return query.Select(p => new JGN_Attr_Templates
            {
                id = p.id,
                title = p.title,
            }).ToListAsync();
        }

        private static IQueryable<JGN_Attr_Templates> processOptionalConditions(IQueryable<JGN_Attr_Templates> collectionQuery, AttrTemplateEntity query)
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

        private static IQueryable<JGN_Attr_Templates> AddSortOption(IQueryable<JGN_Attr_Templates> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Attr_Templates>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Attr_Templates, bool>> returnWhereClause(AttrTemplateEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Attr_Templates>(true);

            where_clause = where_clause.And(p => p.attr_type == (byte)entity.attr_type);

            if (entity.excludedid > 0)
                where_clause = where_clause.And(p => p.id != entity.excludedid);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.nofilter)
            {

                if (entity.term != "")
                    where_clause = where_clause.And(p => p.title.Contains(entity.term));

            }

            return where_clause;
        }

        #endregion

        public static async Task<string> ProcessAction(ApplicationDbContext context, List<AttrTemplateEntity> list)
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
