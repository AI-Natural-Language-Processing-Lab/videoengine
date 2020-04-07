
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using Jugnoon.Entity;
using Jugnoon.Utility;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using LinqKit;
using System.Threading.Tasks;
/// <summary>
/// Business Layer: For processing whitelisted, blacklisted keywords and usernames for website
/// </summary>
namespace Jugnoon.BLL
{
    public enum DictionaryType
    {
        Content = 0,
        UserName = 1,
        All = 2
    };
    public class DictionaryBLL
    {
        // Note: DictionaryBLL Important Terms
        // Type:
        // .............. 0 :-> Content Restricted Values
        // .............. 1 :-> UserName Restricted Values

        // isrestrict:
        // .............. 1:-> screen data and no restriction mean screen data and highlight it on control panel
        // .............. 2:-> screen data and restrict it e.g book -> b**k

        private static int isRestrict = Settings.Configs.GeneralSettings.screen_content;
      
        public static JGN_Dictionary Add(ApplicationDbContext context, JGN_Dictionary data)
        {
            var entity = new JGN_Dictionary()
            {
                value = data.value,
                type = (byte)data.type
            };
            context.Entry(entity).State = EntityState.Added;

            context.SaveChanges();
            data.id = entity.id;

            Update_Values(context);

            return data;
        }

        public static void Update(ApplicationDbContext context, JGN_Dictionary entity)
        {
            var item = context.JGN_Dictionary
                .Where(p => p.id == entity.id)
                .FirstOrDefault();

            if(item != null)
            {
                item.value = entity.value;
                context.SaveChanges();
            }
        }

        public static bool CheckValue(ApplicationDbContext context, string value, int type)
        {
            var Flag = false;
            var count = context.JGN_Dictionary
                .Where(o => o.value == value && o.type == type)
                .Count();

            if (count > 0)
                Flag = true;
            else
                Flag = false;

            return Flag;
            
        }

        public static bool Delete(ApplicationDbContext context, int id)
        {
            var entity = new JGN_Dictionary { id = id };
            context.JGN_Dictionary.Attach(entity);
            context.JGN_Dictionary.Remove(entity);
            context.SaveChanges();
            return true;
        }

        public static void Update_Values(ApplicationDbContext context)
        {
            CacheData(context, 0);
            CacheData(context, 1);
        }

        public static void CacheData(ApplicationDbContext context, int type)
        {
            var key = "ld_screen_" + type;
            var values = Fetch_Values(context, type);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

            // Save data in cache.
            SiteConfig.Cache.Set(key, values, cacheEntryOptions);
        }

        public static async Task<List<JGN_Dictionary>> Return_Values(ApplicationDbContext context, int type)
        {
            string key = "ld_screen_" + type;
            var data = new List<JGN_Dictionary>();
            if (!SiteConfig.Cache.TryGetValue(key, out data))
            {
                data = await Fetch_Values(context, type);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                // Save data in cache.
                SiteConfig.Cache.Set(key, data, cacheEntryOptions);
            }
            else
            {
                data = (List<JGN_Dictionary>)SiteConfig.Cache.Get(key);
            }

            return data;
        }

        private static Task<List<JGN_Dictionary>> Fetch_Values(ApplicationDbContext context, int type)
        {            
            var query = context.JGN_Dictionary
                    .Where(p => p.type == type);

            return query.Select(p => new JGN_Dictionary
            {
                value = p.value
            }).ToListAsync();
        }

