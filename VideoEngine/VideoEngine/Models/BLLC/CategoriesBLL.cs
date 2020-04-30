using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using Jugnoon.Framework;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Category Business Layer Designed for Categories & Locations. 
/// </summary>
namespace Jugnoon.BLL
{
    public class CategoryBLL
    {
        public enum Types
        {
            Videos = 0,
            Channels = 2,
            General = 3,
            AbuseReport = 4
        };


        public static async Task<short> Process(ApplicationDbContext context, JGN_Categories entity)
        {
            if (entity.picturename != null && entity.picturename != "")
            {
                if (entity.picturename.StartsWith("data:image"))
                {
                    // base 64 image
                    byte[] image = Convert.FromBase64String(entity.picturename.Replace("data:image/png;base64,", ""));
                    // create image name
                    string thumbFileName = UtilityBLL.ReplaceSpaceWithHyphin(entity.title) + Guid.NewGuid().ToString().Substring(0, 8) + ".png";

                    var path = SiteConfig.Environment.ContentRootPath + SystemDirectoryPaths.CategoryPhotosDirectoryPath;
                    if (File.Exists(path + "" + thumbFileName))
                        File.Delete(path + "" + thumbFileName);

                    // local storage
                    File.WriteAllBytes(path + "" + thumbFileName, image);
                    entity.picturename = await Helper.Aws.UploadPhoto(context, thumbFileName, path, Configs.AwsSettings.category_photos_directory);
                }
            }

            if (entity.id == 0)
            {
                var _entity = new JGN_Categories()
                {
                    title = entity.title,
                    parentid = entity.parentid,
                    description = UtilityBLL.processNull(entity.description, 0),
                    type = Convert.ToByte(entity.type),
                    priority = entity.priority,
                    isenabled = Convert.ToByte(entity.isenabled),
                    mode = Convert.ToByte(entity.mode),
                    term = UtilityBLL.processNull(entity.term, 0),
                    level = "",
                    picturename = UtilityBLL.processNull(entity.picturename, 0),
                    icon = UtilityBLL.processNull(entity.icon, 0)
                };
                context.Entry(_entity).State = EntityState.Added;
                await context.SaveChangesAsync();
                entity.id = _entity.id;
            }
            else
            {
                var item = await context.JGN_Categories
                       .Where(p => p.id == entity.id)
                       .FirstOrDefaultAsync();

                if (item != null)
                {
                    item.description = entity.description;
                    item.title = entity.title;
                    item.term = entity.term;
                    item.parentid = Convert.ToInt16(entity.parentid);
                    item.priority = entity.priority;
                    item.isenabled = Convert.ToByte(entity.isenabled);
                    item.mode = Convert.ToByte(entity.mode);
                    item.picturename = entity.picturename;
                    item.icon = entity.icon;
                    await context.SaveChangesAsync();
                }
            }
 
            levelArr.Clear();
            string level = prepareLevel(context, (short)entity.id);
            Update_Field(context, entity.id, level, "level");

            return entity.id;
        }

        public static void Delete(ApplicationDbContext context,short id)
        {
            var entity = new JGN_Categories { id = id };
            context.JGN_Categories.Attach(entity);
            context.JGN_Categories.Remove(entity);
            context.SaveChanges();
        }

        /* Utility Functions */
        public static bool Update_Field(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
                var item = context.JGN_Categories
                      .Where(p => p.id == ID)
                      .FirstOrDefault<JGN_Categories>();

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

        public static async Task<List<JGN_Categories>> LoadItems(ApplicationDbContext context,CategoryEntity entity)
        {
            if (!entity.iscache 
                || Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await FetchItems(context,entity);
            }
            else
            {
                string key = GenerateKey("ld_category", entity);
                var data = new List<JGN_Categories>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = await FetchItems(context,entity);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<JGN_Categories>)SiteConfig.Cache.Get(key);
                }

                return data;
            }
        }

        private static async Task<List<JGN_Categories>> FetchItems(ApplicationDbContext context,CategoryEntity entity)
        {
            var collectionQuery = context.JGN_Categories.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            if (entity.id > 0)
                return await LoadCompleteList(collectionQuery);
            else if (entity.issummary)
                return await LoadSummaryList(collectionQuery);
            else if (entity.isdropdown)
                return await LoadDropdownList(collectionQuery);
            else
                return await LoadCompleteList(collectionQuery);
        }

