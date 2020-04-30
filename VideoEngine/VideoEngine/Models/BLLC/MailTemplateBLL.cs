using System;
using System.Collections.Generic;
using System.Text;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Settings;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing mail templates
/// </summary>
namespace Jugnoon.BLL
{
    public enum MailTemplateTypes
    {
        General = 0,
        Members = 1,
        Videos = 2

    };

    public class MailTemplateBLL
    {
        public static async Task<JGN_MailTemplates> Add(ApplicationDbContext context, JGN_MailTemplates entity)
        {
            var _entity = new JGN_MailTemplates()
            {
                templatekey = entity.templatekey,
                description = entity.description,
                subjecttags = entity.subjecttags,
                tags = entity.tags,
                subject = entity.subject,
                contents = entity.contents,
                type = entity.type
            };

            context.Entry(_entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            entity.id = _entity.id;
          
            return entity;
        }
    
        public static Task<List<JGN_MailTemplates>> Get_Template(ApplicationDbContext context, string templatekey)
        {
            var key = "ld_mailtemplates_" + templatekey;
            var data = new List<JGN_MailTemplates>();
            if (!SiteConfig.Cache.TryGetValue(key, out data))
            {
                data = Fetch_Record(context, templatekey).Result;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                // Save data in cache.
                SiteConfig.Cache.Set(key, data, cacheEntryOptions);
            }
            else
            {
                data = (List<JGN_MailTemplates>)SiteConfig.Cache.Get(key);
            }
            return Task.Run(() => data);
        }

        public static Task<List<JGN_MailTemplates>> Fetch_Record(ApplicationDbContext context,string templatekey)
        {
            return context.JGN_MailTemplates
                    .Where(p => p.templatekey == templatekey)
                    .Select(p => new JGN_MailTemplates()
                    {
                        subject = p.subject,
                        contents = p.contents
                    }).ToListAsync();
        }

        public static bool Update_Record(ApplicationDbContext context, int id, string subject, string description, string contents, string tags, string subjecttags)
        {
            var item = context.JGN_MailTemplates
                    .Where(p => p.id == id)
                    .FirstOrDefault<JGN_MailTemplates>();

            if(item != null)
            {
                item.subject = UtilityBLL.processNull(subject, 0);
                item.description = UtilityBLL.processNull(description, 100);
                item.contents = UtilityBLL.processNull(contents, 0);
                item.tags = tags;
                item.subjecttags = subjecttags;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return true;
        }


        public static Task<List<JGN_MailTemplates>> Load(ApplicationDbContext context,MailTemplateEntity entity)
        {
            var collectionQuery = context.JGN_MailTemplates.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            if (entity.id > 0 || !entity.issummary)
                return LoadCompleteList(collectionQuery);
            else
                return LoadSummaryList(collectionQuery);
        }

        public static async Task<int> Count(ApplicationDbContext context, MailTemplateEntity entity)
        {
            if (!entity.iscache || Configs.GeneralSettings.cache_duration == 0  || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_mailtemplate", entity);
                int records = 0;
                if (!SiteConfig.Cache.TryGetValue(key, out records))
                {
                    records = await CountRecords(context,entity);

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
        public static bool Delete(ApplicationDbContext context, short id)
        {          
            var entity = new JGN_MailTemplates { id = id };
            context.JGN_MailTemplates.Attach(entity);
            context.JGN_MailTemplates.Remove(entity);
            context.SaveChanges();
            return true;
        }

        private static Task<int> CountRecords(ApplicationDbContext context, MailTemplateEntity entity)
        {
            return context.JGN_MailTemplates.Where(returnWhereClause(entity)).CountAsync();
        }

        private static string GenerateKey(string key, MailTemplateEntity entity)
        {
            return key + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + entity.pagenumber;
        }

        private static Task<List<JGN_MailTemplates>> LoadCompleteList(IQueryable<JGN_MailTemplates> query)
        {
            return query.Select(p => new JGN_MailTemplates
            {
                id = p.id,
                templatekey = p.templatekey,
                description = p.description,
                tags = p.tags,
                subjecttags = p.subjecttags,
                subject = p.subject,
                contents = p.contents,
                type = p.type
            }).ToListAsync();
        }

        private static Task<List<JGN_MailTemplates>> LoadSummaryList(IQueryable<JGN_MailTemplates> query)
        {
            return query.Select(p => new JGN_MailTemplates
            {
                id = p.id,
                templatekey = p.templatekey,
                description = p.description,
                tags = p.tags,
                subjecttags = p.subjecttags,
                subject = p.subject,
                contents = p.contents,
                type = p.type
            }).ToListAsync();
        }
        
        private static IQueryable<JGN_MailTemplates> processOptionalConditions(IQueryable<JGN_MailTemplates> collectionQuery, MailTemplateEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_MailTemplates>)collectionQuery.Sort(query.order);
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

        private static System.Linq.Expressions.Expression<Func<JGN_MailTemplates, bool>> returnWhereClause(MailTemplateEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_MailTemplates>(true);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.subject.Contains(entity.term) || p.templatekey.Contains(entity.term) || p.description.Contains(entity.term) || p.contents.Contains(entity.term));

            if (entity.templatekey != "")
                where_clause = where_clause.And(p => p.templatekey == entity.templatekey);

            if (entity.type != "-1" && entity.id == 0)
                where_clause = where_clause.And(p => p.type == entity.type);


            return where_clause;
        }

        public static bool CheckTemplate(ApplicationDbContext context, string key)
        {
            var Flag = false;
            var count = context.JGN_MailTemplates
                .Where(o => o.templatekey == key)
                .Count();

            if (count > 0)
                Flag = true;
            else
                Flag = false;

            
            return Flag;

        }

        public static string ProcessAction(ApplicationDbContext context,List<MailTemplateEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {

                        case "delete":
                            Delete(context, Convert.ToInt16(entity.id));
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
