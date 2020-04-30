using Jugnoon.Entity;
using Jugnoon.Framework;
using Jugnoon.Utility;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Jugnoon.BLL;
using Jugnoon.Videos.Models;
using Jugnoon.Models;

namespace Jugnoon.Videos
{
    public enum MediaType
    {
        Videos = 0,
        Audio = 1,
        All = 2
    };

    public enum MediaSourceType
    {
        Own = 0,
        External = 1,
        Both = 2,
        All = 3
    };

    public enum PublishActionType
    {
        Pending = 0,
        Published = 1,
        All = 2
    };

    public enum MovieType
    {
        Clips = 0,
        Movies = 1,
        All = 2
    };

    public class VideoBLL
    {
        // Note: Important Video Terms

        // type:
        // .............. 0: Videos (Default)
        // .............. 1: Audio Files // If audio plugin enabled

        // isPrivate:
        // ........... 0: Public Video
        // ........... 1: Restricted Video
        // ........... 2: Unlisted Video

        // isEnabled:
        // ........... 0: Disabled Video
        // ........... 1: Enabled Video

        // isPublished:
        // ........... 0: Video not published or encoded yet
        // ........... 1: Video published or encoded properly

        // isReviewed:
        // ........... 0: Video not reviewed yet
        // ........... 1: Video reviewed by administrator

        // isComments:
        // ........... 0: Disabled comments
        // ........... 1: Enabled comments

        // isRatings:
        // ........... 0: Disabled ratings
        // ........... 1: Enabled ratings

        // isFeatured:
        // ........... 0: Normal Videos
        // ........... 1: Featured Videos   
        // ........... 3: Premium Videos

        // movieType:
        // ........... 0: Videos
        // ........... 1: Movies   
        // ........... 2: All

        // thumbPreview
        // if not empty it will act as thumb Videos rotator

        // isAdult:
        // ........... 0: Normal Video
        // ........... 1: Adult Video -> In case user redirected to adult content warning section. User must accept 18 year notification before continue.

        

        /// <summary>
        /// Main function responsible for saving processed Videos data in database.
        /// </summary>
        public static async Task<JGN_Videos> AddVideo(ApplicationDbContext context, SaveVideoInfo entity)
        {
            var Item = new JGN_Videos()
            {
                title = entity.title,
                description = entity.description,
                tags = entity.tags,
                id = entity.id,
                duration = entity.duration,
                originalvideofilename = entity.originalvideofilename,
                videofilename = entity.videofilename,
                thumbfilename = entity.thumbfilename,
                isenabled = entity.isenabled,
                isapproved = entity.isapproved,
                userid = entity.userid,
                type = entity.type,
                duration_sec = entity.dursec,
                isprivate = entity.privacy,
                authkey = "",
                pub_url = entity.pub_url,
                thumb_url = entity.thumb_url,
                org_url = entity.org_url,
                isexternal = entity.isexternal,
                youtubeid = entity.youtubeid,
                mode = 0,
                ispublished = entity.ispublished,
                errorcode = entity.errorcode,
                ipaddress = "",
                embed_script = entity.embed_script,
                categories = entity.categories
            };


            Item.isapproved = entity.isapproved;
            // Approved by Default
            if (Jugnoon.Settings.Configs.GeneralSettings.content_approval == 0)
            {
                // Moderator Review Required
                Item.isapproved = 0;
            }
            Item.isenabled = 1;

            Item = await Process_Info(context, Item);

            if (Item.tags != "")
            {
                var tag_type = TagsBLL.Types.Videos;
                /*if (Item.type == 1)
                    tag_type = TagsBLL.Types.Audio;*/
                TagsBLL.Process_Tags(context, Item.tags, tag_type, 0);
            }

            return Item;
        }

        // Enable HTTP Streaming
        public static bool EnableHttpStreaming { get { return false; } }
      
        // Enable Bandwidth Throttling
        public static bool EnableBandwidthTrottling { get { return true; } }
        // Set Bandwidth Rate e.g 100 -> 100kbps
        public static int MaxBandwidthSpeed { get { return 1500; } }
        // Set for Logged In User
        public static int MaxLogedInBandwidthSpeed { get { return 3000; } }

        // prepare bandwidth throtling parameter
        public static string GenerateBandwidthParam()
        {
            string maxParam = "";
            if (EnableBandwidthTrottling)
            {
                if (SiteConfig.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (MaxLogedInBandwidthSpeed > 0) // For logged in Users -> may be higher bandwidth speed
                        maxParam = "&max=" + MaxLogedInBandwidthSpeed;
                    else if (MaxBandwidthSpeed > 0) // For normal users -> may be lower bandwidth speed
                        maxParam = "&max=" + MaxBandwidthSpeed;
                }
                else
                {
                    if (MaxBandwidthSpeed > 0)  // For normal users -> may be lower bandwidth speed
                        maxParam = "&max=" + MaxBandwidthSpeed;
                }
            }
            return maxParam;
        }

        private static void Validation(JGN_Videos gal)
        {
            // validation
            if (gal.title != null && gal.title.Length > 100)
                gal.title= gal.title.Substring(0, 99);

            if (gal.tags != null && gal.tags.Length > 200)
                gal.tags = gal.tags.Substring(0, 199);
        }

      
        #region Data Manipulation
        public static void Process_Video_File_Information(ApplicationDbContext context, JGN_Videos entity)
        {
                var item = context.JGN_Videos
                     .Where(p => p.id == entity.id)
                     .FirstOrDefault<JGN_Videos>();

                if(item != null)
                {
                    item.videofilename = UtilityBLL.processNull(entity.videofilename, 0);
                    item.thumbfilename = UtilityBLL.processNull(entity.thumbfilename, 0);
                    item.originalvideofilename = UtilityBLL.processNull(entity.originalvideofilename, 0);

                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();
                }
        }

