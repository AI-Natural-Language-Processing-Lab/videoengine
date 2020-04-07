using System.Collections.Generic;
using System.Linq;
using Jugnoon.Framework;
using Jugnoon.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using LinqKit;
using System.Threading.Tasks;

namespace Jugnoon.BLL
{
    public class RolePermission
    {
        public static async Task<bool> Add(ApplicationDbContext context,JGN_RolePermissions entity)
        {
            var _entity = new JGN_RolePermissions()
            {
                roleid = entity.roleid,
                objectid = entity.objectid
            };

            context.Entry(_entity).State = EntityState.Added;
            await context.SaveChangesAsync();

            return true;
        }

        public static bool Delete(ApplicationDbContext context, short objectid)
        {
            var entity = new JGN_RolePermissions { id = objectid };
            context.JGN_RolePermissions.Attach(entity);
            context.JGN_RolePermissions.Remove(entity);
            context.SaveChanges();
            return true;
        }
        public static void DeleteRole(ApplicationDbContext context, short roleid)
        {
            context.JGN_RolePermissions.RemoveRange(context.JGN_RolePermissions.Where(x => x.roleid == roleid));
            context.SaveChanges();
        }
        public static string ProcessAction(ApplicationDbContext context,List<RoleDPermissionEntity> list)
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
            return "OK";
        }

        public static Task<List<JGN_RolePermissions>> LoadItems(ApplicationDbContext context, RoleDPermissionEntity entity)
        {
            var collectionQuery = context.JGN_RolePermissions
                .Join(context.JGN_RoleObjects,
                        permission => permission.objectid,
                        robject => robject.id,
                        (permission, robject) => new RoleObjectPermission
                        {
                            permission = permission,
                            objects = robject
                        }).Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return LoadCompleteList(collectionQuery);
        }


        public static int Count(ApplicationDbContext context,RoleDPermissionEntity entity)
        {
                return context.JGN_RolePermissions
                    .Join(context.JGN_RoleObjects,
                         permission => permission.objectid,
                         robject => robject.id,
                         (permission, robject) => new RoleObjectPermission
                         {
                             permission = permission,
                             objects = robject
                         }).Where(returnWhereClause(entity)).Count();
              
        }


        private static Task<List<JGN_RolePermissions>> LoadCompleteList(IQueryable<RoleObjectPermission> query)
        {
            return query.Select(p => new JGN_RolePermissions
            {
                id = p.permission.id,
                roleid = p.permission.roleid,
                objectid = p.permission.objectid,
                robject = new JGN_RoleObjects
                {
                    id = p.objects.id,
                    objectname = p.objects.objectname,
                    description = p.objects.description,
                    uniqueid = p.objects.uniqueid
                }
            }).ToListAsync();
        }

        private static IQueryable<RoleObjectPermission> processOptionalConditions(IQueryable<RoleObjectPermission> collectionQuery, RoleDPermissionEntity query)
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

        private static IQueryable<RoleObjectPermission> AddSortOption(IQueryable<RoleObjectPermission> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<RoleObjectPermission>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<RoleObjectPermission, bool>> returnWhereClause(RoleDPermissionEntity entity)
        {
            var where_clause = PredicateBuilder.New<RoleObjectPermission>(true);

            if (entity.id > 0)
               where_clause = where_clause.And(p => p.permission.id == entity.id);

            if (entity.roleid > 0)
               where_clause = where_clause.And(p => p.permission.roleid == entity.roleid);

            return where_clause;
        }
    }

    public class RoleObjectPermission
    {
        public JGN_RolePermissions permission { get; set; }
        public JGN_RoleObjects objects { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