        public static async Task<int> Count(ApplicationDbContext context,CategoryEntity entity)
        {
            if (!entity.iscache 
                || Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_category", entity);
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

        private static Task<int> CountRecords(ApplicationDbContext context,CategoryEntity entity)
        {
            return context.JGN_Categories.Where(returnWhereClause(entity)).CountAsync();
        }
        
        private static string GenerateKey(string key, CategoryEntity entity)
        {
            return key +  UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + 
                entity.pagenumber + "" + entity.term + "" + entity.type + "" + 
                entity.parentid + "" + entity.pagesize;
        }

        private static Task<List<JGN_Categories>> LoadCompleteList(IQueryable<JGN_Categories> query)
        {
            return query.Select(p => new JGN_Categories
            {
                id = p.id,
                title = p.title,
                description = p.description,
                parentid = p.parentid,
                type = p.type,
                priority = p.priority,
                isenabled = p.isenabled,
                mode = p.mode,
                term = p.term,
                picturename = p.picturename,
                records = p.records,
                level = p.level
            }).ToListAsync();
        }
        private static Task<List<JGN_Categories>> LoadSummaryList(IQueryable<JGN_Categories> query)
        {
            return query.Select(p => new JGN_Categories
            {
                id = p.id,
                title = p.title,
                parentid = p.parentid,
                type = p.type,
                priority = p.priority,
                isenabled = p.isenabled,
                mode = p.mode,
                term = p.term,
                picturename = p.picturename,
                records = p.records,
                level = p.level
            }).ToListAsync();
        }
        private static Task<List<JGN_Categories>> LoadDropdownList(IQueryable<JGN_Categories> query)
        {
            return query.Select(p => new JGN_Categories
            {
                id = p.id,
                title = p.title,
                term = p.term,
                type = p.type,
                records = p.records,
                level = p.level
            }).ToListAsync();
        }

        public static IQueryable<JGN_Categories> processOptionalConditions(IQueryable<JGN_Categories> collectionQuery, CategoryEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<JGN_Categories>)collectionQuery.Sort(query.order);

            if ( query.id == 0)
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

        private static System.Linq.Expressions.Expression<Func<JGN_Categories, bool>> returnWhereClause(CategoryEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Categories>(true);
            
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);
            else
            {
                if (entity.excludedid > 0)
                    where_clause = where_clause.And(p => p.id != entity.excludedid);
                if (entity.parentid > -1)
                    where_clause = where_clause.And(p => p.parentid == entity.parentid);

                if (entity.ispublic)
                    where_clause = where_clause.And(p => p.isenabled == 1);
                else
                {
                    if (entity.isenabled != EnabledTypes.All)
                        where_clause = where_clause.And(p => p.isenabled == (byte)entity.isenabled);
                }

                if (entity.type != 100)
                    where_clause = where_clause.And(p => p.type == entity.type);

                if (entity.mode > 0)
                    where_clause = where_clause.And(p => p.mode == entity.mode);

                if (entity.term != "")
                    where_clause = where_clause.And(p => p.title.Contains(entity.term) || p.description.Contains(entity.term));

            }

            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context,List<CategoryEntity> list)
        {
            foreach (CategoryEntity entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "enable":
                            CategoryBLL.Update_Field(context, entity.id, (byte)1, "isenabled");
                            break;

                        case "disable":
                            CategoryBLL.Update_Field(context, entity.id, (byte)0, "isenabled");
                            break;

                        case "delete":
                            CategoryBLL.Delete(context, (short)entity.id);
                            if (!string.IsNullOrEmpty(entity.picturename))
                            {
                                string RootPath = SiteConfig.Environment.ContentRootPath;
                                string dirPath = RootPath + "/contents/categories/";
                                string thumbPath = dirPath + "temp/";
                                if (File.Exists(dirPath + entity.picturename))
                                    File.Delete(dirPath + entity.picturename);
                                if (File.Exists(thumbPath + entity.picturename))
                                    File.Delete(thumbPath + entity.picturename);
                            }
                            break;
                    }
                }
            }
            return "SUCCESS";
        }

        //*****************************************************************************
        // Utility Function
        //*****************************************************************************
        public static string return_hyphin(string level)
        {
            var str = new StringBuilder();
            foreach (char c in level)
            {
                if (c == '.')
                {
                    str.Append("- ");
                }
            }
            return str.ToString();
        }

        public static ArrayList levelArr = new ArrayList();
        public static string prepareLevel(ApplicationDbContext context, short CategoryID)
        {
            levelArr.Add(CategoryID.ToString());
            GenerateLevel(context, CategoryID);
            levelArr.Reverse();

            var Level = new StringBuilder();
            if (levelArr.Count > 0)
            {
                int counter = 0;
                for (int i = 0; i <= levelArr.Count - 1; i++)
                {
                    if (counter > 0)
                    {
                        Level.Append(".");
                    }
                    Level.Append(levelArr[i]);
                    counter += 1;
                }
            }
            return Level.ToString();
        }

        public static void GenerateLevel(ApplicationDbContext context, short CategoryID)
        {
            short ParentID = context.JGN_Categories.Where(u => u.id == CategoryID).Select(u => u.parentid).ToList()[0];
   
            if (ParentID == CategoryID)
            {
                levelArr.Add(ParentID.ToString());
            }
            else if (ParentID > 0)
            {
                levelArr.Add(ParentID);

                if (Count(context, new CategoryEntity { parentid = ParentID }).Result > 0)
                {
                    GenerateLevel(context, ParentID);
                }
            }
        }

        public static List<JGN_Categories> Fetch_Category_Names(ApplicationDbContext context, int type)
        {           
            var Query = context.JGN_Categories
                .Where(o => o.type == type);
            return Query.Select(p => new JGN_Categories
            {
                title = p.title,
                term = p.term

            }).ToList();
          
        }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