        // Check Videos for authorization
        public static bool Check(ApplicationDbContext context,long videoid, string userid)
        {
            if (videoid == 0 || userid == "")
                return false;

            bool flag = false;
           
            if (context.JGN_Videos.Where(p => p.id == videoid && p.userid == userid).Count() > 0)
                flag = true;

            return flag;
            
        }

        public static void Process_Video_Cloud_Information(ApplicationDbContext context, JGN_Videos entity)
        {
            var item = context.JGN_Videos
                    .Where(p => p.id == entity.id)
                    .FirstOrDefault<JGN_Videos>();

            if(item != null)
            {
                item.pub_url = UtilityBLL.processNull(entity.pub_url, 0);
                item.thumb_url = UtilityBLL.processNull(entity.thumb_url, 0);
                item.org_url = UtilityBLL.processNull(entity.org_url, 0);

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static async Task<JGN_Videos> Process_Info(ApplicationDbContext context, JGN_Videos entity)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.tags, 0));
                entity.description = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.description,0));
                entity.title= DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.title,0));
            }
           
            Validation(entity);

            //XSS CLEANUP
            // not working on medium trust
            //string content = Sanitizer.GetSafeHtmlFragment(txt_content.Value);
            // Compress Code => remove \r\t etc which create unnecessary line breaking issues
            if (entity.description != null && entity.description != "")
            {
                string content = UtilityBLL.CompressCode(entity.description);
                entity.description = UtilityBLL.Process_Content(content);
            }

            var isupdate = false;
            if (entity.id == 0)
            {
                try
                {
                    var _entity = new JGN_Videos()
                    {
                        userid = entity.userid,
                        title = UtilityBLL.processNull(entity.title, 0),
                        description = UtilityBLL.processNull(entity.description, 0),
                        tags = UtilityBLL.processNull(entity.tags, 0),
                        created_at = DateTime.Now,
                        isprivate = entity.isprivate,
                        mode = entity.mode,
                        videofilename = entity.videofilename,
                        thumbfilename = entity.thumbfilename,
                        isenabled = entity.isenabled,
                        duration_sec = entity.duration_sec,
                        duration = UtilityBLL.processNull(entity.duration, 0),
                        originalvideofilename = entity.originalvideofilename,
                        ispublished = entity.ispublished,
                        type = entity.type,
                        isratings = 1, // by default make rating available
                        iscomments = 1, // by default make comments available
                        isapproved = entity.isapproved,
                        pub_url = entity.pub_url,
                        org_url = entity.org_url,
                        thumb_url = entity.thumb_url,
                        embed_script = entity.embed_script,
                        isexternal = (byte)entity.isexternal,
                        ipaddress = entity.ipaddress,
                        youtubeid = entity.youtubeid,
                        authkey = entity.authkey,
                        id = entity.id,
                        preview_url = UtilityBLL.processNull(entity.preview_url, 0)
                    };

                    context.Entry(_entity).State = EntityState.Added;

                    await context.SaveChangesAsync();

                    // Content Associated Categories Processing
                    CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                        _entity.id, (byte)CategoryContentsBLL.Types.Videos, false);

                    entity.id = _entity.id;
                }
                catch(Exception ex)
                {
                    var error = ex.Message;
                }
                
            }
            else
            {
                var item = context.JGN_Videos
                        .Where(p => p.id == entity.id)
                        .FirstOrDefault<JGN_Videos>();

                if (item != null)
                {
                    item.title = entity.title;
                    item.description = entity.description;
                    item.tags = entity.tags;
                    item.isprivate = entity.isprivate;
                    item.mode = entity.mode;

                    await context.SaveChangesAsync();
                    isupdate = true;
                }

                // Content Associated Categories Processing
                CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                    entity.id, (byte)CategoryContentsBLL.Types.Videos, isupdate);
            }

            if (entity.id > 0)
            {
                if (entity.isapproved == 1 && entity.isprivate == 0 && entity.isenabled == 1 && entity.ispublished == 1)
                {
                    // update stats
                    await Refresh_User_Stats(context, entity.userid, entity.type);
                }
            }
            return entity;
        }

        public static async Task<JGN_Videos> Add_Movie(ApplicationDbContext context, JGN_Videos entity)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.tags, 0));
                entity.description = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.description, 0));
                entity.title = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.title, 0));
            }

            Validation(entity);

            //XSS CLEANUP
            // not working on medium trust
            //string content = Sanitizer.GetSafeHtmlFragment(txt_content.Value);
            // Compress Code => remove \r\t etc which create unnecessary line breaking issues
            if (entity.description != null && entity.description != "")
            {
                string content = UtilityBLL.CompressCode(entity.description);
                entity.description = UtilityBLL.Process_Content(content);
            }

            try
            {
                var _entity = new JGN_Videos()
                {
                    userid = entity.userid,
                    title = UtilityBLL.processNull(entity.title, 0),
                    description = UtilityBLL.processNull(entity.description, 0),
                    tags = UtilityBLL.processNull(entity.tags, 0),
                    created_at = DateTime.Now,
                    isprivate = 0,
                    videofilename = "",
                    thumbfilename = "",
                    isenabled = 1,
                    duration_sec = 0,
                    duration = UtilityBLL.processNull(entity.duration, 0),
                    originalvideofilename = "",
                    ispublished = entity.ispublished,
                    type = entity.type,
                    isratings = 1, // by default make rating available
                    iscomments = 1, // by default make comments available
                    isapproved = entity.isapproved,
                    pub_url = UtilityBLL.processNull(entity.pub_url, 0),
                    org_url = UtilityBLL.processNull(entity.org_url, 0),
                    thumb_url = UtilityBLL.processNull(entity.thumb_url, 0),
                    preview_url = UtilityBLL.processNull(entity.preview_url, 0),
                    coverurl = UtilityBLL.processNull(entity.coverurl, 0),
                    embed_script = "",
                    isexternal = 0,
                    ipaddress = "",
                    youtubeid = "",
                    authkey = "",
                    price = entity.price
                };

                context.Entry(_entity).State = EntityState.Added;

                await context.SaveChangesAsync();

                // Content Associated Categories Processing
                CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                    _entity.id, (byte)CategoryContentsBLL.Types.Videos, false);

                entity.id = _entity.id;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
           
            return entity;
        }

        public static async Task<JGN_Videos> Add_Embed_Video(ApplicationDbContext context, JGN_Videos entity)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.tags, 0));
                entity.description = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.description, 0));
                entity.title = DictionaryBLL.Process_Screening(context, UtilityBLL.processNull(entity.title, 0));
            }

            Validation(entity);

            //XSS CLEANUP
            // not working on medium trust
            //string content = Sanitizer.GetSafeHtmlFragment(txt_content.Value);
            // Compress Code => remove \r\t etc which create unnecessary line breaking issues
            if (entity.description != null && entity.description != "")
            {
                string content = UtilityBLL.CompressCode(entity.description);
                entity.description = UtilityBLL.Process_Content(content);
            }

            try
            {
                var _entity = new JGN_Videos()
                {
                    userid = entity.userid,
                    title = UtilityBLL.processNull(entity.title, 0),
                    description = UtilityBLL.processNull(entity.description, 0),
                    tags = UtilityBLL.processNull(entity.tags, 0),
                    created_at = DateTime.Now,
                    isprivate = 0,
                    videofilename = "",
                    thumbfilename = "",
                    isenabled = 1,
                    duration_sec = 0,
                    duration = "",
                    originalvideofilename = "",
                    ispublished = entity.ispublished,
                    type = entity.type,
                    isratings = 1, // by default make rating available
                    iscomments = 1, // by default make comments available
                    isapproved = entity.isapproved,
                    pub_url = "",
                    org_url = "",
                    thumb_url = UtilityBLL.processNull(entity.thumb_url, 0),
                    preview_url = "",
                    coverurl = "",
                    embed_script = UtilityBLL.processNull(entity.embed_script, 0),
                    isexternal = 1,
                    ipaddress = "",
                    youtubeid = "",
                    authkey = ""
                };

                context.Entry(_entity).State = EntityState.Added;

                await context.SaveChangesAsync();

                // Content Associated Categories Processing
                CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                    _entity.id, (byte)CategoryContentsBLL.Types.Videos, false);

                entity.id = _entity.id;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }

            return entity;
        }

        // Update Videos information - myaccount section - edit Videos
        public static bool Update_Video_Info(ApplicationDbContext context, JGN_Videos entity)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, entity.tags);
                entity.description = DictionaryBLL.Process_Screening(context, entity.description);
                entity.title= DictionaryBLL.Process_Screening(context, entity.title);
            }
            
            Validation(entity);
            
            if (entity.id == 0)
            {
                return false;
            }

            entity.description = UtilityBLL.Process_Content(entity.description);

                var item = context.JGN_Videos
                     .Where(p => p.id == entity.id)
                     .FirstOrDefault<JGN_Videos>();

                if(item != null)
                {
                    item.title = UtilityBLL.processNull(entity.title, 0);
                    item.description = UtilityBLL.processNull(entity.description, 0);
                    item.tags = UtilityBLL.processNull(entity.tags, 0);
                    item.isprivate = (byte)entity.isprivate;
                    item.iscomments = (byte)entity.iscomments;
                    item.isratings = (byte)entity.isratings;
                    
                    context.Entry(item).State = EntityState.Modified;
                    context.SaveChanges();

                    // Content Associated Categories Processing
                    CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                        item.id, (byte)CategoryContentsBLL.Types.Videos, true);
            }

            return true;
        }

        // Remove Video Data both from Database and folder (image, videos etc)
        public static async Task RemoveVideo(ApplicationDbContext context, long VideoID, int type)
        {
            string flvvideoname = "";
            string originalvideoname = "";
            string thumbname = "";
            string username = "";

            var _lst = LoadItems(context,new VideoEntity()
            {
                id = VideoID
            }).Result;
            if (_lst.Count > 0)
            {
                flvvideoname = _lst[0].videofilename;
                originalvideoname = _lst[0].originalvideofilename;
                thumbname = _lst[0].thumbfilename;
                username =_lst[0].userid;
            }

            // path of folder where Videos data (Videos, thumbs stored)
            string strPath = SiteConfig.Environment.ContentRootPath + "/contents/member/" + username;

            // Delete Original Video | MP3 Audio
            if (originalvideoname != "")
            {
                if (File.Exists(strPath + "/Default/" + originalvideoname))
                    File.Delete(strPath + "/Default/" + originalvideoname);
            }

            // Delete FLV Video
            if (flvvideoname != "")
            {
                if (type == 0)
                {
                    // Videos -> flv
                    if (File.Exists(strPath + "/FLV/" + flvvideoname))
                        File.Delete(strPath + "/FLV/" + flvvideoname);
                }
                else
                {
                    // audio -> mp3
                    if (File.Exists(strPath + "/MP3/" + flvvideoname))
                        File.Delete(strPath + "/MP3/" + flvvideoname);
                }


            }

            // Delete Images
            string thumb_start_index = "";
            if (thumbname.Contains("_"))
            {
                thumb_start_index = thumbname.Remove(thumbname.LastIndexOf("_"));
                int total_thumbs = 15;
                int i = 0;
                for (i = 0; i <= total_thumbs; i++)
                {
                    string _thumbname = "";
                    if (i > 9)
                        _thumbname = thumb_start_index + "_0" + i + ".jpg";
                    else
                        _thumbname = thumb_start_index + "_00" + i + ".jpg";

                    if (File.Exists(strPath + "/Thumbs/" + _thumbname))
                        File.Delete(strPath + "/Thumbs/" + _thumbname);

                }
            }

            // If Cloud Storage Exist
            if(flvvideoname != "" && flvvideoname.StartsWith("http"))
            {
                if (Jugnoon.Settings.Configs.AwsSettings.enable)
                {
                    MediaCloudStorage.DeleteVideoFiles(flvvideoname, originalvideoname, thumbname);
                }
            }
           
            // Delete Record
            await Delete(context, VideoID, username, type);
            
        }

        public static async Task<bool> Update_VideoInfo_Adm(ApplicationDbContext context, JGN_Videos entity, bool isadmin)
        {       
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, entity.tags);
                entity.description = DictionaryBLL.Process_Screening(context, entity.description);
                entity.title= DictionaryBLL.Process_Screening(context, entity.title);
            }
            
            Validation(entity);
            
            if (entity.id == 0)
            {
                return false;
            }

            entity.description = UtilityBLL.Process_Content(entity.description);

            // var context = SiteConfig.dbContext;


            var item = await context.JGN_Videos
                 .Where(p => p.id == entity.id)
                 .FirstOrDefaultAsync();

            if (item != null)
            {
                item.title = UtilityBLL.processNull(entity.title, 0);
                item.description = UtilityBLL.processNull(entity.description, 0);
                item.tags = UtilityBLL.processNull(entity.tags, 0);
                item.isprivate = entity.isprivate;
                item.iscomments = entity.iscomments;
                item.isratings = entity.isratings;

                // item.mode = (byte)entity.mode;
                if (isadmin)
                {
                    item.liked = entity.liked;
                    item.disliked = entity.disliked;
                    item.views = entity.views;
                    item.isadult = entity.isadult;
                    item.favorites = entity.favorites;
                    item.isapproved = entity.isapproved;
                    item.isenabled = entity.isenabled;
                    item.isfeatured = entity.isfeatured;
                }
              
                context.Entry(item).State = EntityState.Modified;

                await context.SaveChangesAsync();

                // Content Associated Categories Processing
                CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                    item.id, (byte)CategoryContentsBLL.Types.Videos, true);
            }
             
            return true;
        }
        
        public static async Task<bool> Update_VideoInfo_Adm_V2(ApplicationDbContext context, JGN_Videos entity)
        {
            if (Jugnoon.Settings.Configs.GeneralSettings.screen_content == 1)
            {
                entity.tags = DictionaryBLL.Process_Screening(context, entity.tags);
                entity.description = DictionaryBLL.Process_Screening(context, entity.description);
                entity.title = DictionaryBLL.Process_Screening(context, entity.title);
            }

            Validation(entity);

            if (entity.id == 0)
            {
                return false;
            }

            entity.description = UtilityBLL.Process_Content(entity.description);
            
            var item = await context.JGN_Videos
                    .Where(p => p.id == entity.id)
                    .FirstOrDefaultAsync();

            if (item != null)
            {
                item.title = UtilityBLL.processNull(entity.title, 0);
                item.description = UtilityBLL.processNull(entity.description, 0);
                item.tags = UtilityBLL.processNull(entity.tags, 0);
                item.liked = entity.liked;
                item.actors = UtilityBLL.processNull(entity.actors, 0);
                item.actresses = UtilityBLL.processNull(entity.actresses, 0);
                item.views = entity.views;

                context.Entry(item).State = EntityState.Modified;

                await context.SaveChangesAsync();

                // Content Associated Categories Processing
                CategoryContentsBLL.ProcessAssociatedContentCategories(context, entity.categories,
                    item.id, (byte)CategoryContentsBLL.Types.Videos, true);
            }

            return true;
        }

     
        public static async Task<bool> Delete(ApplicationDbContext context, long VideoID, string UserName, int type)
        {            
            // Delete Video
            await Delete(context, VideoID);

            // remove catgory references
            context.JGN_CategoryContents.RemoveRange(context.JGN_CategoryContents.Where(x => x.contentid == VideoID && x.type == (byte)CategoryContentsBLL.Types.Videos));

            if (UserName != "")
            {
                // update user stats
                await Refresh_User_Stats(context, UserName, type);
            }

            return true;
        }

        // Delete Videos where no statistic update is required
        // Mostly in case of youtube embed videos
        public static async Task<bool> Delete(ApplicationDbContext context, long VideoID)
        {
            var entity = new JGN_Videos { id = VideoID };
            context.JGN_Videos.Attach(entity);
            context.JGN_Videos.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        
        #endregion
        
        #region Perform Actions

        
        public static bool Update_Field_V3(ApplicationDbContext context,long ID, dynamic Value, string FieldName)
        {
                var item = context.JGN_Videos
                     .Where(p => p.id == ID)
                     .FirstOrDefault<JGN_Videos>();

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

     
        public static string Get_Field_Value(ApplicationDbContext context,long ID, string FieldName)
        {
            string Value = "";
            // var context = SiteConfig.dbContext;
           
            
                var item = context.JGN_Videos
                     .Where(p => p.id == ID)
                     .FirstOrDefault<JGN_Videos>();

                if(item != null)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == FieldName.ToLower())
                        {
                            if (prop.GetValue(item, null) != null)
                                Value = prop.GetValue(item, null).ToString();
                        }
                    }
                }
                
            

            return Value;
        }

        public static long Get_Album_Media(ApplicationDbContext context, long AlbumID, int type)
        {
            long Value = 0;
 
                var query = context.JGN_Videos.Where(p => p.id == AlbumID && p.type == type && p.isenabled == 1 && p.isapproved == 1);
                if (query.Count() > 0)
                    Value =query.Max(p => p.id);
            
            return Value;
        }

        // process Videos rating
        public static double Post_Rating(ApplicationDbContext context,long VideoID, int current_rating, int total_ratings, double ratings)
        {
            // increment 
            total_ratings = total_ratings + 1;
            ratings = ratings + current_rating;
            double average_rating = (double)Math.Round(ratings / total_ratings, 2);
            // update rating
            // var context = SiteConfig.dbContext;
           
            
                var item = context.JGN_Videos
                     .Where(p => p.id == VideoID)
                     .FirstOrDefault<JGN_Videos>();

                item.ratings = (float)ratings;
                item.total_rating = total_ratings;
                item.avg_rating = (float)average_rating;

                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            
          
            return average_rating;
        }

        #endregion


        #region Core Loading Script
        public static async Task<List<JGN_Videos>> LoadItems(ApplicationDbContext context, VideoEntity entity)
        {
            if (entity.categoryname != ""
             || entity.categoryid > 0
             || entity.category_ids.Length > 0)
                return await CategorizeVideos.LoadItems(context, entity);
            else if (entity.loadfavorites)
                return await FavoritVideos.LoadItems(context, entity);
            else if (entity.loadliked)
                return await LikedVideos.LoadItems(context, entity);
            else if (entity.loadplaylist)
                return await PlaylistVideos.LoadItems(context, entity);
            else if (entity.loadabusereports)
                return await AbuseVideos.LoadItems(context, entity);
            else
                return await _LoadItems(context, entity);
        }

        private static async Task<List<JGN_Videos>> _LoadItems(ApplicationDbContext context,VideoEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0  
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {              
                return await FetchItems(context,entity);
            }
            else
            {
                string key = GenerateKey("lg_video_1_", entity);
                var data = new List<JGN_Videos>();
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
                    data = (List<JGN_Videos>)SiteConfig.Cache.Get(key);
                }
                return data;
            }
        }

        private static Task<List<JGN_Videos>> FetchItems(ApplicationDbContext context,VideoEntity entity)
        {
            var collectionQuery = processOptionalConditions(prepareQuery(context, entity), entity);
            if (entity.id > 0 || !entity.issummary)
                return LoadCompleteList(collectionQuery);
            else
                return LoadSummaryList(collectionQuery);
        }

        public static async Task<int> Count(ApplicationDbContext context, VideoEntity entity)
        {
            if (entity.categoryname != ""
             || entity.categoryid > 0
             || entity.category_ids.Length > 0)
                return await CategorizeVideos.Count(context, entity);
            else if (entity.loadfavorites)
                return await FavoritVideos.Count(context, entity);
            else if (entity.loadliked)
                return await LikedVideos.Count(context, entity);
            else if (entity.loadplaylist)
                return await PlaylistVideos.Count(context, entity);
            else if (entity.loadabusereports)
                return await AbuseVideos.Count(context, entity);
            else
                return await _Count(context, entity);
        }

        private static async Task<int> _Count(ApplicationDbContext context,VideoEntity entity)
        {
            if (!entity.iscache 
                || Jugnoon.Settings.Configs.GeneralSettings.cache_duration == 0 
                || entity.pagenumber > Jugnoon.Settings.Configs.GeneralSettings.max_cache_pages)
            {
                return await CountRecords(context,entity);
            }
            else
            {
                string key = GenerateKey("cnt_videos_1", entity);
                int records = 0;
                if (!SiteConfig.Cache.TryGetValue(key, out records))
                {
                    records = CountRecords(context,entity).Result;

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

        private static Task<int> CountRecords(ApplicationDbContext context, VideoEntity entity)
        {
            return prepareQuery(context, entity)
                  .CountAsync();
        }

        public static string GenerateKey(string key, VideoEntity vd)
        {
            var str = new StringBuilder();
            str.Append(key + "_" + vd.movietype + "" + vd.isfeatured + "" + 
                vd.type + "" + vd.categoryid + vd.categoryname + "" + vd.isadult + "" + vd.isexternal + "" + vd.datefilter + 
                "" + UtilityBLL.ReplaceSpaceWithHyphin(vd.order.ToLower()) + "" + vd.pagenumber + "" + vd.pagesize + "" + vd.loadall);
            if (vd.term != "")
                str.Append(UtilityBLL.ReplaceSpaceWithHyphin(vd.term.ToLower()));
            if (vd.tags != "")
                str.Append(UtilityBLL.ReplaceSpaceWithHyphin(vd.tags.ToLower()));

            return str.ToString();
        }
   
        private static Task<List<JGN_Videos>> LoadCompleteList(IQueryable<VideoQueryEntity> query)
        {
            return query.Select(p => new JGN_Videos
            {
                id = p.video.id,
                albumid = p.video.albumid,
                userid = p.video.userid,
                title = p.video.title,
                description = p.video.description,
                tags = p.video.tags,
                categories = p.video.categories,
                isadult = (byte)p.video.isadult,
                created_at = (DateTime)p.video.created_at,
                isenabled = p.video.isenabled,
                views = p.video.views,
                comments = p.video.comments,
                isfeatured = (byte)p.video.isfeatured,
                duration = p.video.duration,
                iscomments = p.video.iscomments,
                isapproved = p.video.isapproved,
                liked = p.video.liked,
                disliked = p.video.disliked,
                duration_sec = p.video.duration_sec,
                isprivate = p.video.isprivate,
                mode = p.video.mode,
                total_rating = p.video.total_rating,
                ratings = p.video.ratings,
                avg_rating = p.video.avg_rating,
                favorites = p.video.favorites,
                videofilename = p.video.videofilename,
                thumbfilename = p.video.thumbfilename,
                originalvideofilename = p.video.originalvideofilename,
                embed_script = p.video.embed_script,
                isratings = (byte)p.video.isratings,
                isexternal = (byte)p.video.isexternal,
                ispublished = p.video.ispublished,
                pub_url = p.video.pub_url,
                org_url = p.video.org_url,
                thumb_url = p.video.thumb_url,
                errorcode = p.video.errorcode,
                youtubeid = p.video.youtubeid,
                downloads = p.video.downloads,
                coverurl = p.video.coverurl,
                preview_url = p.video.preview_url,
                price = p.video.price,
                actors = p.video.actors,
                actresses = p.video.actresses,
                movietype = p.video.movietype,
                streamoutputs = p.video.streamoutputs,
                thumb_preview = p.video.thumb_preview,
                author = new ApplicationUser()
                {
                    Id = p.user.Id,
                    firstname = p.user.firstname,
                    lastname = p.user.lastname,
                    UserName = p.user.UserName,
                    picturename = p.user.picturename
                }
            }).ToListAsync();

        }

        private static Task<List<JGN_Videos>> LoadSummaryList(IQueryable<VideoQueryEntity> query)
        {
            return query.Select(prepareSummaryList()).ToListAsync();
        }

        public static System.Linq.Expressions.Expression<Func<VideoQueryEntity, JGN_Videos>> prepareSummaryList()
        {
            return p => new JGN_Videos
            {
                id = p.video.id,
                albumid = p.video.albumid,
                userid = p.video.userid,
                title = p.video.title,
                isadult = (byte)p.video.isadult,
                created_at = (DateTime)p.video.created_at,
                isenabled = p.video.isenabled,
                views = p.video.views,
                isfeatured = (byte)p.video.isfeatured,
                duration = p.video.duration,
                iscomments = p.video.iscomments,
                isapproved = p.video.isapproved,
                liked = p.video.liked,
                disliked = p.video.disliked,
                duration_sec = p.video.duration_sec,
                isprivate = p.video.isprivate,
                mode = p.video.mode,
                total_rating = p.video.total_rating,
                ratings = p.video.ratings,
                avg_rating = p.video.avg_rating,
                favorites = p.video.favorites,
                thumbfilename = p.video.thumbfilename,
                isratings = (byte)p.video.isratings,
                isexternal = (byte)p.video.isexternal,
                ispublished = p.video.ispublished,
                thumb_url = p.video.thumb_url,
                errorcode = p.video.errorcode,
                youtubeid = p.video.youtubeid,
                price = p.video.price,
                coverurl = p.video.coverurl,
                thumb_preview = p.video.thumb_preview,
                author = new ApplicationUser()
                {
                    Id = p.user.Id,
                    firstname = p.user.firstname,
                    lastname = p.user.lastname,
                    UserName = p.user.UserName,
                    picturename = p.user.picturename
                }
            };

        }

        private static IQueryable<VideoQueryEntity> prepareQuery(ApplicationDbContext context, VideoEntity entity)
        {
            return context.JGN_Videos
                .Join(context.AspNetusers,
                video => video.userid,
                user => user.Id,
                (video, user) => new VideoQueryEntity
                {
                    video = video,
                    user = user
                }).
                Where(returnWhereClause(entity));
        }


        public static IQueryable<VideoQueryEntity> processOptionalConditions(IQueryable<VideoQueryEntity> collectionQuery, VideoEntity query)
        {
            if (query.order != "")
                collectionQuery = (IQueryable<VideoQueryEntity>)collectionQuery.Sort(query.order);

            if (query.id == 0)
            {
                // validation check (if not set, it will return zero records that will make it difficult to debug the code)
                if (query.pagesize == 0)
                    query.pagesize = 18;
                // skip logic
                if (query.pagenumber > 1)
                    collectionQuery = collectionQuery.Skip(query.pagesize * (query.pagenumber - 1));
                // take logic
                if (!query.loadall)
                    collectionQuery = collectionQuery.Take(query.pagesize);
            }
            return collectionQuery;
        }
      

        public static System.Linq.Expressions.Expression<Func<VideoQueryEntity, bool>> returnWhereClause(VideoEntity entity)
        {
            var where_clause = PredicateBuilder.New<VideoQueryEntity>(true);
           
            if (entity.excludedid > 0)
                where_clause = where_clause.And(p => p.video.id != entity.excludedid);

            if (entity.id > 0)
                where_clause = where_clause.And(p => p.video.id == entity.id);

            if (entity.albumid > 0)
                where_clause = where_clause.And(p => p.video.albumid == entity.albumid);

            if (!entity.nofilter)
            {
                if (entity.categoryname != "" || entity.categoryid > 0 || entity.category_ids.Length > 0)
                {
                    where_clause = where_clause.And(p => p.video_category.type == (byte)CategoryContentsBLL.Types.Videos);
                    if (entity.categoryname != null && entity.categoryname != "")
                        where_clause = where_clause.And(x => x.category.title == entity.categoryname || x.category.term == entity.categoryname);

                    if (entity.categoryid > 0)
                        where_clause = where_clause.And(x => x.video_category.categoryid == entity.categoryid);

                    if (entity.category_ids.Length > 0)
                    {
                        foreach (var id in entity.category_ids)
                        {
                            where_clause = where_clause.And(x => x.video_category.categoryid == id);
                        }
                    }
                }

                if (entity.loadfavorites)
                    where_clause = where_clause.And(p => p.favorite.type == (byte)FavoriteBLL.Types.Videos);

                if (entity.loadliked)
                    where_clause = where_clause.And(p => p.rating.type == (byte)UserRatingsBLL.Types.Videos && p.rating.rating == 0);

                if (entity.loadplaylist && entity.playlistid > 0)
                    where_clause = where_clause.And(x => x.playlist.id == entity.playlistid);

                if (entity.loadabusereports)
                    where_clause = where_clause.And(p => p.abusereports.type == (byte)AbuseReport.Types.Videos);

                if (entity.type != MediaType.All)
                    where_clause = where_clause.And(p => p.video.type == (byte)entity.type);

                if (entity.movietype != MovieType.All)
                    where_clause = where_clause.And(p => p.video.movietype == (byte)entity.movietype);

                if (entity.tags != "" && entity.tags != null)
                    where_clause = where_clause.And(p => p.video.tags.Contains(entity.tags.Trim()));

                if (entity.actresses != "" && entity.actresses != null)
                {
                    foreach (var actress in entity.actresses.Split(char.Parse(",")))
                    {
                        if (actress != "")
                        {
                            var filter = actress.Trim();
                            where_clause = where_clause.And(p => p.video.actresses == filter);
                        }
                    }
                }
                if (entity.actors != "" && entity.actors != null)
                {
                    foreach (var actor in entity.actresses.Split(char.Parse(",")))
                    {
                        if (actor != "")
                        {
                            var filter = actor.Trim();
                            where_clause = where_clause.And(p => p.video.actors == filter);
                        }
                    }
                }

                if (entity.price > 0)
                    where_clause = where_clause.And(p => p.video.price > entity.price);

                if (entity.userid != "" && entity.userid != null)
                {
                    if (entity.loadliked)
                        where_clause = where_clause.And(x => x.rating.userid == entity.userid);
                    else if (entity.loadfavorites)
                        where_clause = where_clause.And(x => x.favorite.userid == entity.userid);
                    else
                        where_clause = where_clause.And(p => p.video.userid == entity.userid);
                }
                    

                if (entity.username != "" && entity.username != null)
                    where_clause = where_clause.And(p => p.user.UserName == entity.username);

                if (entity.mode > 0)
                    where_clause = where_clause.And(p => p.video.mode == entity.mode);

                if (entity.isfeatured != FeaturedTypes.All)
                    where_clause = where_clause.And(p => p.video.isfeatured == (byte)entity.isfeatured);

                if (entity.term != "" && entity.term != null)
                    where_clause = where_clause.And(p => p.video.title.Contains(entity.term) || p.video.description.Contains(entity.term) || p.video.tags.Contains(entity.term));

                if (entity.month > 0 && entity.year > 0)
                    where_clause = where_clause.And(p => p.video.created_at.Month == entity.month && p.video.created_at.Year == entity.year);
                else if (entity.year > 0)
                    where_clause = where_clause.And(p => p.video.created_at.Year == entity.year);
                else if (entity.month > 0)
                    where_clause = where_clause.And(p => p.video.created_at.Month == entity.month);

                if (entity.datefilter != DateFilter.AllTime)
                {
                    switch (entity.datefilter)
                    {
                        case DateFilter.Today:
                            where_clause = where_clause.And(p => p.video.created_at >= DateTime.Now.AddDays(-1));
                            break;
                        case DateFilter.ThisWeek:
                            where_clause = where_clause.And(p => p.video.created_at >= DateTime.Now.AddDays(-7));
                            break;
                        case DateFilter.ThisMonth:
                            where_clause = where_clause.And(p => p.video.created_at >= DateTime.Now.AddDays(-31));
                            break;
                        case DateFilter.ThisYear:
                            // this year record
                            where_clause = where_clause.And(p => p.video.created_at >= DateTime.Now.AddYears(-1));
                            break;
                    }
                }

                if (entity.ispublic)
                    where_clause = where_clause.And(p => p.video.ispublished == 1 
                    && p.video.isenabled == 1
                    && p.video.isapproved == 1
                    && p.user.isenabled == 1);
                else
                {
                    // custom settings
                    if (entity.isenabled != EnabledTypes.All)
                        where_clause = where_clause.And(p => p.video.isenabled == (byte)entity.isenabled);

                    if (entity.isapproved != ApprovedTypes.All)
                        where_clause = where_clause.And(p => p.video.isapproved == (byte)entity.isapproved);

                    if (entity.isprivate != PrivacyActionTypes.All)
                        where_clause = where_clause.And(p => p.video.isprivate == (byte)entity.isprivate);

                    if (entity.ispublished != PublishActionType.All)
                        where_clause = where_clause.And(p => p.video.ispublished == (byte)entity.ispublished);
                }

               if (entity.isadult != AdultTypes.All)
                    where_clause = where_clause.And(p => p.video.isadult == (byte)entity.isadult);

                if (entity.isexternal != MediaSourceType.All)
                    where_clause = where_clause.And(p => p.video.isexternal == (byte)entity.isexternal);

                if (entity.iscomments != CommentActionTypes.All)
                    where_clause = where_clause.And(p => p.video.iscomments == (byte)entity.iscomments);

                if (entity.maxid > 0)
                    where_clause = where_clause.And(p => p.video.id > entity.maxid);

                if (entity.minid > 0)
                    where_clause = where_clause.And(p => p.video.id < entity.minid);
            }
            return where_clause;
        }


        #endregion

        #region Report Script
        public static async Task<GoogleChartEntity> LoadReport(ApplicationDbContext context, VideoEntity entity)
        {
            if (entity.reporttype == DefaultReportTypes.Yearly)
                return await VideoReports.YearlyReport(context, entity);
            else if (entity.reporttype == DefaultReportTypes.CurrentMonth)
                return await VideoReports.CurrentMonthReport(context, entity);
            else
                return await VideoReports.Last12MonthsReport(context, entity);
        }
        #endregion

        public static List<ArchiveEntity> Load_Arch_List(ApplicationDbContext context,int type, int records, bool isall)
        {
            // cache implementation
            int cache_duration = Jugnoon.Settings.Configs.GeneralSettings.cache_duration;
            if (cache_duration == 0) // no cache
                return Fetch_Archive_List(context,type, records, isall);
            else
            {
                string key = "ld_vd_arc_lst_" + type + "" + records + "" + isall;
                var data = new List<ArchiveEntity>();
                if (!SiteConfig.Cache.TryGetValue(key, out data))
                {
                    data = Fetch_Archive_List(context,type, records, isall);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                    // Save data in cache.
                    SiteConfig.Cache.Set(key, data, cacheEntryOptions);
                }
                else
                {
                    data = (List<ArchiveEntity>)SiteConfig.Cache.Get(key);
                }

                return data;
            }

        }
        public static List<ArchiveEntity> Fetch_Archive_List(ApplicationDbContext context,int type, int records, bool isall)
        {
            // var context = SiteConfig.dbContext;
            var model = context.JGN_Videos
                .GroupBy(o => new
                {
                    Month = o.created_at.Month,
                    Year = o.created_at.Year
                })
                .Select(g => new ArchiveEntity
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Count()
                })
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ToList();

            return model;
        }

        #region Youtube SDK
       
        public static bool Check_YOUTUBE_ID(ApplicationDbContext context, string youtubeid)
        {
            bool flag = false;
            if (context.JGN_Videos.Where(p => p.youtubeid == youtubeid).Count() > 0)
                flag = true;

            return flag;
        }


        #endregion
         
        public static async Task<bool> Refresh_User_Stats(ApplicationDbContext context, string UserName, int type)
        {

            string field = "stat_videos";
            if (type == 1)
                field = "stat_audio";
            var count = await CountRecords(context, new VideoEntity()
            {
                username = UserName,
                ispublic = true
            });
            await UserStatsBLL.Update_Field(context, UserName, count, field);
            
            return true;
        }

        public static async Task<string> ProcessAction(ApplicationDbContext context,List<VideoEntity> list)
        {
            foreach (var entity in list)
            {
                if (entity.id > 0)
                {
                    switch (entity.actionstatus)
                    {
                        case "enable":
                            Update_Field_V3(context, entity.id, (byte)1, "isenabled");
                            break;

                        case "disable":
                            Update_Field_V3(context, entity.id, (byte)0, "isenabled");
                            break;
                        case "approve":
                            Update_Field_V3(context, entity.id, (byte)1, "isapproved");
                            break;
                        case "featured":
                            Update_Field_V3(context, entity.id, (byte)1, "isfeatured");
                            break;
                        case "premium":
                            Update_Field_V3(context, entity.id, (byte)2, "isfeatured");
                            break;
                        case "normal":
                            Update_Field_V3(context, entity.id, (byte)0, "isfeatured");
                            break;
                        case "adult":
                            Update_Field_V3(context, entity.id, (byte)2, "isadult");
                            break;
                        case "nonadult":
                            Update_Field_V3(context, entity.id, (byte)0, "isadult");
                            break;
                        case "private":
                            Update_Field_V3(context, entity.id, (byte)1, "isprivate");
                            break;
                        case "public":
                            Update_Field_V3(context, entity.id, (byte)0, "isprivate");
                            break;
                        case "movie":
                            Update_Field_V3(context, entity.id, (byte)2, "type");
                            break;
                        case "clip":
                            Update_Field_V3(context, entity.id, (byte)0, "type");
                            break;
                        case "delete":
                            await RemoveVideo(context, entity.id, (int)entity.type);
                            break;
                        case "delete_fav":
                            await FavoriteBLL.Delete(context, entity.id, entity.userid, (byte)entity.type, (byte)FavoriteBLL.Types.Videos);
                            break;
                        case "delete_like":
                            await UserRatingsBLL.Delete(context, entity.id, entity.userid, (byte)UserRatingsBLL.Types.Videos);
                            break;
                        case "delete_playlist":
                            await PlayListBLL.Delete(context, entity.id);
                            break;
                    }
                }
            }
            return "OK";
        }
    }

    /// <summary>
    /// Entity used while joining data of tables (videos, users, category, favorite, rating, playlist based on conditions) via Entity Framework (Linq)
    /// </summary>
    public class VideoQueryEntity
    {
        public JGN_Videos video { get; set; }
        public ApplicationUser user { get; set; }
        public JGN_Categories category { get; set; }
        public JGN_CategoryContents video_category { get; set; }
        public JGN_Favorites favorite { get; set; }
        public JGN_User_Ratings rating { get; set; }
        public JGN_Playlist_Videos playlist { get; set; }
        public JGN_AbuseReports abusereports { get; set; }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
