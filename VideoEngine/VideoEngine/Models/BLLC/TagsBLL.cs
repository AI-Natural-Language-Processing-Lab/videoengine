using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Settings;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing tags or labels
/// </summary>
namespace Jugnoon.BLL
{
    public class TagsBLL
    {
        private UtilityBLL _utility = new UtilityBLL();

        public enum Types
        {
            All = 100,
            General = 5,
            Videos = 0
        };

        public enum TagType
        {
            Normal = 0,
            UserSearches = 1,
            All = 2
        };

        public enum TagLevel
        {
            High = 0,
            Medium = 1,
            Low = 2,
            All = 100
        };
       
        public static bool Add(ApplicationDbContext context,string title, Types type, int records, string term)
        {
            if (title == null || title.Length < 3)
                return false;

                var _entity = new JGN_Tags()
                {
                    title = title,
                    type = (byte)type,
                    records = records,
                    term = term
                };

                context.Entry(_entity).State = EntityState.Added;
               context.SaveChanges();
            
            
            return true;
        }

        public static bool Update(ApplicationDbContext context,JGN_Tags entity)
        {

                var item =  context.JGN_Tags
                     .Where(p => p.id == entity.id)
                     .FirstOrDefault<JGN_Tags>();

                if(item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    item.isenabled = entity.isenabled;
                    item.tag_level = entity.tag_level;
                    item.tag_type = entity.tag_type;

                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }


            return true;
        }

        public static bool Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {

                var item = context.JGN_Tags
                     .Where(p => p.id == ID)
                     .FirstOrDefault<JGN_Tags>();

                if(item != null)
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
                
            
            return true;
        }

