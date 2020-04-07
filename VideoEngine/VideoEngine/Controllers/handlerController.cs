using System;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Jugnoon.BLL;
using Jugnoon.Utility;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Jugnoon.Settings;
using Jugnoon.Videos;
using System.Threading.Tasks;
using Jugnoon.Localize;

namespace VideoEngine.Controllers
{
    public class handlerController : Controller
    {
        ApplicationDbContext _context;
        public handlerController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
           IStringLocalizer<GeneralResource> generalLocalizer,
           IStringLocalizer<VideoResource> videoLocalizer,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           IOptions<General> generalSettings,
           IOptions<Media> mediaSettings,
           IOptions<Social> socialSettings,
           IOptions<Features> featureSettings,
           IOptions<Smtp> smtpSettings,
           IOptions<Registration> registerSettings
           )
        {
            _context = context;
            // readable configuration
            Jugnoon.Settings.Configs.GeneralSettings = generalSettings.Value;
            Jugnoon.Settings.Configs.SocialSettings = socialSettings.Value;
            Jugnoon.Settings.Configs.FeatureSettings = featureSettings.Value;
            Jugnoon.Settings.Configs.SmtpSettings = smtpSettings.Value;
            Jugnoon.Settings.Configs.MediaSettings = mediaSettings.Value;
            Jugnoon.Settings.Configs.RegistrationSettings = registerSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.videoLocalizer = videoLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }
        // GET: handler
        public IActionResult Index()
        {
            return View();
        }

        #region Flag Reporting
        public IActionResult flag()
        {
            var Type = 0; // 0: video, audio, 1: comment adv points, 2: photo rating , 3: blog rating
            long ContentID = 0;
            var username = "";
            var Message = "";
            var Mediatype = 0;
            var ClientID = ""; //element id where you wwant to display output
            var Contenttype = "";

            var _ctype = "text/plain";

            if (HttpContext.Request.Query["t"].Count > 0)
                Type = Convert.ToInt32(HttpContext.Request.Query["t"]);
            if (HttpContext.Request.Query["id"].Count > 0)
                ContentID = Convert.ToInt64(HttpContext.Request.Query["id"]);
            if (HttpContext.Request.Query["usr"].Count > 0)
                username = HttpContext.Request.Query["usr"].ToString();
            if (HttpContext.Request.Query["id"].Count > 0)
                ClientID = HttpContext.Request.Query["id"].ToString();
            if (HttpContext.Request.Query["mtp"].Count > 0)
                Mediatype = Convert.ToInt32(HttpContext.Request.Query["mtp"]);

            if (HttpContext.Request.Query["ctp"].Count > 0)
                Contenttype = HttpContext.Request.Query["ctp"].ToString();

            Message = Flag_Generate_Output(Contenttype, Type, ContentID, username);

            return this.Content(Message, _ctype);
        }

        private string Flag_Generate_Output(string ContentType, int Type, long ContentID, string UserName)
        {
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            var str = new StringBuilder();

            string content_type = SiteConfig.generalLocalizer["_video"].Value;
            if (ContentType != "")
                content_type = ContentType;
            else
            {
                // backward compatible
                switch (Type)
                {
                    case 2:
                        content_type = SiteConfig.generalLocalizer["_photo"].Value;
                        break;
                    case 3:
                        content_type = "blog post";
                        break;
                }
            }
            // show message
            string flag_submit_path = Config.GetUrl("handler/postabuse");
            string flag_submit_parameters = "t=" + Type + "&usr=" + UserName + "&ctp=answer&id=" + ContentID;
            
            str.AppendLine("<div class=\"m-b-5 text-center mb5_c\" id=\"flg_msg\"></div>");
            str.AppendLine("<p class=\"m-b-10\"><strong>Report This " + content_type + " as Inappropriate</strong></p>");
            str.AppendLine("<p class=\"m-b-5\">Please select the category that most closely reflects your concern about the " + content_type + ", so that we can review it and determine whether it violates our Community Guidelines or isn't appropriate for all viewers. Abusing this feature is also a violation of the Community Guidelines, so don't do it.</p>");
            str.AppendLine("<div class=\"m-b-5\">");
            str.AppendLine(Flag_Generate_Options());
            str.AppendLine("</div>");
            str.AppendLine("<div class=\"m-b-5\">");
            str.AppendLine("&nbsp;<a href=\"#\" class=\"btn btn-primary flag_sbt\" data-destination=\"action_box_" + ContentID + "\" data-path=\"" + flag_submit_path + "\" data-param=\"" + flag_submit_parameters + "\">Flag This " + content_type + "</a>");
            str.AppendLine("</div>");
            
            return Config.SetHiddenMessage_v2(str.ToString(), "", true, 4);
        }

