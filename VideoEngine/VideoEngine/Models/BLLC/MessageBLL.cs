using System;
using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Settings;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
using Jugnoon.Models;
/// <summary>
/// Locations Business Layer Designed for Messages. 
/// </summary>

namespace Jugnoon.BLL
{
    public class MessageBLL
    {
        public static async Task<JGN_Messages_Recipents> sendMessage(ApplicationDbContext context, JGN_Messages_Recipents entity)
        {
            // save message
            var messageEntity = new JGN_Messages()
            {
                from_uid = entity.message.from_uid,
                subject = entity.message.subject,
                body = entity.message.body,
                reply_id = entity.message.reply_id
            };

            context.Entry(messageEntity).State = EntityState.Added;
            await context.SaveChangesAsync();
            entity.message.id = messageEntity.id;

            // save message recipents
            var messageRecipentEntity = new JGN_Messages_Recipents()
            {
                to_uid = entity.to_uid,
                messageid = entity.message.id,
                msg_sent = DateTime.Now
            };
            context.Entry(messageRecipentEntity).State = EntityState.Added;
            await context.SaveChangesAsync();
            entity.id = messageRecipentEntity.id;

            return entity;
        }

        public static void Delete(ApplicationDbContext context, long id)
        {
            var item = context.JGN_Messages_Recipents
                 .Where(p => p.id == id)
                 .FirstOrDefault();

            if (item != null)
            {
                item.msg_deleted = DateTime.Now;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void ReadMessage(ApplicationDbContext context, long id)
        {
            var item = context.JGN_Messages_Recipents
                 .Where(p => p.id == id)
                 .FirstOrDefault();

            if (item != null)
            {
                item.msg_read = DateTime.Now;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static async Task<List<JGN_Messages_Recipents>> LoadItems(ApplicationDbContext context, MessageEntity entity)
        {
            if (!entity.iscache
                || Configs.GeneralSettings.cache_duration == 0
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await FetchItems(context, entity);
            }
            else
            {
                string key = GenerateKey("ld_location", entity);
                var data = new List<JGN_Messages_Recipents>();
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
                    data = (List<JGN_Messages_Recipents>)SiteConfig.Cache.Get(key);
                }

                return data;
            }
        }

        private static async Task<List<JGN_Messages_Recipents>> FetchItems(ApplicationDbContext context, MessageEntity entity)
        {
            var collectionQuery = prepareQuery(context, entity);
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            if (entity.loadUserList)
                return await LoadUserList(collectionQuery);
            else 
                return await LoadCompleteList(collectionQuery);
           
        }

        public static async Task<int> Count(ApplicationDbContext context, MessageEntity entity)
        {
            if (!entity.iscache
                || Configs.GeneralSettings.cache_duration == 0
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context, entity);
            }
            else
            {
                string key = GenerateKey("cnt_message", entity);
                int records = 0;
                if (!SiteConfig.Cache.TryGetValue(key, out records))
                {
                    records = await CountRecords(context, entity);

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

        private static Task<int> CountRecords(ApplicationDbContext context, MessageEntity entity)
        {
            return prepareQuery(context, entity).CountAsync();
        }

        private static string GenerateKey(string key, MessageEntity entity)
        {
            return key + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" +
                entity.pagenumber + "" + entity.From + "" +
                entity.To + "" + entity.pagesize;
        }

        private static Task<List<JGN_Messages_Recipents>> LoadCompleteList(IQueryable<UserMessageEntity> query)
        {
            return query.Select(p => new JGN_Messages_Recipents
            {
                id = p.recipent.id,
                to_uid = p.recipent.to_uid,
                msg_read = p.recipent.msg_read,
                msg_sent = p.recipent.msg_sent,
                user = new ApplicationUser()
                {
                    Id = p.to.Id,
                    firstname = p.to.firstname,
                    lastname = p.to.lastname,
                    picturename = p.to.picturename
                },
                message = new JGN_Messages()
                {
                    id = p.message.id,
                    subject = p.message.subject,
                    body = p.message.body,
                    reply_id = p.message.reply_id,
                    user = new ApplicationUser()
                    {
                        Id = p.from.Id,
                        firstname = p.from.firstname,
                        lastname = p.from.lastname,
                        picturename = p.from.picturename
                    }
                }
            }).ToListAsync();
        }
     
        private static Task<List<JGN_Messages_Recipents>> LoadUserList(IQueryable<UserMessageEntity> query)
        {
            return query.Select(p => new JGN_Messages_Recipents
            {
                id = p.recipent.id,
                to_uid = p.recipent.to_uid,
                user = new ApplicationUser()
                {
                    Id = p.to.Id,
                    firstname = p.to.firstname,
                    lastname = p.to.lastname,
                    picturename = p.to.picturename
                },
                message = new JGN_Messages()
                {
                    id = p.message.id,
                    subject = p.message.subject
                }
            }).ToListAsync();
        }

        private static IQueryable<UserMessageEntity> prepareQuery(ApplicationDbContext context, MessageEntity entity)
        {
            return context.JGN_Messages
             .Join(context.AspNetusers,
                 message => message.from_uid,
                 from => from.Id, (message, from) => new { message, from })
             .Join(context.JGN_Messages_Recipents,
                 message => message.message.id,
                 recipent => recipent.messageid, (message, recipent) => new { message, recipent })
             .Join(context.AspNetusers,
                 recipent => recipent.recipent.to_uid,
                 to => to.Id, (recipent, to) =>
                 new UserMessageEntity
                 {
                     message = recipent.message.message,
                     recipent = recipent.recipent,
                     from = recipent.message.from,
                     to = to
                 })
             .Where(returnWhereClause(entity));
        }

        public static IQueryable<UserMessageEntity> processOptionalConditions(IQueryable<UserMessageEntity> collectionQuery, MessageEntity query)
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

        private static IQueryable<UserMessageEntity> AddSortOption(IQueryable<UserMessageEntity> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<UserMessageEntity>)collectionQuery.Sort(field, reverse);
        }
        private static System.Linq.Expressions.Expression<Func<UserMessageEntity, bool>> returnWhereClause(MessageEntity entity)
        {
            var where_clause = PredicateBuilder.New<UserMessageEntity>(true);

            // load message + all replies (threads)
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.message.id == entity.id || p.message.reply_id == entity.id);


            if (entity.To != "")
                where_clause = where_clause.And(p => p.to.Id == entity.To);

            if (entity.From != "")
                where_clause = where_clause.And(p => p.from.Id == entity.From);

            if (entity.reply_id > 0)
                where_clause = where_clause.And(p => p.message.reply_id == entity.reply_id);

            if (entity.isDeleted)
                where_clause = where_clause.And(p => p.recipent.msg_deleted != null);
            else
                where_clause = where_clause.And(p => p.recipent.msg_deleted == null);

            if (entity.isRead)
                where_clause = where_clause.And(p => p.recipent.msg_read != null);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.message.subject.Contains(entity.term)
                || p.message.body.Contains(entity.term));

            return where_clause;
        }
    }

    public class UserMessageEntity
    {
        public JGN_Messages_Recipents recipent { get; set; }
        public JGN_Messages message { get; set; }
        public ApplicationUser from { get; set; }
        public ApplicationUser to { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