        // Core function to process screening data
        public static string Process_Screening(ApplicationDbContext context, string text)
        {
            if (text == null)
                return "";
                
            var _lst = Return_Values(context, 0).Result;
            if (_lst.Count == 0)
                return text;

            var keywords = new StringBuilder();
            if (_lst.Count > 0)
            {
                int i = 0;
                // create output like (apple|banana|mango)
                for (i = 0; i <= _lst.Count - 1; i++)
                {
                    if (_lst.Count == 1)
                        keywords.Append("(" + @"\b" + _lst[i].value.Trim() + @"\b)");
                    else if (i == 0)
                        keywords.Append("(" + @"\b" + _lst[i].value.Trim() + @"\b");
                    else if (i == _lst.Count - 1)
                        keywords.Append("|" + @"\b" + _lst[i].value.Trim() + @"\b)");
                    else
                        keywords.Append("|" + @"\b" + _lst[i].value.Trim() + @"\b");
                }
                string key = keywords.ToString();
                //// Swap out the ,<space> for pipes and add the braces
                Regex r = new Regex(", ?");
                //string keyword = "(" + r.Replace(keywords.ToString(), "|") + ")";

                // Get ready to replace the keywords
                MatchEvaluator _match = null;
                if (isRestrict == 0)
                    _match = new MatchEvaluator(MatchEval);
                else
                    _match = new MatchEvaluator(RestrictMatchEval);

                r = new Regex(keywords.ToString(), RegexOptions.IgnoreCase);

                //// Do the replace
                return r.Replace(text, _match);
            }
            else
            {
                return "";
            }
        }

        public static string MatchEval(Match match)
        {
            if (match.Groups[1].Success)
            {
                return "<span class=\"label label-danger\">" + match.ToString() + "</span>";
            }
            else
            {
                // no match
                return "";
            }
        }

        public static string RestrictMatchEval(Match match)
        {
            if (match.Groups[1].Success)
            {
                return UtilityBLL.Restrict_Word(match.ToString());
            }
            else
            {
                // no match
                return "";
            }
        }

        public static async Task<bool> Validate_Search_Word(ApplicationDbContext context, string text)
        {
            bool flag = true; // valid word
            var _lst = await Return_Values(context, 0);
            if (_lst.Count > 0)
            {
                int i = 0;
                // create output like (apple|banana|mango)
                for (i = 0; i <= _lst.Count - 1; i++)
                {
                    if (flag)
                    {
                        if (text.Contains(_lst[i].value))
                            flag = false;
                    }
                }
            }

            return flag;
        }

        public static async Task<List<JGN_Dictionary>> LoadItems(ApplicationDbContext context,DictionaryEntity entity)
        {
            var collectionQuery = context.JGN_Dictionary.Where(returnWhereClause(entity));
            collectionQuery = processOptionalConditions(collectionQuery, entity);
            return await LoadCompleteList(collectionQuery);
        }

        public static Task<int> Count(ApplicationDbContext context,DictionaryEntity entity)
        {
            return context.JGN_Dictionary.Where(returnWhereClause(entity)).CountAsync();
        }
   
        private static Task<List<JGN_Dictionary>> LoadCompleteList(IQueryable<JGN_Dictionary> query)
        {
            return query.Select(p => new JGN_Dictionary
            {
                id = p.id,
                type = p.type,
                value = p.value
            }).ToListAsync();
        }       

        private static IQueryable<JGN_Dictionary> processOptionalConditions(IQueryable<JGN_Dictionary> collectionQuery, DictionaryEntity query)
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

        public static IQueryable<JGN_Dictionary> AddSortOption(IQueryable<JGN_Dictionary> collectionQuery, string field, string direction)
        {
            var reverse = false;
            if (direction == "desc")
                reverse = true;

            return (IQueryable<JGN_Dictionary>)collectionQuery.Sort(field, reverse);
        }

        private static System.Linq.Expressions.Expression<Func<JGN_Dictionary, bool>> returnWhereClause(DictionaryEntity entity)
        {
            var where_clause = PredicateBuilder.New<JGN_Dictionary>(true);

            if (entity.id > 0)
                where_clause = where_clause.And(p => p.id == entity.id);

            if (entity.type != DictionaryType.All)
                where_clause = where_clause.And(p => p.type == (byte)entity.type);

            if (entity.term != "")
                where_clause = where_clause.And(p => p.value.Contains(entity.term));

            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context,List<DictionaryEntity> list)
        {
            foreach (DictionaryEntity entity in list)
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
