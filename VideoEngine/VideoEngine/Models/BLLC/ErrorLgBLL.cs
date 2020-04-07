using System;
using Jugnoon.Utility;
using Jugnoon.Entity;
using System.Collections.Generic;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing error logged on website
/// </summary>
namespace Jugnoon.BLL
{
    public class ErrorLgBLL
    {
        public static bool Add(ApplicationDbContext context,string description, string url, string stack_trace)
        {           
            context.Entry(new JGN_Log()
            {
                description = UtilityBLL.processNull(description, 0),
                url = UtilityBLL.processNull(url, 0),
                stack_trace = UtilityBLL.processNull(stack_trace, 0),
                created_at = DateTime.Now
            }).State = EntityState.Added;

            context.SaveChanges();
            
            return true;
        }

        public static bool Delete(ApplicationDbContext context, int id)
        {            
                var entity = new JGN_Log { id = id };
                context.JGN_Log.Attach(entity);
                context.JGN_Log.Remove(entity);
                context.SaveChanges();
                return true;
        }

        public static bool Delete(ApplicationDbContext context)
        {
            var all = from c in context.JGN_Log select c;
            context.JGN_Log.RemoveRange(all);
            context.SaveChanges();
            return true;
        }

        public static Task<List<JGN_Log>> Load(ApplicationDbContext context,LogEntity entity)
        {
            var collectionQuery = context.JGN_Log.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);

            if (entity.id > 0)
            {
                entity.loadall = true;
                return LoadCompleteList(collectionQuery);
            }
            else
            {
                return LoadSummaryList(collectionQuery);
            }
        }
                
        public static Task<int> Count(ApplicationDbContext context,LogEntity entity)
        {
            return context.JGN_Log.Where(returnWhereClause(entity)).CountAsync();
        }

        private static Task<List<JGN_Log>> LoadCompleteList(IQueryable<JGN_Log> query)
        {
            return query.Select(p => new JGN_Log
            {
                id = (int)p.id,
                description = p.description,
                url = p.url,
                stack_trace = p.stack_trace,
                created_at = (DateTime)p.created_at
            }).ToListAsync();
        }
        private static Task<List<JGN_Log>> LoadSummaryList(IQueryable<JGN_Log> query)
        {
            return query.Select(p => new JGN_Log
            {
                id = (int)p.id,
                url = p.url,
                description = p.description,
                stack_trace = p.stack_trace,
                created_at = (DateTime)p.created_at
            }).ToListAsync();
        }

        private static IQueryable<JGN_Log> processOptionalConditions(IQueryable<JGN_Log> collectionQuery, LogEntity query)
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

        private static IQueryable<JGN_Log> AddSortOption(IQueryable<JGN_Log> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Log>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Log, bool>> returnWhereClause(LogEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Log>(true);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);
            if (entity.term != "")
                where_clause = where_clause.And(p => p.description.Contains(entity.term) || p.stack_trace.Contains(entity.term) || p.url.Contains(entity.term));
            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context,List<LogEntity> list)
        {
            foreach (LogEntity entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "delete":
                            Delete(context, (int)entity.id);
                            break;
                        case "deleteall":
                            Delete(context);
                            break;
                    }
                }
            }
            return "SUCCESS";
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
