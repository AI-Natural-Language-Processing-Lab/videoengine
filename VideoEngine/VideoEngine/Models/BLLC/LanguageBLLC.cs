using System;
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
/// Business Layer: For processing multi-language user-interface, culture management
/// </summary>
namespace Jugnoon.BLL
{

    public class LanguageBLLC
    {

        // Note: Language Important Terms

        // isDefault:
        // ............ 0:- Normal Item
        // ............ 1:- Default Language (Only one can possible)

        // isSelected
        // ............ 0:- Normal Item
        // ............ 1:- Selected Language (Multiple options can be possible)
        public static string Return_Current_CultureName()
        {
            var culture = Utility.Helper.Cookie.ReadCookie("CultureInfo");
            if (culture != null && culture != null)
                return culture;
            else
                return Configs.GeneralSettings.default_culture;
        }

        public static async Task<JGN_Languages> Process(ApplicationDbContext context, JGN_Languages lang)
        {
            if (lang.id == 0)
            {
                var entity = new JGN_Languages()
                {
                    culturename = lang.culturename,
                    language = lang.language,
                    region = lang.region,
                    isselected = (byte)lang.isselected
                };

                context.Entry(entity).State = EntityState.Added;

                await context.SaveChangesAsync();

                lang.id = entity.id;

            }
            else
            {
                var item = await context.JGN_Languages
                .Where(p => p.id == lang.id)
                .FirstOrDefaultAsync();

                if (item != null)
                {
                    item.culturename = lang.culturename;
                    item.language = lang.language;
                    item.region = lang.region;
                    item.isselected = (byte)lang.isselected;

                    await context.SaveChangesAsync();
                }
            }

            return lang;
        }

        public static void Delete(ApplicationDbContext context,int id)
        {
                var entity = new JGN_Languages { id = (short)id };
                context.JGN_Languages.Attach(entity);
                context.JGN_Languages.Remove(entity);
                context.SaveChanges();
            
        }

        public static bool Update_Value(ApplicationDbContext context, int id, string fieldname, dynamic Value)
        {
            var item = context.JGN_Languages
                    .Where(p => p.id == id)
                    .FirstOrDefault();

            if (item != null)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (prop.Name.ToLower() == fieldname.ToLower())
                    {
                        prop.SetValue(item, Value);
                    }
                }
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return true;
        }

        public static bool Reset_IsDefault(ApplicationDbContext context)
        {            
            var JGN_Languages = context.JGN_Languages.ToList();
            JGN_Languages.ForEach(m => m.isdefault = 0);
            context.SaveChanges();
            return true;
        }

        public static async Task<List<JGN_Languages>> LoadItems(ApplicationDbContext context, LanguageEntity entity)
        {
            string key = "lang_" + entity.id + "_" + entity.isselected + "_" + entity.isdefault;
            var data = new List<JGN_Languages>();
            if (!SiteConfig.Cache.TryGetValue(key, out data))
            {
                data = await _Load(context, entity);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                // Save data in cache.
                SiteConfig.Cache.Set(key, data, cacheEntryOptions);
            }
            else
            {
                data = (List<JGN_Languages>)SiteConfig.Cache.Get(key);
            }
            return data;

        }
       
        private static Task<List<JGN_Languages>> _Load(ApplicationDbContext context, LanguageEntity entity)
        {
            var collectionQuery = context.JGN_Languages.Where(returnWhereClause(entity));
            return LoadCompleteList(processOptionalConditions(collectionQuery, entity));
        }

        public static async Task<int> Count(ApplicationDbContext context, LanguageEntity entity)
        {
            if (!entity.iscache 
                || Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_language", entity);
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

        private static Task<int> CountRecords(ApplicationDbContext context, LanguageEntity entity)
        {
            return context.JGN_Languages.Where(returnWhereClause(entity)).CountAsync();
        }
        private static string GenerateKey(string key, LanguageEntity entity)
        {
            return key + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + entity.pagenumber;
        }
        private static Task<List<JGN_Languages>> LoadCompleteList(IQueryable<JGN_Languages> query)
        {
            return query.Select(p => new JGN_Languages
            {
                id = (short)p.id,
                culturename = p.culturename,
                language = p.language,
                region = p.region,
                isdefault = p.isdefault,
                isselected = p.isselected
            }).ToListAsync();
        }
       
        private static IQueryable<JGN_Languages> processOptionalConditions(IQueryable<JGN_Languages> collectionQuery, LanguageEntity query)
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

        private static IQueryable<JGN_Languages> AddSortOption(IQueryable<JGN_Languages> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Languages>)collectionQuery.Sort(field, reverse);

        }
        private static System.Linq.Expressions.Expression<Func<JGN_Languages, bool>> returnWhereClause(LanguageEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Languages>(true);

            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.isselected != 2)
                where_clause = where_clause.And(p => p.isselected == entity.isselected);
            if (entity.isdefault != 2)
                where_clause = where_clause.And(p => p.isdefault == entity.isdefault);
            if (entity.term != "")
                where_clause = where_clause.And(p => p.culturename.Contains(entity.term) || p.language.Contains(entity.term) || p.region.Contains(entity.term));

            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context,List<LanguageEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "default":
                            Reset_IsDefault(context);
                            Update_Value(context, (int)entity.id, "isdefault", (byte)1);
                            break;
                        case "selected":
                            // toggle selection
                            if (entity.isselected == 1)
                                entity.isselected = 0;
                            else
                                entity.isselected = 1;

                            Update_Value(context, (int)entity.id, "isselected", (byte)entity.isselected);
                            break;
                       
                        case "delete":
                            Delete(context, (int)entity.id);

                            break;
                    }
                }
            }
            return "OK";
        }

        public static string returnFlagCss(string culturename, List<JGN_Languages> _lst)
        {
            var css = "flag-us";
            switch(culturename)
            {
                case "en":
                    css = "flag-us";
                    break;
                case "en-US":
                    css = "flag-us";
                    break;
                case "ar-SA":
                    css = "flag-sa";
                    break;
                case "de-DE":
                    css = "flag-de";
                    break;
                case "es-ES":
                    css = "flag-es";
                    break;
                case "fr-FR":
                    css = "flag-fr";
                    break;
                case "it-IT":
                    css = "flag-it";
                    break;
                case "ja-JP":
                    css = "flag-jp";
                    break;
                case "pt-BR":
                    css = "flag-br";
                    break;
                case "ru-RU":
                    css = "flag-ru";
                    break;
                case "tr-TR":
                    css = "flag-tr";
                    break;
                case "zh-CHS":
                    css = "flag-ch";
                    break;
            }
            return css;
        }

        public static string returnLanguage(string culturename, List<JGN_Languages> _lst)
        {
            var value = "English";
            if (culturename.Contains("en"))
                return value;

            foreach (var item in _lst)
            {
                if (item.culturename.Contains(culturename))
                {
                    value = item.language;
                }
            }
            return value;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