        private string Flag_Generate_Options()
        {
            var str = new StringBuilder();
            var _array = new ArrayList();
            int type = 4; // load abuse types

            var List = CategoryBLL.LoadItems(_context, new Jugnoon.Entity.CategoryEntity()
            {
                type = type,
                pagesize = 1000,
                ispublic = true
            }).Result;

            str.AppendLine("<select class=\"form-control\" name=\"flag_list\" id=\"abuse_list\">");
            str.AppendLine("<option value=\"\">" + SiteConfig.generalLocalizer["_select_reason"].Value + "</option>");
            foreach (var Item in List)
            {
                str.AppendLine("<option value=\"" + Item.title+ "\">" + Item.title+ "</option>");
            }
            str.AppendLine("</select>");
            return str.ToString();
        }

        public async Task<IActionResult> postabuse()
        {
            int Type = 0; // 0: video, audio, 1: comment adv points, 2: photo rating , 3: blog rating
            long ContentID = 0;
            string username = "";
            string Message = "";
            string Value = "";
            int MediaType = 0; // 0: video, 1: audio file
            string ElementID = ""; // div element id where you want to load box with close link

            var _ctype = "text/plain";

            if (HttpContext.Request.Query["t"].Count > 0)
                Type = Convert.ToInt32(HttpContext.Request.Query["t"]);
            if (HttpContext.Request.Query["id"].Count > 0)
                ContentID = Convert.ToInt64(HttpContext.Request.Query["id"]);
            if (HttpContext.Request.Query["usr"].Count > 0)
                username = HttpContext.Request.Query["usr"].ToString();
            if (HttpContext.Request.Query["val"].Count > 0)
                Value = HttpContext.Request.Query["val"].ToString();
            if (HttpContext.Request.Query["mtp"].Count > 0)
                MediaType = Convert.ToInt32(HttpContext.Request.Query["mtp"]);
            if (HttpContext.Request.Query["elid"].Count > 0)
                ElementID = HttpContext.Request.Query["elid"].ToString();


            if (ContentID > 0 && username != "")
            {
                // data received properly
                // make type compatible to abuse types
                if (Type == 2)
                    Type = 5; // photos
                if (Type == 3)
                    Type = 7; // blog post

                // post abuse report
                string output_message = await Process_V3(username,ContentID,Value,Type,MediaType);
                Message = Config.SetHiddenMessage_v2(output_message, ElementID, true, 0);
            }

            return this.Content(Message, _ctype);
        }

        private async Task<string> Process_V3(string content_username, long contentid, string reason, int type, int mediatype)
        {
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                string sign_in = "<a href=\"" + Config.GetUrl() + "Login.aspx\" class=\"bold\">Sign In</a>";
                string sign_up = "<a href=\"" + Config.GetUrl() + "Register.aspx\" class=\"bold\">Sign Up</a>";
                return sign_in + " or " + sign_up + " to post report!";
            }
            var info = SiteConfig.userManager.GetUserAsync(User).Result;
            var userName = info.UserName;

            if (content_username == userName)
            {
                return SiteConfig.generalLocalizer["_abuse_msg_01"].Value; // You can't post abuse / spam report on your own content.
            }

            if (await AbuseReport.Check_UserName(_context, userName, contentid, type))
            {
                return SiteConfig.generalLocalizer["_abuse_msg_02"].Value; // "You already post abuse / spam report on this content.";
            }

            //***********************************
            // ENABLE comment if you want to validate report based on ip address
            //***********************************
            string ipaddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            if (await AbuseReport.Check_IPAddress(_context, ipaddress, contentid, type))
            {
                return SiteConfig.generalLocalizer["_abuse_msg_03"].Value; //	Report already posted from this IP address.	
            }

            await AbuseReport.Add(_context, contentid, userName, ipaddress, reason, type);

