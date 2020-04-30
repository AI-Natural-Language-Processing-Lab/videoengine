using System;
using System.Collections.Generic;
using System.Text;
using Jugnoon.Entity;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;

namespace Jugnoon.BLL
{
    public class PackagesBLL
    {
        // Important Terms:

        // isenabled
        // 0: ............... : Disabled Package
        // 1: ............... : Enabled Package

        // Price
        // 0 ............... : Free
        // Any Vlue ........ :

        // Type
        // 0.................: Apply on all content types
        // any value ........: Apply on specific content types (0: video, 1: audio, 2: galleries, 3: photos, 4: blogs)

        // Credits
        // Any value ................: use to purchase premium contents

        // Package Type
        // ...................... 0: Free Packages
        // ...................... 1: Paid Packages
        // ...................... 2: UserBLLhip Subscription

        public static JGN_Packages Process(ApplicationDbContext context, JGN_Packages entity)
        {

            if (entity.id == 0)
            {
                var _entity = new JGN_Packages()
                {
                    name = entity.name,
                    description = entity.description,
                    isenabled = entity.isenabled,
                    price = entity.price,
                    created_at = DateTime.Now,
                    type = entity.type,
                    credits = entity.credits,
                    package_type = entity.package_type,
                    currency = entity.currency,
                    months = entity.months,
                    discount = entity.discount
                };
                context.Entry(_entity).State = EntityState.Added;
                context.SaveChanges();
                entity.id = _entity.id;
            }
            else
            {
                var item = context.JGN_Packages
                  .Where(p => p.id == entity.id)
                  .FirstOrDefault<JGN_Packages>();

                if (item != null)
                {
                    item.name = entity.name;
                    item.description = entity.description;
                    item.isenabled = entity.isenabled;
                    item.price = entity.price;
                    item.type = entity.type;
                    item.credits = entity.credits;
                    item.package_type = entity.package_type;
                    item.currency = entity.currency;
                    item.months = entity.months;
                    item.discount = entity.discount;

                    context.SaveChanges();
                }
            }

            return entity;
        }

        public static void Delete(ApplicationDbContext context,int id)
        {
            
                var entity = new JGN_Packages { id = (byte)id };
                context.JGN_Packages.Attach(entity);
                context.JGN_Packages.Remove(entity);
                context.SaveChanges();
            
        }

        public static Task<List<JGN_Packages>> Load(ApplicationDbContext context, PackageEntity entity)
        {
            var collectionQuery = context.JGN_Packages.Where(returnWhereClause(entity));
            return LoadCompleteList(processOptionalConditions(collectionQuery, entity));
        }

        public static int Count(ApplicationDbContext context, PackageEntity entity)
        {
            return context.JGN_Packages.Where(returnWhereClause(entity)).Count();
        }
        private static Task<List<JGN_Packages>> LoadCompleteList(IQueryable<JGN_Packages> query)
        {
            return query.Select(p => new JGN_Packages
            {
                id = p.id,
                name = p.name,
                description = p.description,
                isenabled = p.isenabled,
                price = p.price,
                created_at = (DateTime)p.created_at,
                type = p.type,
                credits = p.credits,
                package_type = p.package_type,
                currency = p.currency,
                months = p.months,
                discount = p.discount
            }).ToListAsync();
        }
        private static IQueryable<JGN_Packages> processOptionalConditions(IQueryable<JGN_Packages> collectionQuery, PackageEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_Packages>)collectionQuery.Sort(query.order);

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

        private static IQueryable<JGN_Packages> AddSortOption(IQueryable<JGN_Packages> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Packages>)collectionQuery.Sort(field, reverse);

        }

        private static System.Linq.Expressions.Expression<Func<JGN_Packages, bool>> returnWhereClause(PackageEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Packages>(true);
            if (entity.id > 0)
               where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
               where_clause = where_clause.And(p => p.name.Contains(entity.term) || p.description.Contains(entity.term));

            if (entity.name != "")
               where_clause = where_clause.And(p => p.name == entity.name);

            if (entity.isenabled != EnabledTypes.All)
               where_clause = where_clause.And(p => p.isenabled == (byte)entity.isenabled);

            if (entity.package_type != 3)
               where_clause = where_clause.And(p => p.package_type == entity.package_type);

            if (entity.type > -1)
               where_clause = where_clause.And(p => p.type == entity.type);

            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context,List<PackageEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {

                        case "delete":
                            Delete(context, (int)entity.id);

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
