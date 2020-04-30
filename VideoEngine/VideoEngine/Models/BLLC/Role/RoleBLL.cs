using System;
using System.Collections.Generic;
using System.Linq;
using Jugnoon.Framework;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;

namespace Jugnoon.BLL
{
    public class RoleBLL
    {
        public static async Task<JGN_Roles> Add(ApplicationDbContext context, JGN_Roles entity)
        {
            var _entity = new JGN_Roles()
            {
                rolename = entity.rolename,
                created_at = DateTime.Now
            };

            context.Entry(_entity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return _entity;
        }

        public static bool Delete(ApplicationDbContext context, short roleid)
        {
            RolePermission.DeleteRole(context, roleid);
            
                var entity = new JGN_Roles { id = roleid };
                context.JGN_Roles.Attach(entity);
                context.JGN_Roles.Remove(entity);
                context.SaveChanges();
            

            return true;
        }

        public static string ProcessAction(ApplicationDbContext context,List<RoleEntity> list)
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

        public static Task<List<JGN_Roles>> LoadItems(ApplicationDbContext context, RoleEntity entity)
        {
            var collectionQuery = context.JGN_Roles.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);

            return LoadCompleteList(collectionQuery);
        }


        public static async Task<int> Count(ApplicationDbContext context,RoleEntity entity)
        {
            return await context.JGN_Roles.Where(returnWhereClause(entity)).CountAsync();
        }

        public static Task<List<JGN_Roles>> LoadCompleteList(IQueryable<JGN_Roles> query)
        {
            return query.Select(p => new JGN_Roles
            {
                id = p.id,
                rolename = p.rolename,
                created_at = p.created_at
            }).ToListAsync();
        }


        public static IQueryable<JGN_Roles> processOptionalConditions(IQueryable<JGN_Roles> collectionQuery, RoleEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_Roles>)collectionQuery.Sort(query.order);
            // skip logic
            if (query.pagenumber > 1)
                collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
            // take logic
            if (!query.loadall)
                collectionQuery = collectionQuery.Take(query.pagesize);


            return collectionQuery;
        }

        private static System.Linq.Expressions.Expression<Func<JGN_Roles, bool>> returnWhereClause(RoleEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Roles>(true);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.rolename.Contains(entity.term) || p.rolename.Contains(entity.term));


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
