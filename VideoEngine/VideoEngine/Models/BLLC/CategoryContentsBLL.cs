using Jugnoon.Entity;
using System.Linq;
using System.Text;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
/// <summary>
///  Category Content Association Business Layer
/// </summary>
namespace Jugnoon.BLL
{
    public class CategoryContentsBLL
    {
        public enum Types
        {
            Videos = 0
        };

        public static void Add(ApplicationDbContext context, JGN_CategoryContents entity)
        {
                var _entity = new JGN_CategoryContents()
                {
                    categoryid = entity.categoryid,
                    contentid = entity.contentid,
                    type = entity.type
                };

                context.Entry(_entity).State = EntityState.Added;

               context.SaveChanges();
            
        }

        /// <summary>
        /// Delete all content associated categories
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentid"></param>
        /// <param name="type">void</param>
        public static void Delete(ApplicationDbContext context, long contentid, int type)
        {
              var itemsToDelete = context.JGN_CategoryContents.Where(x => x.contentid == contentid && x.type == type);
              context.JGN_CategoryContents.RemoveRange(itemsToDelete);
              context.SaveChanges();
        }

        /// <summary>
        ///  Delete specific content associated category
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentid"></param>
        /// <param name="categoryid"></param>
        /// <param name="type">void</param>
        public static void DeleteAssociatedCategory(ApplicationDbContext context, long contentid, long categoryid, int type)
        {
            var itemsToDelete = context.JGN_CategoryContents.Where(x => x.contentid == contentid && x.categoryid == categoryid && x.type == type);
            context.JGN_CategoryContents.RemoveRange(itemsToDelete);
            context.SaveChanges();
        }

        /// <summary>
        /// Returns list of category / content association data (no category detail) just reference category id, content id.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentid"></param>
        /// <param name="type"></param>
        /// <returns>List<JGN_CategoryContents></returns>
        public static List<JGN_CategoryContents> FetchContentCategories(ApplicationDbContext context, long contentid, int type)
        {
             return context.JGN_CategoryContents
                    .Where(p => p.contentid == contentid && p.type == type).ToList();
        }

        /// <summary>
        /// Returns list of category / content association data along with category information
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentid"></param>
        /// <param name="type"></param>
        /// <returns>List<JGN_CategoryContents></returns>
        public static Task<List<JGN_CategoryContents>> FetchContentCategoryList(ApplicationDbContext context, long contentid, int type)
        {
            return context.JGN_CategoryContents
              .Join(context.JGN_Categories,
                  content => content.categoryid,
                  category => category.id,
                  (content, category) => new
                  {
                      content,
                      category
                  })
                  .Where(p => p.content.contentid == contentid && p.content.type == type)
                  .Select(p => new JGN_CategoryContents
                  {
                      id = p.content.id,
                      category = new JGN_Categories()
                      {
                          id = p.category.id,
                          title = p.category.title,
                          term = p.category.term
                      }
                  }).ToListAsync();
        }

        /// <summary>
        /// Return list of category ids for selected content & type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contentid"></param>
        /// <param name="type"></param>
        /// <returns>string[]</returns>
        public static string[] FetchAssociatedCategories(ApplicationDbContext context, long contentid, byte type)
        {
            var categories = new StringBuilder();
           
            var list = context.JGN_CategoryContents
                    .Where(p => p.contentid == contentid && p.type == type).ToList();

            foreach(var item in list)
            {
                if (categories.ToString() != "")
                    categories.Append(",");
                categories.Append(item.categoryid);
            }

            return categories.ToString().Split(char.Parse(","));
        }

        /// <summary>
        /// Core function responsible for processing associated content categories (Specific for add or update content record operation)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="categories"></param>
        /// <param name="contentid"></param>
        /// <param name="type"></param>
        /// <param name="isupdate"></param>
        public static void ProcessAssociatedContentCategories(ApplicationDbContext context, string[] categories, long contentid, byte type, bool isupdate)
        {
           
            if (categories != null && contentid > 0)
            {
                if (!isupdate)
                {
                    // add new record
                    foreach (var category in categories)
                    {
                        Add(context, new JGN_CategoryContents()
                        {
                            contentid = contentid,
                            categoryid = Convert.ToInt16(category),
                            type = type
                        });
                    }
                }
                else
                {
                    // update record
                    var content_categories = FetchContentCategories(context, contentid, type);
                    if (categories.Length == 0 && content_categories.Count > 0)
                    {
                        // remove all category association for selected content as there is no selected category exist
                        Delete(context, contentid, type);
                    }
                    else if (categories.Length > 0 && content_categories.Count == 0)
                    {
                        // category selection exist but no category already associated or found in database
                        // add directly without mapping
                        foreach (var category in categories)
                        {
                            Add(context, new JGN_CategoryContents()
                            {
                                contentid = contentid,
                                categoryid = Convert.ToInt16(category),
                                type = type
                            });
                        }
                    }
                    else if (categories.Length > 0 && content_categories.Count > 0)
                    {
                        // category also selected and also associated
                        // i: cleanup process (check if associated category exist in database but not in returned list, remove such category
                        foreach (var c_category in content_categories)
                        {
                            if (!isDbCategoryExist(c_category, categories))
                            {
                                DeleteAssociatedCategory(context, contentid, c_category.categoryid, type);
                            }
                        }
                        foreach (var category in categories)
                        {
                            if (!isReceivingCategoryExist(category, content_categories))
                            {
                                Add(context, new JGN_CategoryContents()
                                {
                                    contentid = contentid,
                                    categoryid = Convert.ToInt16(category),
                                    type = type
                                });
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Check whether selected database category exist in list of associated returned category list, Usage Case (update record -> if not exist then delete it from db)
        /// </summary>
        /// <param name="content_category"></param>
        /// <param name="categories"></param>
        /// <returns>bool</returns>
        private static bool isDbCategoryExist(JGN_CategoryContents content_category, string[] categories)
        {
            var isexist = false;
            foreach (var category in categories)
            {
                if (content_category.categoryid.ToString() == category)
                    isexist = true;
            }
            return isexist;
        }

        /// <summary>
        /// Check whether selected category already exist in associated category list within database. Usage Case (update record -> if not exist then add it)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="content_categories"></param>
        /// <returns>bool</returns>
        private static bool isReceivingCategoryExist(string category, List<JGN_CategoryContents> content_categories)
        {
            var isexist = false;
            foreach (var c_category in content_categories)
            {
                if (c_category.categoryid.ToString() == category)
                    isexist = true;
            }
            return isexist;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