        public static string ProcessAction(ApplicationDbContext context,List<TagEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "disable":
                            Update_Field_V3(context, entity.id, (byte)0, "isenabled");
                            break;
                        case "enable":
                            Update_Field_V3(context, entity.id, (byte)1, "isenabled");
                            break;
                        case "togglestatus":
                            Update_Field_V3(context, entity.id, (byte)entity.isenabled, "isenabled");
                            break;
                        case "high":
                            Update_Field_V3(context, entity.id, (byte)0, "tag_level");
                            break;
                        case "medium":
                            Update_Field_V3(context, entity.id, (byte)1, "tag_level");
                            break;
                        case "low":
                            Update_Field_V3(context, entity.id, (byte)2, "tag_level");
                            break;
                        case "delete":
                            Delete(context, (int)entity.id);
                            break;
                    }
                }
            }
            return "OK";
        }

        public static bool Add(ApplicationDbContext context,string title, Types type, int records, TagType tag_type, EnabledTypes isenabled, string term)
        {
            if (title == null || title.Length < 3)
                return false;
           
                var _entity = new JGN_Tags()
                {
                    title = title,
                    type = (byte)type,
                    records = records,
                    tag_type = (byte)tag_type,
                    isenabled = (byte)isenabled,
                    term = term
                };

                context.Entry(_entity).State = EntityState.Added;

               context.SaveChanges();

            return true;
        }
        
        public static bool Delete(ApplicationDbContext context, int id)
        {

                var entity = new JGN_Tags { id = id };
                 context.JGN_Tags.Attach(entity);
                 context.JGN_Tags.Remove(entity);
                context.SaveChanges();
            
            
            return true;
        }
        
        // Core function to process content related tags
        public static void Process_Tags(ApplicationDbContext context, string tags, Types type, TagType tag_type)
        {
            if(tags != null)
            {
                // process tags
                var tagItems = tags.ToString().Split(char.Parse(","));
                foreach(var JGN_Tags in tagItems)
                {
                    if(JGN_Tags.Length > 3)
                    {
                        // if JGN_Tags not already exist
                        string term = UtilityBLL.CleanSearchTerm(JGN_Tags.Trim());
                        if (Count(context,new TagEntity()
                        {
                            title = term,
                            tag_level = TagLevel.All,
                            type = type,
                            isenabled = EnabledTypes.All,
                            tag_type = tag_type
                        }).Result == 0)
                        {
                            // insert each JGN_Tags into tags table
                            int records = 1; // show one record in start by selected JGN_Tags
                            if (tag_type == TagType.UserSearches)
                                Add(context, JGN_Tags.Trim(), type, records, tag_type, EnabledTypes.Enabled, term);
                            else
                                Add(context, JGN_Tags.Trim(), type, records, term);
                        }
                        else
                        {
                            // JGN_Tags exist - update JGN_Tags records
                            // Due to performance issue, don't enable it on front line. update JGN_Tags statistics from control panel
                            // Process_Records(arr[j].ToString(), type);
                        }
                    }
                }
                
            }
            
        }
        
        // Script to check whether each JGN_Tags user input has exceed maximum limit of 35 characters.
        public static bool Validate_Tags(string tags)
        {
            if (tags == null)
                return true; // skip validation

            bool flag = true;
            // process tags
            if (tags.Contains(","))
            {
                string[] arr;
                arr = tags.ToString().Split(char.Parse(","));
                int j = 0;
                for (j = 0; j <= arr.Length - 1; j++)
                {
                    // if JGN_Tags length greater than 20 characters
                    if (arr[j].Length > 35)
                        flag = false;
                }
            }
            else
            {
                if (tags.Length > 35)
                    flag = false;
            }
            return flag;
        }

        public static async Task<List<JGN_Tags>> LoadItems(ApplicationDbContext context, TagEntity entity)
        {
            if (!entity.iscache
                || Configs.GeneralSettings.cache_duration == 0
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await FetchItems(context, entity);
            }
            else
            {
                string key = GenerateKey("lg_tag_", entity);
                var data = new List<JGN_Tags>();
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
                    data = (List<JGN_Tags>)SiteConfig.Cache.Get(key);
                }

                return data;
            }
        }

        private static async Task<List<JGN_Tags>> FetchItems(ApplicationDbContext context, TagEntity entity)
        {
            var collectionQuery = context.JGN_Tags.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return await LoadCompleteList(collectionQuery);
        }

        public static async Task<int> Count(ApplicationDbContext context, TagEntity entity)
        {
            if (!entity.iscache
                || Configs.GeneralSettings.cache_duration == 0
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context, entity);
            }
            else
            {
                string key = GenerateKey("cnt_tag", entity);
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

        private static async Task<int> CountRecords(ApplicationDbContext context, TagEntity entity)
        {
            return await context.JGN_Tags.Where(returnWhereClause(entity)).CountAsync();
        }
        private static string GenerateKey(string key, TagEntity entity)
        {
            return key + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" +
                entity.tag_level + "" + entity.tag_type + "" + entity.type + "" + entity.pagenumber + "" + entity.term;
        }
        private static async Task<List<JGN_Tags>> LoadCompleteList(IQueryable<JGN_Tags> query)
        {
            return await query.Select(p => new JGN_Tags
            {
                id = p.id,
                title = p.title,
                isenabled = p.isenabled,
                tag_level = p.tag_level,
                priority = p.priority,
                type = p.type,
                records = p.records,
                tag_type = p.tag_type,
                term = p.term
            }).ToListAsync();
        }


        private static IQueryable<JGN_Tags> processOptionalConditions(IQueryable<JGN_Tags> collectionQuery, TagEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_Tags>)collectionQuery.Sort(query.order);

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

        public static IQueryable<JGN_Tags> AddSortOption(IQueryable<JGN_Tags> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Tags>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Tags, bool>> returnWhereClause(TagEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Tags>(true);

            if (entity.id > 0)
               where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.title != "")
               where_clause = where_clause.And(p => p.title == entity.title);

            if (entity.term != "")
               where_clause = where_clause.And(p => p.title.Contains(entity.term) || p.term.Contains(entity.term));

            if (entity.type != Types.All)
               where_clause = where_clause.And(p => p.type == (byte)entity.type);

            if (entity.ispublic)
            {
                if (entity.isenabled != EnabledTypes.All)
                    where_clause = where_clause.And(p => p.isenabled == (byte)entity.isenabled);
            }
            

            if (entity.tag_level != TagLevel.All)
               where_clause = where_clause.And(p => p.tag_level == (byte)entity.tag_level);

            if (entity.tag_type != TagType.All)
               where_clause = where_clause.And(p => p.tag_type == (byte)entity.tag_type);


            return where_clause;
        }

        //*********************************************
        // User Search Tracking Script
        //********************************************
        public static void Add_Search_Query(ApplicationDbContext _context, string term, Types type)
        {           
            // User search term storing and tracking
            // Good for generating wide range of tags based on user searches.
            // Help to improve, present your website data to search engines.
            // Search Record Tracking, if user search term less than 25 characters
            if (Configs.GeneralSettings.store_searches)
            {
                if (term.Length > 3 && term.Length < 25)
                {
                    // validate user friendly searches
                    if (DictionaryBLL.Validate_Search_Word(_context, term.Trim()).Result)
                    {
                        // If search term already exist then stop adding dubplicate JGN_Tags
                        if (!term.Trim().Contains("@"))
                        {
                            var count = Count(_context, new TagEntity()
                            {
                                type = type,
                                tag_type = TagType.UserSearches, // search based generated JGN_Tags
                                ispublic = true
                            }).Result;
                            if (count == 0)
                            {
                                int records = 1; // show one record in start by selected JGN_Tags
                                Add(_context, term, type, records, TagType.UserSearches, EnabledTypes.Enabled, term.ToLower().Trim());
                            }
                        }

                    }
                }
            }
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
