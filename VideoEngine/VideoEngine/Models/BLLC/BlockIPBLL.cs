using System;
using Jugnoon.Entity;
using System.Collections.Generic;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing blocked ip addresses
/// </summary>
namespace Jugnoon.BLL
{
    public class BlockIPBLL
    {
        public static JGN_BlockIP Add_IP(ApplicationDbContext context, JGN_BlockIP entity)
        {
            var data = new JGN_BlockIP()
            {
                ipaddress = entity.ipaddress,
                created_at = DateTime.Now
            };
            context.Entry(data).State = EntityState.Added;

            context.SaveChanges();

            entity.id = data.id;
            
            return entity;
        }

        public static void Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
            if ( ID  > 0)
            {
                var item = context.JGN_BlockIP
                    .Where(p => p.id == ID)
                    .FirstOrDefault<JGN_BlockIP>();

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

        public static void Delete(ApplicationDbContext context, int id)
        {
            if (id > 0)
            {
                var entity = new JGN_BlockIP { id = id };
                context.JGN_BlockIP.Attach(entity);
                context.JGN_BlockIP.Remove(entity);
                context.SaveChanges();
            }
        }

        
        public static bool Validate(ApplicationDbContext cntx, string ipaddress)
        {
            bool flag = false;
            var records = cntx.JGN_BlockIP
                    .Where(p => p.ipaddress == ipaddress)
                    .Count();
            if (records > 0)
                flag = true;
            return flag;
        }

        public static Task<List<JGN_BlockIP>> LoadItems(ApplicationDbContext context,BlockIPEntity entity)
        {
            var collectionQuery = context.JGN_BlockIP.Where(returnWhereClause(entity));
            return LoadCompleteList(processOptionalConditions(collectionQuery, entity));
        }

        public static async Task<int> Count(ApplicationDbContext context,BlockIPEntity entity)
        {
            return await context.JGN_BlockIP.Where(returnWhereClause(entity)).CountAsync();
        }
        
        public static Task<List<JGN_BlockIP>> LoadCompleteList(IQueryable<JGN_BlockIP> query)
        {
            return query.Select(p => new JGN_BlockIP
            {
                id = (int)p.id,
                ipaddress = p.ipaddress,
                created_at = p.created_at,
            }).ToListAsync();
        }
        
        private static IQueryable<JGN_BlockIP> processOptionalConditions(IQueryable<JGN_BlockIP> collectionQuery, BlockIPEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_BlockIP>)collectionQuery.Sort(query.order);
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


        private static System.Linq.Expressions.Expression<Func<JGN_BlockIP, bool>> returnWhereClause(BlockIPEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_BlockIP>(true);

            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.ipaddress.Contains(entity.term));


            return where_clause;
        }
        public static string ProcessAction(ApplicationDbContext context,List<BlockIPEntity> list)
        {
            foreach (BlockIPEntity entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "delete":
                            BlockIPBLL.Delete(context, (int)entity.id);
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