            int count_reports = await AbuseReport.Count(_context, contentid, type);
            if (count_reports > Jugnoon.Settings.Configs.GeneralSettings.spam_count)
            {
                int OldValue = 0;
                switch (type)
                {
                    case 0:
                        // disable video
                        VideoBLL.Update_Field_V3(_context, contentid, (byte)0, "isenabled");
                        OldValue = Convert.ToInt32(VideoBLL.Get_Field_Value(_context, contentid, "isenabled"));
                        break;
                }

            }
            return SiteConfig.generalLocalizer["_abuse_msg_04"].Value;
        }


        #endregion

        public IActionResult embedvideo()
        {
           long VideoID = 0;
           string ElementID = ""; // div element id where you want to load box with close linkou want to load box with close link

            var _ctype = "text/plain";
            if (HttpContext.Request.Query["cid"].Count > 0)
                VideoID = Convert.ToInt64(HttpContext.Request.Query["cid"]);
            if (HttpContext.Request.Query["elid"].Count > 0)
                ElementID = HttpContext.Request.Query["elid"].ToString();

            return this.Content(EmbedVideo_Generate_Output(VideoID, ElementID), _ctype);
        }

        private string EmbedVideo_Generate_Output(long VideoID, string ElementID)
        {
            var str = new StringBuilder();
            str.AppendLine("<div class=\"mb5\">");
            str.AppendLine("<textarea name=\"vd_bx_embed\" rows=\"3\" cols=\"40\" id=\"vd_bx_embed\" onclick=\"this.focus();this.select();\" style=\"height:70px;width:100%;\">");
            str.AppendLine("<iframe src=\"" + Config.GetUrl("videoframe/" + VideoID + "/video") + "\" name=\"frame_v" + VideoID + "\" scrolling=\"no\" frameborder=\"no\" align=\"center\" height=\"380px\" width=\"640px\"></iframe>");
            str.AppendLine("</textarea>");
            str.AppendLine("</div>");
            str.AppendLine("<div class=\"item\">" + SiteConfig.videoLocalizer["_embed_msg_01"].Value + "</div>");
            str.AppendLine("</div>");
            return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 4); 
        }

        public IActionResult playlist()
        {
            long ContentID = 0;
            string username = ""; // User who added media in his / her favorite lis
            string Author_username = ""; // User who are actually owner of media
            string Message = "";
            int _Favorites = 0;
            int MediaType = 0; // 0: video, 1: audio
            string ElementID = ""; // div element id where you want to load box with close link
            
            var _ctype = "text/plain";
            if (HttpContext.Request.Query["id"].Count > 0)
                ContentID = Convert.ToInt64(HttpContext.Request.Query["id"]);
            if (HttpContext.Request.Query["usr"].Count > 0)
                username = HttpContext.Request.Query["usr"].ToString();
            if (HttpContext.Request.Query["ausr"].Count > 0)
                Author_username = HttpContext.Request.Query["ausr"].ToString();
            if (HttpContext.Request.Query["mtp"].Count > 0)
                MediaType = Convert.ToInt32(HttpContext.Request.Query["mtp"]);
           
            if (HttpContext.Request.Query["elid"].Count > 0)
                ElementID = HttpContext.Request.Query["elid"].ToString();
            

            Message = Playlist_Generate_Output(MediaType, username, ElementID, Author_username, ContentID, _Favorites);

            return this.Content(Message, _ctype);
        }

        private string Playlist_Generate_Output(int MediaType, string UserName, string ElementID, string Author_UserName, long ContentID, int _Favorites)
        {
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            var str = new StringBuilder();
            string media = SiteConfig.generalLocalizer["_video"].Value;
            string _url = Config.GetUrl();
            string fav_url = _url + "account/videos/playlists";
            
            // check authentication
            if (UserName == "")
            {
                string sign_in = "<a href=\"" + Config.GetUrl() + "signin\" class=\"bold\">" + SiteConfig.generalLocalizer["_sign_in"].Value + "</a>";
                string sign_up = "<a href=\"" + Config.GetUrl() + "signup\" class=\"bold\">" + SiteConfig.generalLocalizer["_sign_up"].Value + "</a>";
                str.AppendLine("" + sign_in + " or " + sign_up + " now!");
                return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 2);
            }

            // check your own video
            if (Author_UserName == UserName)
            {
                str.AppendLine("Can't add your own " + media + " to your <a href=\"" + fav_url + "\">" + SiteConfig.videoLocalizer["_playlists"].Value + "</a>.");
                return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 0);
            }

            // playlist feature currently under development
            str.AppendLine("Playlist Currently Disabled");

            // wrap output
            return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 1);

        }

        public async Task<IActionResult> favorites()
        {
            int Favtype = 0; // 0: video, audio, 1: qa fav, 2: gallery or album
            long ContentID = 0;
            string username = ""; // User who added media in his / her favorite lis
            string Author_username = ""; // User who are actually owner of media
            string Message = "";
            int _Favorites = 0;
            int MediaType = 0; // 0: video, 1: audio
            string ElementID = ""; // div element id where you want to load box with close link


            var _ctype = "text/plain";
            if (HttpContext.Request.Query["id"].Count > 0)
                ContentID = Convert.ToInt64(HttpContext.Request.Query["id"]);
            if (HttpContext.Request.Query["usr"].Count > 0)
                username = HttpContext.Request.Query["usr"].ToString();
            if (HttpContext.Request.Query["ausr"].Count > 0)
                Author_username = HttpContext.Request.Query["ausr"].ToString();
            if (HttpContext.Request.Query["mtp"].Count > 0)
                MediaType = Convert.ToInt32(HttpContext.Request.Query["mtp"]);
            if (HttpContext.Request.Query["favt"].Count > 0)
                _Favorites = Convert.ToInt32(HttpContext.Request.Query["favt"]);
            if (HttpContext.Request.Query["elid"].Count > 0)
                ElementID = HttpContext.Request.Query["elid"].ToString();

            if (HttpContext.Request.Query["ftype"].Count > 0)
                Favtype = Convert.ToInt32(HttpContext.Request.Query["ftype"]);


            Message = await Favorites_Generate_Output(Favtype, MediaType, username, ElementID, Author_username, ContentID, _Favorites);

            return this.Content(Message, _ctype);
        }

        private async Task<string> Favorites_Generate_Output(int FavType, int MediaType, string UserName, string ElementID, string Author_UserName, long ContentID, int _Favorites)
        {
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            var str = new StringBuilder();
            string media = SiteConfig.generalLocalizer["_video"].Value;
            string _url = Config.GetUrl();
            string fav_url = _url + "account/my-videos/favorites";
            if (FavType == 2)
            {
                media = "album";
                if (MediaType == 1)
                    fav_url = _url + "account/my-audio/favorites";
                else
                    fav_url = _url + "account/my-alubms/favorites";
            }
            else if (MediaType == 1)
            {
                media = SiteConfig.generalLocalizer["_audio"].ToString();
                fav_url = _url + "account/my-audio/favorites";
            }

            // check authentication
            if (UserName== "")
            {
                string sign_in = "<a href=\"" + Config.GetUrl() + "signin\" class=\"bold\">" + SiteConfig.generalLocalizer["_sign_in"].Value + "</a>";
                string sign_up = "<a href=\"" + Config.GetUrl() + "signup\" class=\"bold\">" + SiteConfig.generalLocalizer["_sign_up"].Value + "</a>";
                str.AppendLine("" + sign_in + " or " + sign_up + " now!");
                return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 2);
            }

            // check your own video
            if (Author_UserName== UserName)
            {
                str.AppendLine("Can't add your own " + media + " to your <a href=\"" + fav_url + "\">" + SiteConfig.generalLocalizer["_favorites"] + "</a>.");
                return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 0);
            }

            if (await FavoriteBLL.Check(_context, UserName, ContentID, FavType))
            {
                str.AppendLine("You already add this " + media + " to your <a href=\"" + fav_url + "\">" + SiteConfig.generalLocalizer["_favorites"].Value + "</a>.");
                return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 0);
            }
            
            await FavoriteBLL.Add(_context, UserName, ContentID, MediaType, FavType);

            // increment favorite video state
            _Favorites++;
            VideoBLL.Update_Field_V3(_context, ContentID, _Favorites, "favorites");

            str.AppendLine(media.ToUpper() + " has been <strong>added</strong> to your <a href=\"" + fav_url + "\">" + SiteConfig.generalLocalizer["_favorites"].Value + "</a>.");
            // wrap output
            return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 4);

        }

        public IActionResult share()
        {
            string Content_type = "";

            var _ctype = "text/plain";

            if (HttpContext.Request.Query["tp"].Count > 0)
                Content_type = HttpContext.Request.Query["tp"].ToString();

            return this.Content(Share_Generate_Output(Content_type), _ctype);
        }

        private string Share_Generate_Output(string Content_Type)
        {
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            var str = new StringBuilder();
            str.AppendLine("<div class=\"mb10 mr10 ml10 mt10\">");
            str.AppendLine("<div class=\"mb5\"><strong>Share This " + Content_Type + "</strong></div>");
            str.AppendLine("<div class=\"row\">");
            str.AppendLine("<div class=\"col-md-12\">");
            str.AppendLine("<!-- AddThis Button BEGIN -->");
            str.AppendLine("<div class=\"addthis_toolbox addthis_default_style addthis_32x32_style\">");
            str.AppendLine("<a class=\"addthis_button_facebook\"></a>");
            str.AppendLine("<a class=\"addthis_button_twitter\"></a>");
            str.AppendLine("<a class=\"addthis_button_reddit\"></a>");
            str.AppendLine("<a class=\"addthis_button_digg\"></a>");
            str.AppendLine("<a class=\"addthis_button_delicious\"></a>");
            str.AppendLine("<a class=\"addthis_button_stumbleupon\"></a>");
            str.AppendLine("<a class=\"addthis_button_google\"></a>");
            str.AppendLine("<a class=\"addthis_button_compact\"></a>");
            str.AppendLine("<a class=\"addthis_counter addthis_bubble_style\"></a>");
            str.AppendLine("</a>");
            str.AppendLine("</div>");

            // integrate third party sharing tootls
            // share this
            if (Jugnoon.Settings.Configs.SocialSettings.sharethis_propertyId != "")
            {
                // js file associated with this attached in layout.cshtml 
                str.AppendLine("<div class=\"sharethis-inline-share-buttons\"></div>");
            }
            else if (Jugnoon.Settings.Configs.SocialSettings.addthis_pubid != "")
            {
                str.AppendLine("<script type=\"text/javascript\">        var addthis_config = { \"data_track_clickback\": true };</script>");
                str.AppendLine("<script type=\"text/javascript\" src=\"http://s7.addthis.com/js/250/addthis_widget.js#pubid=" + Jugnoon.Settings.Configs.SocialSettings.addthis_pubid + "\"></script>");
                str.AppendLine("<!-- AddThis Button END -->");
            }
           
            str.AppendLine("</div>");
           
            str.AppendLine("</div>");

            str.AppendLine("</div>");
            return Config.SetHiddenMessage_v2(str.ToString(), "actionshare", true, 4);
        }

        public async Task<IActionResult> like()
        {
            int type = 0; // 0: video, audio, 1: comment adv points, 2: photo rating , 3: blog rating
            int MediaType = 0; // 0: video, 1: audio
            long ContentID = 0;
            string username = "";
            int Liked = 0;
            int Disliked = 0;
            int Action = 0; // 0: Liked, 1: Disliked
            string Message = "";
            string ElementID = ""; // div id where to load content
            int Ratingtype = 0; // 0: like / dislike, 1: start rating
            double Avg_Ratings = 0;
            double Ratings = 0;
            int TotalRatings = 0;
            int Val = 0;
            var _ctype = "text/plain";
            /*if (Request.UrlReferrer == null)
            {
                Response.Write("p400"); // Authorization Failed"
            }
            else
            { */
            if (HttpContext.Request.Query["t"].Count > 0)
                    type = Convert.ToInt32(HttpContext.Request.Query["t"]);
                if (HttpContext.Request.Query["mtp"].Count > 0)
                    MediaType = Convert.ToInt32(HttpContext.Request.Query["mtp"]);
                if (HttpContext.Request.Query["id"].Count > 0)
                    ContentID = Convert.ToInt64(HttpContext.Request.Query["id"]);
                if (HttpContext.Request.Query["usr"].Count > 0)
                    username = HttpContext.Request.Query["usr"].ToString();
                if (HttpContext.Request.Query["lk"].Count > 0)
                    Liked = Convert.ToInt32(HttpContext.Request.Query["lk"]);
                if (HttpContext.Request.Query["dlk"].Count > 0)
                    Disliked = Convert.ToInt32(HttpContext.Request.Query["dlk"]);
                if (HttpContext.Request.Query["act"].Count > 0)
                    Action = Convert.ToInt32(HttpContext.Request.Query["act"]);
                if (HttpContext.Request.Query["elid"].Count > 0)
                    ElementID = HttpContext.Request.Query["elid"].ToString();
                if (HttpContext.Request.Query["rtype"].Count > 0)
                    Ratingtype = Convert.ToInt32(HttpContext.Request.Query["rtype"]);
                if (HttpContext.Request.Query["ratings"].Count > 0)
                    Ratings = Convert.ToDouble(HttpContext.Request.Query["ratings"]);
                if (HttpContext.Request.Query["aratings"].Count > 0)
                    Avg_Ratings = Convert.ToDouble(HttpContext.Request.Query["aratings"]);
                if (HttpContext.Request.Query["tratings"].Count > 0)
                    TotalRatings = Convert.ToInt32(HttpContext.Request.Query["tratings"]);
                if (HttpContext.Request.Query["val"].Count > 0)
                    Val = Convert.ToInt32(HttpContext.Request.Query["val"]);

                // if user already logged in
                if (username != "")
                {
                    // check whether he / she already post rating on same content
                    if (!await UserRatingsBLL.Check(_context, username, ContentID, type))
                    {
                        // proceed user to post vote
                        if (Ratingtype == 0)
                        {
                            // like / dislike
                            Post_Vote(Action, Liked, Disliked, type, ContentID);
                        }
                        else
                        {
                            // normal ratings
                            Post_Rating(Action, Avg_Ratings, Liked, Disliked, type, ContentID, Ratings, TotalRatings, Val);
                        }

                        // add user statistics
                        await UserRatingsBLL.Add(_context, username, ContentID, type, Val); //rating 0: liked, 1: disliked
                        if (Ratingtype == 0)
                            Message = Set_Liked_Message(Action, type, MediaType, Liked, Disliked, username, ElementID);
                        else
                            Message = Set_Rating_Message(type, MediaType, ElementID, Liked, Disliked, username, Avg_Ratings, TotalRatings);
                    }
                    else
                    {
                        // he already post their vote
                        if (Ratingtype == 0)
                            Message = Set_Liked_Message(Action, type, MediaType, Liked, Disliked, username, ElementID);
                        else
                            Message = Set_Rating_Message(type, MediaType, ElementID, Liked, Disliked, username, Avg_Ratings, TotalRatings);
                    }
                }
                /*else
                {
                    if (Site_Settings.Feature_Login_Rating == 0)
                    {
                        // directly post vote without storing user statistics
                        if (Ratingtype == 0)
                        {
                            Post_Vote(Action, Liked, Disliked, type, ContentID);
                            Message = Set_Liked_Message(Action, type, MediaType, Liked, Disliked, username, ElementID);
                        }
                        else
                        {
                            Post_Rating(Action, Avg_Ratings, Liked, Disliked, type, ContentID, Ratings, TotalRatings, Val);
                            Message = Set_Rating_Message(type, MediaType, ElementID, Liked, Disliked, username, Avg_Ratings, TotalRatings);
                        }
                    }
                }*/

               
            //}
            return this.Content(Message, _ctype);
        }

        // like | dislike
        private void Post_Vote(int Action, int Liked, int Disliked, int Type, long ContentID)
        {
            if (Action == 0)
            {
                Liked = Liked + 1;
                switch (Type)
                {
                    case 0:
                        VideoBLL.Update_Field_V3(_context, ContentID, Liked, "liked");
                        break;
                }
                Liked = Liked + 1;
            }
            else
            {
                Disliked = Disliked + 1;
                switch (Type)
                {
                    case 0:
                        VideoBLL.Update_Field_V3(_context, ContentID, Disliked, "disliked");
                        break;
                }
                Disliked = Disliked + 1;
            }
        }

        // normal ratings
        private void Post_Rating(int Action, double Avg_Ratings, int Liked, int Disliked, int Type, long ContentID, double Ratings, int TotalRatings, int Val)
        {
            if (Action > 5)
                Action = 5;

            switch (Type)
            {
                case 0:
                    Avg_Ratings = VideoBLL.Post_Rating(_context, ContentID, Val, TotalRatings, Ratings);
                    break;
                case 3:
                    // currently not supported in blogs
                    break;
            }
            TotalRatings = TotalRatings + 1;
            Ratings = Ratings + 1;
        }

        // set liked | disliked Message
        private string Set_Liked_Message(int Action, int Type, int MediaType, int Liked, int Disliked, string UserName, string ElementID)
        {
            string action = "liked";
            if (Action != 0)
                action = "disliked";

            string content_type = "";
            string content_url = "";
            switch (Type)
            {
                case 0:
                    // video | audio
                    if (MediaType == 1)
                    {
                        // audio
                        content_url = "<a href=\"" + Config.GetUrl() + "account/my-audio/liked\">audio files you " + action + "</a>";
                        content_type = "audio file";
                    }
                    else
                    {
                        // video
                        content_url = "<a href=\"" + Config.GetUrl() + "account/my-videos/liked\">videos you " + action + "</a>";
                        content_type = SiteConfig.generalLocalizer["_video"].Value;
                    }
                    break;
                case 2:
                    content_url = "<a href=\"" + Config.GetUrl() + "account/my-photos/liked\">photos you " + action + "</a>";
                    content_type = SiteConfig.generalLocalizer["_photo"].Value;
                    break;
                case 3:
                    content_url = "<a href=\"" + Config.GetUrl() + "account/my-blogs/liked\">blogs you " + action + "</a>";
                    content_type = "blog post";
                    break;
                case 9:
                    // no gallery feature available - redirect user to photos
                    content_url = "<a href=\"" + Config.GetUrl() + "account/my-photos/liked\">photo galleries you " + action + "</a>";
                    content_type = "gallery";
                    break;

            }
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            int total_votes = Liked + Disliked;
            var str = new StringBuilder();
            str.AppendLine("<div class=\"mb10\">You " + action + " this " + content_type + "." + SiteConfig.generalLocalizer["_thanks_for_the_feedback"].Value);
            if (UserName != "") // if user logged in 
                str.AppendLine(" See more " + content_url + ".");
            str.AppendLine("</div>");
            str.AppendLine("<div class=\"mb10\"><strong>Rating for this " + content_type + " </strong><span class=\"normal-text reverse\">(" + total_votes + " total)</span>");
            str.AppendLine("\n");
            // internal rating section
            str.AppendLine("<div class=\"mb5\" style=\"width:300px;\">");
            str.AppendLine(Generate_Rating_Bx(Liked, Disliked));
            str.AppendLine("</div>");
            str.AppendLine("</div>");

            return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 4);

        }

        // Generate Rating Message
        private string Set_Rating_Message(int Type, int MediaType, string ElementID, int Liked, int Disliked, string UserName, double Avg_Ratings, int TotalRatings)
        {
            string action = "rated";
            string content_type = "";
            string content_url = "";
            switch (Type)
            {
                case 0:
                    // video | audio
                    if (MediaType == 1)
                    {
                        // audio
                        content_url = "<a href=\"" + Config.GetUrl() + "account/audio/liked\">audio files you " + action + "</a>";
                        content_type = "audio file";
                    }
                    else
                    {
                        // video
                        content_url = "<a href=\"" + Config.GetUrl() + "account/videos/liked\">videos you " + action + "</a>";
                        content_type = SiteConfig.generalLocalizer["_video"].Value;
                    }
                    break;
                case 2:
                    content_url = "<a href=\"" + Config.GetUrl() + "account/photos/liked\">photos you " + action + "</a>";
                    content_type = SiteConfig.generalLocalizer["_photo"].Value;
                    break;
                case 3:
                    content_url = "<a href=\"" + Config.GetUrl() + "account/blogs/liked\">blogs you " + action + "</a>";
                    content_type = "blog post";
                    break;
                case 9:
                    // currently not supported
                    // no gallery feature available - redirect user to photos
                    content_url = "<a href=\"" + Config.GetUrl() + "account/photos/liked\">photo galleries you " + action + "</a>";
                    content_type = "gallery";
                    break;

            }
            //**********************************************************
            // Generate Output Panel
            //**********************************************************
            int total_votes = Liked + Disliked;
            var str = new StringBuilder();
            str.AppendLine("<div class=\"card\"><div class=\"card-body\"");
            str.AppendLine("<div class=\"m-b-10\">You " + action + " this " + content_type + ". " + SiteConfig.generalLocalizer["_thanks_for_rating"].Value);
            if (UserName != "") // if user logged in
                str.AppendLine(" See more " + content_url + ".");
            str.AppendLine("</div>");
            str.AppendLine("<div class=\"m-b-10\"><strong>Rating for this " + content_type + " </strong><span class=\"normal-text reverse\">(" + TotalRatings + " total, " + Avg_Ratings + " out of 5)</span>");
            str.AppendLine("\n");
            // internal rating section
            str.AppendLine("<div class=\"m-b-5\">");
            str.AppendLine("<ul class='star-rating'>");
            str.AppendLine("<li class='current-rating' style=\"width:" + Calculate_Ratings(Avg_Ratings) + "px;\">Currently " + Math.Round(Avg_Ratings, 2) + "/5 Stars.</li>");
            str.AppendLine("<li><a href='#' title='1 star out of 5' class='one-star'>1</a></li>");
            str.AppendLine("<li><a href='#' title='2 stars out of 5' class='two-stars'>2</a></li>");
            str.AppendLine("<li><a href='#' title='3 stars out of 5' class='three-stars'>3</a></li>");
            str.AppendLine("<li><a href='#' title='4 stars out of 5' class='four-stars'>4</a></li>");
            str.AppendLine("<li><a href='#' title='5 stars out of 5' class='five-stars'>5</a></li>");
            str.AppendLine("</ul>");
            str.AppendLine("</div>");
            str.AppendLine("</div>");
            str.AppendLine("</div></div>");
            return Config.SetHiddenMessage_v2(str.ToString(), ElementID, true, 4);

        }

        private string Generate_Rating_Bx(int Liked, int Disliked)
        {
            var str = new StringBuilder();
            int total_liked = Liked + Disliked;
            double liked_percentage = (double)(Liked * 100) / total_liked;
            double disliked_percentage = (double)(Disliked * 100) / total_liked;
            if (liked_percentage > 0 || disliked_percentage > 0)
            {
                if (liked_percentage > 98)
                {
                    str.AppendLine("<div id=\"like_rating\" style=\"width: 100%;\" class=\"bx_like\"></div>");
                }
                else if (disliked_percentage > 98)
                {
                    str.AppendLine("<div id=\"dislike_rating\" style=\"width: 100%;\" class=\"bx_dislike\"></div>");
                }
                else
                {
                    disliked_percentage = 100 - liked_percentage; // 7 for creating gap between two meters.
                                                                  // liked meter
                    str.AppendLine("<div class=\"mb5\">");
                    str.AppendLine("<div id=\"like_rating\" style=\"width: " + Convert.ToInt32(liked_percentage).ToString() + "%;\" class=\"bx_like\"></div>");
                    str.AppendLine("</div>");
                    str.AppendLine("<div class=\"mb5\">");
                    str.AppendLine("<div id=\"dislike_rating\" style=\"width: " + Convert.ToInt32(disliked_percentage).ToString() + "%;\" class=\"bx_dislike\"></div>");
                    str.AppendLine("</div>");
                    str.AppendLine("\n");

                }
            }
            else
            {
                // no liked or disliked yet
                // show full rating
                str.AppendLine("<div id=\"like_rating\" style=\"width: 100%;\" class=\"bx_like\"></div>");
            }

            return str.ToString();
        }

        private int Calculate_Ratings(double Avg_Ratings)
        {
            return (int)Avg_Ratings * 24; // 24x24 each rating size
        }

        /* resize remote images */
        public void resize()
        {
            /*string FileName = "";
            int width = 800;
            int height = 600;


            if (HttpContext.Request.Query["file"].Count > 0)
            {
                FileName = WebUtility.UrlDecode(HttpContext.Request.Query["file"].ToString());
            }

            if (HttpContext.Request.Query["size"].Count > 0)
            {
                string[] arr = HttpContext.Request.Query["size"].ToString().Split(char.Parse("x"));
                if (arr.Length > 0)
                {
                    width = Convert.ToInt32(arr[0].Trim());
                    height = Convert.ToInt32(arr[1].Trim());
                }
            }

            var ht = new Hashtable();
            Byte[] arrImg = null;
            var ImageID = Guid.NewGuid().ToString().Substring(0, 6);
            // get original photo url
            string _fileName = FileName.Replace("thumbs/", "");
            // download and save image in memory stream
            var client = new WebClient();
            byte[] imageBytes = client.DownloadData(_fileName);
            byte[] bFile;
            bFile = Jugnoon.Utility.Image.Generate_Online_Thumbnail(new MemoryStream(imageBytes), width, height);
            ht.Add(ImageID, bFile);

            arrImg = (byte[])ht[ImageID];

            return base.File(arrImg, "image/jpeg");
            */
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
