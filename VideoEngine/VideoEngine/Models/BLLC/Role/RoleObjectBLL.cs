using System.Collections.Generic;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Jugnoon.Entity;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System;
using System.Threading.Tasks;

namespace Jugnoon.BLL
{
    public class RoleObjectBLL
    {
        public static async Task<JGN_RoleObjects> Add(ApplicationDbContext context, JGN_RoleObjects entity)
        {
            var _entity = new JGN_RoleObjects()
            {
                objectname = entity.objectname,
                description = entity.description,
                uniqueid = entity.uniqueid
            };

            context.Entry(_entity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return _entity;
        }
        public static async Task<bool> Update_Record(ApplicationDbContext context, JGN_RoleObjects entity)
        {
            var item = await context.JGN_RoleObjects
                    .Where(p => p.id == entity.id)
                    .FirstOrDefaultAsync();

            if (item != null)
            {
                item.objectname = UtilityBLL.processNull(entity.objectname, 0);
                item.description = UtilityBLL.processNull(entity.description, 0);

                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return true;
        }
        public static bool Delete(ApplicationDbContext context, short objectid)
        {
            var entity = new JGN_RoleObjects { id = objectid };
            context.JGN_RoleObjects.Attach(entity);
            context.JGN_RoleObjects.Remove(entity);

            return true;
        }

        public static string ProcessAction(ApplicationDbContext context,List<RoleObject> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "delete":
                            Delete(context, (short)entity.id);
                            break;
                    }
                }
            }
            return "Operation performed successfully";
        }

        public static Task<List<JGN_RoleObjects>> LoadItems(ApplicationDbContext context,RoleObject entity)
        {
            var collectionQuery = context.JGN_RoleObjects.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
         }


        public static async Task<int> Count(ApplicationDbContext context,RoleObject entity)
        {
            return await context.JGN_RoleObjects.Where(returnWhereClause(entity)).CountAsync();
        }


        private static Task<List<JGN_RoleObjects>> LoadCompleteList(IQueryable<JGN_RoleObjects> query)
        {
            return query.Select(p => new JGN_RoleObjects
            {
                id = p.id,
                objectname = p.objectname,
                description = p.description,
                uniqueid = p.uniqueid
            }).ToListAsync();
        }

        private static IQueryable<JGN_RoleObjects> processOptionalConditions(IQueryable<JGN_RoleObjects> collectionQuery, RoleObject query)
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

        private static IQueryable<JGN_RoleObjects> AddSortOption(IQueryable<JGN_RoleObjects> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_RoleObjects>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_RoleObjects, bool>> returnWhereClause(RoleObject entity)
        {
            var where_clause = PredicateBuilder.New<JGN_RoleObjects>(true);

            if (entity.id > 0)
               where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
               where_clause = where_clause.And(p => p.objectname.Contains(entity.term) || p.objectname.Contains(entity.term));

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
