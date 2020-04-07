using System;
using Jugnoon.Framework;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Jugnoon.Models;
using LinqKit;
using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Utility;

/// <summary>
/// Business Layer : For processing abuse / spam reports
/// </summary>
namespace Jugnoon.BLL
{
    public class AbuseReport
    {
       
        public enum Types
        {
            Videos = 0,
            Channels = 1,
            UserMessages = 2,
            VideoComments = 4,
            Photos = 5,
            PhotoComments = 6,
            Blogs = 7,
            BlogComments = 8,
            Albums = 9,
            qa = 11,
            ANAnswer = 100,
            Forums = 12,
            Polls = 13,
            Classified = 14,
            Products = 15
        };

        public enum Status
        {
            NotReviewed = 0,
            Reviewed = 1,
            Closed = 2,
            All = 3
        };

        public static async Task<JGN_AbuseReports> Add(ApplicationDbContext context,long ContentID, string userid, string IPAddress, string Reason, int Type)
        {
            var entry = new JGN_AbuseReports()
            {
                contentid = ContentID,
                userid = userid,
                ipaddress = IPAddress,
                reason = Reason,
                type = (byte)Type,
                created_at = DateTime.Now
            };
            context.Entry(entry).State = EntityState.Added;

            await context.SaveChangesAsync();

            return entry;
        }

        public static async Task Update(ApplicationDbContext context, JGN_AbuseReports entity)
        {
            var item = context.JGN_AbuseReports
                    .Where(p => p.id == entity.id)
                    .FirstOrDefault();

            if (item != null)
            {
                item.status = entity.status;
                item.review_comment  = UtilityBLL.processNull(entity.review_comment, 0);
              
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

        }

        public static async Task Delete(ApplicationDbContext context, long ContentID, int Type)
        {           
            var entity = new JGN_AbuseReports { contentid = ContentID, type = (byte)Type };
            context.JGN_AbuseReports.Attach(entity);
            context.JGN_AbuseReports.Remove(entity);
            await context.SaveChangesAsync();
        }

        // delete single reports related to content
        public static async Task Delete(ApplicationDbContext context, long Id)
        {
            var entity = new JGN_AbuseReports { id = Id };
            context.JGN_AbuseReports.Attach(entity);
            context.JGN_AbuseReports.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<bool> Check_UserName(ApplicationDbContext context, string userid, long ContentID, int Type)
        {
            if (await context.JGN_AbuseReports
                .Where(p => p.userid == userid && p.contentid == ContentID && p.type == Type)
                .CountAsync() > 0)
                return true;
            else
                return false;
        }

        public static async Task<bool> Check_IPAddress(ApplicationDbContext context, string ipaddress, long ContentID, int Type)
        {
            if (await context.JGN_AbuseReports
                .Where(p => p.ipaddress == ipaddress && p.contentid == ContentID && p.type == Type)
                .CountAsync() > 0)
                return true;
            else
                return false;
        }
              

        public static async Task<int> Count(ApplicationDbContext context,long ContentID, int Type)
        {
            return await context.JGN_AbuseReports
                    .Where(p => p.contentid == ContentID && p.type == Type)
                    .CountAsync();
        }


        #region Core Loading Script

        public static Task<List<JGN_AbuseReports>> LoadItems(ApplicationDbContext context, AbuseEntity entity)
        {
            var collectionQuery = processOptionalConditions(prepareQuery(context, entity), entity);
            return LoadCompleteList(collectionQuery);
        }
        
        public static Task<int> Count(ApplicationDbContext context, AbuseEntity entity)
        {
            return prepareQuery(context, entity).CountAsync();
        }

        private static IQueryable<AbuseQueryEntity> prepareQuery(ApplicationDbContext context, AbuseEntity entity)
        {
            return context.JGN_AbuseReports
                .Join(context.AspNetusers,
                    abuse => abuse.userid,
                    user => user.Id,
                    (abuse, user) => new AbuseQueryEntity
                    {
                        abusereports = abuse,
                        user = user
                    }).Where(returnWhereClause(entity));
        }

 
        private static Task<List<JGN_AbuseReports>> LoadCompleteList(IQueryable<AbuseQueryEntity> query)
        {
            return query.Select(p => new JGN_AbuseReports
            {
                id = p.abusereports.id,
                contentid = p.abusereports.contentid,
                userid = p.abusereports.userid,
                ipaddress = p.abusereports.ipaddress,
                reason = p.abusereports.reason,
                created_at = p.abusereports.created_at,
                type = p.abusereports.type,
                report_user = new ApplicationUser()
                {
                    firstname = p.user.firstname,
                    lastname = p.user.lastname,
                    UserName = p.user.UserName,
                    picturename = p.user.picturename
                }
            }).ToListAsync();
        }
               
        public static IQueryable<AbuseQueryEntity> processOptionalConditions(IQueryable<AbuseQueryEntity> collectionQuery, AbuseEntity query)
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

        private static IQueryable<AbuseQueryEntity> AddSortOption(IQueryable<AbuseQueryEntity> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<AbuseQueryEntity>)collectionQuery.Sort(field, reverse);

        }
        public static System.Linq.Expressions.Expression<Func<AbuseQueryEntity, bool>> returnWhereClause(AbuseEntity entity)
        {
            var where_clause = PredicateBuilder.New<AbuseQueryEntity>(true);

            if (entity.contentid > 0)
                where_clause = where_clause.And(p => p.abusereports.contentid != entity.contentid);

            where_clause = where_clause.And(p => p.abusereports.type == (byte)entity.type);

            if (entity.status != Status.All)
                where_clause = where_clause.And(p => p.abusereports.status == (byte)entity.status);

            return where_clause;
        }

        #endregion

        public static async Task<string> ProcessAction(ApplicationDbContext context, List<AbuseEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        
                        case "delete":
                            // rewrote its logic
                            await Delete(context, entity.id);
                            break;
                        
                    }
                }
            }
            return "OK";
        }

    }

    public class AbuseQueryEntity
    {
        public JGN_AbuseReports abusereports { get; set; }
        public ApplicationUser user { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
