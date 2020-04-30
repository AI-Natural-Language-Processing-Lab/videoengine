using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Jugnoon.Entity;
using Jugnoon.Utility;
using Jugnoon.Framework;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
using Jugnoon.Models;
/// <summary>
/// Business Layer: For processing website own comments data
/// </summary>
namespace Jugnoon.BLL
{
    public class CommentsBLL
    {
        public enum Types
        {
            Videos = 0
        };

        public static JGN_Comments Process(ApplicationDbContext context, JGN_Comments entity)
        {

            if (entity.id == 0)
            {
                var userEntity = new JGN_Comments()
                {
                    contentid = entity.contentid,
                    userid = entity.userid,
                    message = UtilityBLL.processNull(entity.message, 0),
                    created_at = entity.created_at,
                    isenabled = entity.isenabled,
                    type = entity.type,
                    points = entity.points,
                    isapproved = entity.isapproved,
                    levels = entity.levels,
                    replyid = entity.replyid,
                    replies = entity.replies
                };
                context.Entry(userEntity).State = EntityState.Added;
                context.SaveChanges();
                entity.id = userEntity.id;
            }
            else
            {
                var item = context.JGN_Comments
                       .Where(p => p.id == entity.id)
                       .FirstOrDefault<JGN_Comments>();

                item.message = entity.message;
                context.SaveChanges();
            }


            levelArr.Clear();
            string level = prepareLevel(context, (short)entity.id);
            Update_Field(context, entity.id, level, "level");

            return entity;
        }

        public static void Delete(ApplicationDbContext context, short id)
        {
            var entity = new JGN_Comments { id = id };
            context.JGN_Comments.Attach(entity);
            context.JGN_Comments.Remove(entity);
            context.SaveChanges();

        }

        /* Utility Functions */
        public static bool Update_Field(ApplicationDbContext context, long ID, dynamic Value, string FieldName)
        {
            var item = context.JGN_Comments
                  .Where(p => p.id == ID)
                  .FirstOrDefault<JGN_Comments>();

            if (item != null)
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

        public static Task<List<JGN_Comments>> LoadItems(ApplicationDbContext context, CommentEntity entity)
        {
            return FetchItems(context, entity);
        }

        private static Task<List<JGN_Comments>> FetchItems(ApplicationDbContext context, CommentEntity entity)
        {
            var collectionQuery = processOptionalConditions(prepareQuery(context, entity), entity);
            return LoadCompleteList(collectionQuery);
        }
        public static Task<int> Count(ApplicationDbContext context, CommentEntity entity)
        {
            return prepareQuery(context, entity).CountAsync();
        }


        private static string GenerateKey(string key, CommentEntity entity)
        {
            return key + UtilityBLL.ReplaceSpaceWithHyphin(entity.order.ToLower()) + "" + entity.pagenumber + "" 
                + entity.term + "" + entity.type + "" + entity.replyid + "" + entity.pagesize;
        }

        private static Task<List<JGN_Comments>> LoadCompleteList(IQueryable<CommentUserEntity> query)
        {
            return query.Select(p => new JGN_Comments
            {
                id = p.comment.id,
                message = p.comment.message,
                contentid = p.comment.contentid,
                userid = p.comment.userid,
                type = p.comment.type,
                created_at = p.comment.created_at,
                isenabled = p.comment.isenabled,
                points = p.comment.points,
                isapproved = p.comment.isapproved,
                replyid = p.comment.replyid,
                levels = p.comment.levels,
                replies = p.comment.replies,
                author = new ApplicationUser()
                {
                    firstname = p.user.firstname,
                    lastname = p.user.lastname,
                    UserName = p.user.UserName,
                    picturename = p.user.picturename
                }
            }).ToListAsync();
        }

        private static IQueryable<CommentUserEntity> prepareQuery(ApplicationDbContext context, CommentEntity entity)
        {
            return context.JGN_Comments
                .Join(context.AspNetusers,
                comment => comment.userid,
                user => user.Id,
                (comment, user) => new CommentUserEntity
                {
                    comment = comment,
                    user = user
                }).Where(returnWhereClause(entity));
        }

        public static IQueryable<CommentUserEntity> processOptionalConditions(IQueryable<CommentUserEntity> collectionQuery, CommentEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<CommentUserEntity>)collectionQuery.Sort(query.order);

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

        private static System.Linq.Expressions.Expression<Func<CommentUserEntity, bool>> returnWhereClause(CommentEntity entity)
        {
            var where_clause = PredicateBuilder.New<CommentUserEntity>(true);
            where_clause = where_clause.And(p => p.comment.type == entity.type);
            if (entity.id > 0)
                where_clause = where_clause.And(p => p.comment.id == entity.id);
            else
            {
                if (entity.excludedid > 0)
                    where_clause = where_clause.And(p => p.comment.id != entity.excludedid);

                if (entity.userid != "")
                    where_clause = where_clause.And(p => p.comment.userid == entity.userid);

                if (entity.isenabled != EnabledTypes.All)
                    where_clause = where_clause.And(p => p.comment.isenabled == (byte)entity.isenabled);

                if (entity.term != "")
                    where_clause = where_clause.And(p => p.comment.message.Contains(entity.term));

            }

            return where_clause;
        }

        public static string ProcessAction(ApplicationDbContext context, List<CommentEntity> list)
        {
            foreach (CommentEntity entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "enable":
                              Update_Field(context, entity.id, (byte)1, "isenabled");
                            break;

                        case "disable":
                             Update_Field(context, entity.id, (byte)0, "isenabled");
                            break;

                        case "delete":
                            Delete(context, (short)entity.id);
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

        public static void GenerateLevel(ApplicationDbContext context, long comment_id)
        {
            long ParentID = context.JGN_Comments
                .Where(u => u.id == comment_id)
                   .Select(u => u.replyid).ToList()[0];

            if (ParentID == comment_id)
            {
                levelArr.Add(ParentID.ToString());
            }
            else if (ParentID > 0)
            {
                levelArr.Add(ParentID);

                if (Count(context, new CommentEntity { replyid = ParentID }).Result > 0)
                {
                    GenerateLevel(context, ParentID);
                }
            }
        }
    }


    /// <summary>
    /// Entity used while joining data of two tables (comments, users) via Entity Framework (Linq)
    /// </summary>
    public class CommentUserEntity
    {
        public JGN_Comments comment { get; set; }
        public ApplicationUser user { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
