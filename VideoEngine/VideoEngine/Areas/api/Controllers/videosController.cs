using Jugnoon.BLL;
using Jugnoon.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jugnoon.Utility;
using Newtonsoft.Json;
using Jugnoon.Settings;
using System.Collections;
using Jugnoon.Scripts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using Jugnoon.Utility.Helper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Jugnoon.Setup;
using Jugnoon.Videos.Models;
using Jugnoon.Videos;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class videosController : ControllerBase
    {
        ApplicationDbContext _context;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        // writable injector configurations specific to videos
        private readonly IWritableOptions<Jugnoon.Videos.Settings.General> _general_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Aws> _aws_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Ffmpeg> _ffmpeg_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Direct> _direct_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Movie> _movie_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Player> _player_options;
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Youtube> _youtube_options;
        public videosController(
            IOptions<SiteConfiguration> settings,
            IMemoryCache memoryCache,
            ApplicationDbContext context,
            IStringLocalizer<GeneralResource> generalLocalizer,
            IStringLocalizer<VideoResource> videoLocalizer,
            IWebHostEnvironment _environment,
            IHttpContextAccessor _httpContextAccessor,
            IWritableOptions<Jugnoon.Videos.Settings.General> general_options,
            IWritableOptions<Jugnoon.Videos.Settings.Aws> aws_options,
            IWritableOptions<Jugnoon.Videos.Settings.Ffmpeg> ffmpeg_options,
            IWritableOptions<Jugnoon.Videos.Settings.Direct> direct_options,
            IWritableOptions<Jugnoon.Videos.Settings.Movie> movie_options,
            IWritableOptions<Jugnoon.Videos.Settings.Player> player_options,
            IWritableOptions<Jugnoon.Videos.Settings.Youtube> youtube_options,
            IOptions<General> generalSettings,
            IOptions<Aws> awsSettings,
            IOptions<Media> mediaSettings,
            IOptions<Smtp> smtpSettings,
            IOptions<Features> featureSettings,
            IOptions<Registration> registerSettings,
            IOptions<Jugnoon.Videos.Settings.General> generalVideoSettings,
            IOptions<Jugnoon.Videos.Settings.Aws> awsVideoSettings,
            IOptions<Jugnoon.Videos.Settings.Direct> directVideoSettings,
            IOptions<Jugnoon.Videos.Settings.Movie> movieVideoSettings,
            IOptions<Jugnoon.Videos.Settings.Youtube> youtubeVideoSettings,
             IOptions<Jugnoon.Videos.Settings.Ffmpeg> ffmpegVideoSettings
        )
        {
            // readable settigns (global)
            Jugnoon.Settings.Configs.GeneralSettings = generalSettings.Value;
            Jugnoon.Settings.Configs.AwsSettings = awsSettings.Value;
            Jugnoon.Settings.Configs.MediaSettings = mediaSettings.Value;
            Jugnoon.Settings.Configs.SmtpSettings = smtpSettings.Value;
            Jugnoon.Settings.Configs.FeatureSettings = featureSettings.Value;
            Jugnoon.Settings.Configs.RegistrationSettings = registerSettings.Value;
            // readable settings (content specific)
            Jugnoon.Videos.Configs.GeneralSettings = generalVideoSettings.Value;
            Jugnoon.Videos.Configs.AwsSettings = awsVideoSettings.Value;
            Jugnoon.Videos.Configs.DirectSettings = directVideoSettings.Value;
            Jugnoon.Videos.Configs.MovieSettings = movieVideoSettings.Value;
            Jugnoon.Videos.Configs.YoutubeSettings = youtubeVideoSettings.Value;
            Jugnoon.Videos.Configs.FfmpegSettings = ffmpegVideoSettings.Value;

            // writable configuration injectors
            _general_options = general_options;
            _aws_options = aws_options;
            _ffmpeg_options = ffmpeg_options;
            _direct_options = direct_options;
            _movie_options = movie_options;
            _player_options = player_options;
            _youtube_options = youtube_options;
            // normal injectors
            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            _context = context;
           
            SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.videoLocalizer = videoLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<VideoEntity>(json);

            var _posts = await VideoBLL.LoadItems(_context, data);

            /* setup thumb path */
            foreach (var item in _posts)
            {
                item.picturename = VideoUtil.ProcessVideoThumb("", item, false, new ListItems()); // default set
                item.url = VideoUrlConfig.PrepareUrl(item);
                item.author_url = UserUrlConfig.ProfileUrl(item.author, Jugnoon.Settings.Configs.RegistrationSettings.uniqueFieldOption);
                item.customize_date = UtilityBLL.CustomizeDate((DateTime)item.created_at, DateTime.Now);
            }
            var _records = 0;
            if (data.id == 0)
                _records = await VideoBLL.Count(_context, data);
            var _type = CategoryBLL.Types.Videos;
            /*switch (data.type)
            {
                case MediaType.Audio:
                    _type = CategoryBLL.Types.Audio; // audio
                    break;
            }*/
            var _categories = new List<JGN_Categories>();
            if (data.loadstats)
            {
                _categories = await CategoryBLL.LoadItems(_context, new CategoryEntity()
                {
                    id = 0,
                    type = (int)_type,
                    mode = 0,
                    isenabled = EnabledTypes.All,
                    parentid = -1,
                    order = "level asc", // don't change this
                    issummary = false,
                    isdropdown = true,
                    loadall = true // load all data
                });
            }

            var _settings = new
            {
                general = Jugnoon.Videos.Configs.GeneralSettings,
                aws = Jugnoon.Videos.Configs.AwsSettings,
                direct = Jugnoon.Videos.Configs.DirectSettings,
                movie = Jugnoon.Videos.Configs.MovieSettings,
                youtube = Jugnoon.Videos.Configs.YoutubeSettings,
                ffmpeg = Jugnoon.Videos.Configs.FfmpegSettings
            };

            return Ok(new { posts = _posts, records = _records, categories = _categories, settings = _settings });
        }

        [HttpPost("load_reports")]
        public async Task<ActionResult> load_reports()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<VideoEntity>(json);
            var _reports = await VideoBLL.LoadReport(_context, data);
            return Ok(new { data = _reports });
        }

        [HttpPost("direct_proc")]
        public async Task<ActionResult> direct_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            try
            {
                var data = JsonConvert.DeserializeObject<List<SaveVideoInfo>>(json);

                return Ok(await ProcessVideos.direct_proc(_context, data));
            }
            catch(Exception ex)
            {
                return Ok(new { status = "error", message = ex.Message });
            }
          
        }

        [HttpPost("aws_proc")]
        public async Task<ActionResult> aws_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<SaveVideoInfo>>(json);

            return Ok(await ProcessVideos.aws_proc(_context, data));
        }

        [HttpPost("ffmpeg_proc")]
        public async Task<ActionResult> ffmpeg_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<SaveVideoInfo>>(json);

            return Ok(await ProcessVideos.ffmpeg_proc(_context, data));
        }
     

        [HttpPost("yt_proc")]
        public async Task<ActionResult> yt_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<SaveVideoInfo>>(json);

            return Ok(await ProcessVideos.yt_proc(_context, data));
        }

        [HttpPost("embed_proc")]
        public async Task<ActionResult> embed_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Videos>(json);

            return Ok(await VideoBLL.Add_Embed_Video(_context, data));
        }


        [HttpPost("movie_proc")]
        public async Task<ActionResult> movie_proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Videos>(json);

            return Ok(await VideoBLL.Add_Movie(_context, data));
        }

        [HttpPost("get_yt_categories")]
        public ActionResult get_yt_categories()
        {
            var _yt = new YoutubeCategories();
            var model = new YoutubeEntity
            {
                OrderList = YoutubeTypes.OrderList(),
                YoutubeCategories = _yt.GetVideoCategories(_context),
                DateList = YoutubeTypes.DateList(),
                uploaddate = 3
            };

            return Ok(new { categorylist = model });
        }

        [HttpPost("fetch_youtube")]
        public ActionResult fetch_youtube()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<YoutubeEntity>(json);

            if (data.term == "")
            {
                return Ok(new { status = "error", message = "please enter term" });
            }

            var obj = new ProcessVideos();
            if (data.term.StartsWith("http"))
                return Ok(obj.yt_single_proc(_context, data)); // single youtube video
            else
                return Ok(obj.yt_multiple_proc(_context, data)); // multiple youtube videos
        }


        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<VideoEntity>(json);
            data.issummary = false;
            data.isdropdown = false;
            data.nofilter = true;
            var _posts = await VideoBLL.LoadItems(_context, data);
            if (_posts.Count == 0)
            {
                return Ok(new { status = "error", message = "no record found" });
            }

            _posts[0].picturename = VideoUtil.ProcessVideoThumb("", _posts[0], false, new ListItems()); // default set
            _posts[0].customize_date = UtilityBLL.CustomizeDate((DateTime)_posts[0].created_at, DateTime.Now);
            var model = _posts[0];
            // Player processing
            // Video Path settings
            string VideoPath = "";
            string PictureName = "";
            if (model.type == 0)
            {
                // video settings
                VideoPath = VideoUrlConfig.Return_FLV_Video_Url(model.pub_url, model.userid) + "/" + model.videofilename;
                if (model.thumb_url != "none")
                    PictureName = model.thumb_url;
                else
                    PictureName = VideoUrlConfig.Return_Video_Thumb_Url(model.thumb_url, model.userid) + "/" + model.thumbfilename;
                string MP4Path = "";
                if (VideoPath.EndsWith(".webm"))
                    MP4Path = VideoPath.Replace(".webm", ".mp4");
            }
            else
            {
                // audio settings
                VideoPath = VideoUrlConfig.Return_MP3_Audio_Url(model.pub_url, model.userid) + "/" + model.videofilename;
                if (model.thumb_url != "none")
                    PictureName = model.thumb_url;
                else if (model.coverurl.StartsWith("http"))
                    PictureName = model.coverurl;
                else
                    PictureName = VideoUrlConfig.Return_Video_Thumb_Url(model.thumb_url, model.userid) + "/" + model.thumbfilename;
            }

            // script specifically for updating default video thumbnail
            _posts[0].thumb_url = VideoUtil.ProcessVideoThumb("", _posts[0], false, new ListItems()); // default set
            // setup thumb index 
            if (_posts[0].thumb_url.Contains("_"))
                _posts[0].picturename = _posts[0].thumb_url.Remove(_posts[0].thumb_url.LastIndexOf("_"));
            else
                _posts[0].picturename = _posts[0].thumb_url;

            
            _posts[0].author.img_url = UserUrlConfig.ProfilePhoto(_posts[0].author.Id, _posts[0].author.picturename, 0);
            // array of associate category list
            _posts[0].category_list = await CategoryContentsBLL.FetchContentCategoryList(_context, data.id, (byte)CategoryContentsBLL.Types.Videos);

            string _script = model.embed_script;
            string _videoUrl = "";
            if (model.preview_url != null && model.preview_url != "")
                _videoUrl = model.preview_url;
            else
                _videoUrl = model.pub_url;

            model.player = new PlayerEntity();
            model.player.embedscript = "";
            model.player.youtubeid = "";

            model.player.type = model.type;

            if (model.coverurl != "")
                model.player.picturename = model.coverurl;
            else
                model.player.picturename = PictureName;

            if (model.type == 0)
            {
                // video player settings
                // If cloud enabled
                if (_script != "" || model.youtubeid != "")
                {
                    // third party embed video script enabled
                    if (_script == "")
                    {
                        model.player.youtubeid = "https://www.youtube.com/embed/" + model.youtubeid.Replace("/watch?v=", "");
                    }
                    else
                    {
                        model.player.embedscript = _script;
                    }
                }
                else if (_videoUrl.StartsWith("http"))
                {
                    // cloud front streaming
                    model.player.url = Jugnoon.Utility.CloudFront.CreateCannedPrivateURL(_videoUrl, 60);
                }
                else if (model.videofilename.EndsWith("mp4"))
                {
                    /* if (TokenBLL.isTokenEnabled)
                    {
                        // Token Based Stream Authentication Enabled
                        // Recommended to stream on HTML5 Players
                        string maxParam = VideoBLL.GenerateBandwidthParam();
                        string Token = TokenBLL.Add();
                        model.player.url = "/stream/token.ashx?f=" + model.videofilename + "&u=" + model.username + "&tk=" + Token + "" + maxParam;
                        if (model.originalvideofilename.EndsWith(".png"))
                            model.player.picturename = "/contents/member/" + model.username + "/thumbs/" + model.originalvideofilename;
                    }
                    else if (VideoBLL.EnableHttpStreaming)
                    {
                        // Http Streaming Enabled
                        // Recommended to stream on HTML5 Players
                        string maxParam = VideoBLL.GenerateBandwidthParam();
                        model.player.url = "/stream/stream.ashx?f=" + model.videofilename + "&u=" + model.username + "" + maxParam;
                    }
                    else
                    {
                        // Direct Play
                        model.player.url = VideoPath;
                    } */
                }
                else
                {
                    // load and play videos normally
                    model.player.url = VideoPath;
                }
            }
            else
            {
                // audio player settings
                model.player.url = VideoPath;
                model.player.picturename = PictureName;
            }

            return Ok(new { status = "success", post = model });
        }

        [HttpPost("getinfo_acc")]
        public async Task<ActionResult> getinfo_acc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<VideoEntity>(json);
            data.issummary = false;
            data.isdropdown = false;
            data.nofilter = true;
            var _posts = await VideoBLL.LoadItems(_context, data);
            if (_posts.Count == 0)
            {
                return Ok(new { status = "error", message = "no record found" });
            }

            // script specifically for updating default video thumbnail
            _posts[0].thumb_url = VideoUtil.ProcessVideoThumb("", _posts[0], false, new ListItems()); // default set
            // setup thumb index 
            if (_posts[0].thumb_url.Contains("_"))
                _posts[0].picturename = _posts[0].thumb_url.Remove(_posts[0].thumb_url.LastIndexOf("_"));
            else
                _posts[0].picturename = _posts[0].thumb_url;

            // array of associate category list
            _posts[0].category_list = await CategoryContentsBLL.FetchContentCategoryList(_context, data.id, (byte)CategoryContentsBLL.Types.Videos);
            return Ok(new { status = "success", post = _posts[0] });
        }

        [HttpPost("update_video_info")]
        public async Task<ActionResult> update_video_info()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var model = JsonConvert.DeserializeObject<JGN_Videos>(json);

            if (model.tags != "")
            {
                // validate tags
                if (!TagsBLL.Validate_Tags(model.tags))
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_tags"].Value });
                }
            }


            if (UtilityBLL.isLongWordExist(model.title) || UtilityBLL.isLongWordExist(model.title))
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_title"].Value });
            }


            var vd = new JGN_Videos();
            vd.title = model.title;
            vd.description = UGeneral.SanitizeText(model.description);
            vd.tags = model.tags;
            vd.id = model.id;
            vd.categories = model.categories;
            vd.iscomments = (byte)model.iscomments;
            vd.isratings = (byte)model.isratings;
            vd.isprivate = (byte)model.isprivate;
            vd.views = model.views;
            vd.liked = model.liked;
            vd.disliked = model.disliked;
            vd.avg_rating = (byte)model.avg_rating;
            vd.total_rating = model.total_rating;
            vd.ratings = (byte)model.avg_rating * model.ratings;
            vd.isenabled = (byte)model.isenabled;
            vd.isapproved = (byte)model.isapproved;
            vd.isfeatured = (byte)model.isfeatured;

            await VideoBLL.Update_VideoInfo_Adm(_context, vd, model.isadmin);

            // Process tags
            if (model.tags != "")
            {
                TagsBLL.Process_Tags(_context, model.tags, TagsBLL.Types.Videos, 0);
            }

            return Ok(new { status = "success", record = vd, message = SiteConfig.generalLocalizer["_record_updated"].Value });
        }

        [HttpPost("editvideos")]
        public async Task<ActionResult> editvideos()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<JGN_Videos>>(json);

            var model = data[0];

            if (model.tags != "")
            {
                // validate tags
                if (!TagsBLL.Validate_Tags(model.tags))
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_tags"].Value });
                }
            }


            if (UtilityBLL.isLongWordExist(model.title) || UtilityBLL.isLongWordExist(model.title))
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_title"].Value });
            }


            var vd = new JGN_Videos();
            vd.title = model.title;
            vd.description = UGeneral.SanitizeText(model.description);
            vd.tags = model.tags;
            vd.id = model.id;
            vd.categories = model.categories;
            //vd.iscomments = (byte)model.iscomments;
            //vd.isratings = (byte)model.isratings;
            //vd.isprivate = (byte)model.isprivate;
            //vd.views = model.views;
            //vd.liked = model.liked;
            //vd.disliked = model.disliked;
            //vd.avg_rating = (byte)model.avg_rating;
            //vd.total_rating = model.total_rating;
            //vd.ratings = (byte)model.avg_rating * model.ratings;
            ///vd.isenabled = (byte)model.isenabled;
            //vd.isapproved = (byte)model.isapproved;
            //vd.isfeatured = (byte)model.isfeatured;

            await VideoBLL.Update_VideoInfo_Adm_V2(_context, vd);

            // Process tags
            if (model.tags != "")
            {
                TagsBLL.Process_Tags(_context, model.tags, TagsBLL.Types.Videos, 0);
            }

            return Ok(new { status = "success", record = vd, message = SiteConfig.generalLocalizer["_record_updated"].Value });
        }

        [HttpPost("editvideos_macc")]
        public ActionResult editvideos_macc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Videos>(json);

            var model = data;

            if (model.id == 0)
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }
            if (model.tags != "")
            {
                // validate tags
                if (!TagsBLL.Validate_Tags(model.tags))
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_tags"].Value });
                }
            }


            if (UtilityBLL.isLongWordExist(model.title) || UtilityBLL.isLongWordExist(model.title))
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_invalid_title"].Value });
            }


            var vd = new JGN_Videos();
            vd.title = model.title;
            vd.description = UGeneral.SanitizeText(model.description);
            vd.tags = model.tags;
            vd.id = model.id;
            vd.categories = model.categories;
            vd.iscomments = (byte)model.iscomments;
            vd.isratings = (byte)model.isratings;
            vd.isprivate = (byte)model.isprivate;
            //vd.views = model.views;
            //vd.liked = model.liked;
            //vd.disliked = model.disliked;
            //vd.avg_rating = (byte)model.avg_rating;
            //vd.total_rating = model.total_rating;
            //vd.ratings = (byte)model.avg_rating * model.ratings;
            ///vd.isenabled = (byte)model.isenabled;
            //vd.isapproved = (byte)model.isapproved;
            //vd.isfeatured = (byte)model.isfeatured;

            VideoBLL.Update_Video_Info(_context, vd);

            // Process tags
            if (model.tags != "")
            {
                TagsBLL.Process_Tags(_context, model.tags, TagsBLL.Types.Videos, 0);
            }

            return Ok(new { status = "success", record = vd, message = SiteConfig.generalLocalizer["_record_updated"].Value });
        }

        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            StringValues UserName;
            SiteConfig.HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("UName", out UserName);

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            // string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                  MediaTypeHeaderValue.Parse(Request.ContentType),
                  _defaultFormOptions.MultipartBoundaryLengthLimit);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();

            var uploadPath = SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(Jugnoon.Videos.DirectoryPaths.UserVideosDefaultDirectoryPath, UserName.ToString());
            if (!Directory.Exists(uploadPath))
            {
                Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, UserName.ToString()));
            }

            var fileName = "";
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var output = formAccumulator.GetResults();
                        var chunk = "0";
                        foreach (var item in output)
                        {
                            if (item.Key == "name")
                                fileName = item.Value;
                            else if (item.Key == "chunk")
                                chunk = item.Value;
                        }

                        var Path = uploadPath + "" + fileName;
                        using (var fs = new FileStream(Path, chunk == "0" ? FileMode.Create : FileMode.Append))
                        {
                            await section.Body.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.ToString(), value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                var result = formAccumulator.GetResults();

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }


            string url = VideoUrlConfig.Source_Video_Url(UserName.ToString()) + "/" + fileName;
            string fileType = System.IO.Path.GetExtension(fileName);
            string fileIndex = fileName.Replace(fileType, "");

            return Ok(new { jsonrpc = "2.0", result = "OK", fname = fileName, url = url, filetype = fileType, filename = fileName, fileIndex = fileIndex });
        }

        [HttpPost("action")]
        public async Task<ActionResult> action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<VideoEntity>>(json);
           
            await VideoBLL.ProcessAction(_context, data);
            
            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("proc_new")]
        public async Task<ActionResult> proc_new()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<JGN_Videos>>(json);

            foreach (var _rec in data)
            {
                _rec.isenabled = 1; // by default enabled
                if (Jugnoon.Settings.Configs.GeneralSettings.content_approval == 1)
                    _rec.isapproved = 1;
                else
                    _rec.isapproved = 0;

                var _counter = 1;
                foreach (var thumb in _rec.thumbs)
                {
                    var _prefix = "00";
                    if (_counter <= 9)
                        _prefix = "00";
                    else if (_counter <= 99)
                        _prefix = "0";
                    else
                        _prefix = "";


                    /* save base64 image in physical path */
                    //byte[] image = Convert.FromBase64String(thumb.filename.Replace("data:image/png;base64,", ""));
                    string thumbFileName = Guid.NewGuid().ToString().Substring(0,6)
                        + _rec.videofilename.Remove(_rec.videofilename.LastIndexOf(".")) + "_" + _prefix + "" + _counter + ".png";
                    _counter++;
                    string old_path = VideoUrlConfig.Thumbs_Path(_rec.userid) + "/" + thumb.filename;
                    string new_path = VideoUrlConfig.Thumbs_Path(_rec.userid) + "/" + thumbFileName;

                    System.IO.File.Move(old_path, new_path);
                    //System.IO.File.WriteAllBytes(path, image);

                    //if (thumb.selected)
                    _rec.thumbfilename = thumbFileName;

                    //_rec.tfile = _rec.thumbfilename;
                }
                try
                {
                    // in case of direct uploader, there is no published video, source video is published.
                    // shift source video to published directory
                    var SourcePath = VideoUrlConfig.Source_Video_Path(_rec.userid); // no source video there
                    var publishedPath = VideoUrlConfig.Published_Video_Path(_rec.userid);
                    var thumbsPath = VideoUrlConfig.Thumbs_Path(_rec.userid);
                    if (System.IO.File.Exists(publishedPath + "/" + _rec.videofilename))
                        System.IO.File.Delete(publishedPath + "/" + _rec.videofilename);

                    System.IO.File.Move(SourcePath + "/" + _rec.videofilename, publishedPath + "/" + _rec.videofilename);
                    if (Jugnoon.Settings.Configs.AwsSettings.enable) // old ref: Jugnoon.Settings.Configs.AwsSettings.enable
                    {
                        if (Jugnoon.Videos.Configs.AwsSettings.bucket != "") // old ref: CloudSettings.VideoBucketName
                        {
                            //var previewVideoUrl = ""; // not yet added in this version
                            var _arr = new ArrayList();
                            _arr.Add(_rec.videofilename);
                            var _thumbFileName = _rec.thumbfilename;
                            if (_thumbFileName.Contains("_"))
                                _thumbFileName = _thumbFileName.Remove(_thumbFileName.LastIndexOf("_"));
                            else
                                _thumbFileName = _thumbFileName.Replace(".jpg", "");

                            string status = MediaCloudStorage.UploadMediaFiles_V2("", "", publishedPath, _arr, thumbsPath, _thumbFileName, _rec.thumbs.Count(), "", _rec.userid, "png");
                            if (status == "PubFailed" || status == "ThumbFailed")
                            {
                                ErrorLgBLL.Add(_context, "Error Uploading to Cloud", "", "Error Code 1009, message: storing content to cloud failed");
                            }
                        }
                        else
                        {
                            ErrorLgBLL.Add(_context, "Error Uploading to Cloud", "", "Cloud Storage Enabled But No Cloud Storage Settings Available");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                }

                var cloud_url = Jugnoon.Videos.Url.prepareUrl(_rec.userid);

                var Item = new JGN_Videos()
                {
                    title = _rec.title,
                    description = _rec.description,
                    tags = _rec.tags,
                    id = _rec.id,
                    categories = _rec.categories,
                    duration = _rec.duration,
                    originalvideofilename = _rec.videofilename,
                    videofilename = _rec.videofilename,
                    thumbfilename = _rec.thumbfilename,
                    isenabled = _rec.isenabled,
                    isapproved = _rec.isapproved,
                    userid = _rec.userid,
                    type = _rec.type,
                    duration_sec = _rec.duration_sec,
                    isprivate = 0, // _rec.privacy,
                    authkey = "",
                    pub_url = cloud_url.publish_filename + _rec.videofilename,
                    thumb_url = cloud_url.thumb_filename + _rec.thumbfilename,
                    org_url = cloud_url.source_filename + _rec.originalvideofilename,
                    isexternal = 0,
                    youtubeid = "",
                    mode = 0,
                    ispublished = 1,
                    errorcode = _rec.errorcode,
                    ipaddress = "",
                    embed_script = "",
                    // submittype = 2 // ffmpeg target type
                };

                Item.isapproved = 1;
                // Approved by Default
                if (Jugnoon.Settings.Configs.GeneralSettings.content_approval == 0)
                {
                    // Moderator Review Required
                    Item.isapproved = 0;
                }
                //XSS CLEANUP
                // not working on medium trust
                //string content = Sanitizer.GetSafeHtmlFragment(txt_content.Value);
                // Compress Code => remove \r\t etc which create unnecessary line breaking issues
                string content = UtilityBLL.CompressCode(Item.description);

                // Process Contents -> links, bbcodes etc
                content = UtilityBLL.Process_Content_Text(content);
                

                Item = await VideoBLL.Process_Info(_context, Item);

                if (Item.tags != "")
                {
                    var tag_type = TagsBLL.Types.Videos;
                    /*if (Item.type == 1)
                        tag_type = TagsBLL.Types.Audio;*/

                    TagsBLL.Process_Tags(_context, Item.tags, tag_type, 0);
                }
            }
            return Ok(new { status = "success", message = "video processed successfully" });

        }

        [HttpPost("thumbuploads")]
        public async Task<IActionResult> thumbuploads()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            StringValues UserName;
            SiteConfig.HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("UName", out UserName);

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();
            // string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                  MediaTypeHeaderValue.Parse(Request.ContentType),
                  _defaultFormOptions.MultipartBoundaryLengthLimit);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();

            var uploadPath = SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(Jugnoon.Videos.DirectoryPaths.UserVideoThumbsDirectoryPath, UserName.ToString());
            if (!Directory.Exists(uploadPath))
            {
                Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, UserName.ToString()));
            }


            var fileName = "";
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var output = formAccumulator.GetResults();
                        var chunk = "0";
                        foreach (var item in output)
                        {
                            if (item.Key == "name")
                                fileName = item.Value;
                            else if (item.Key == "chunk")
                                chunk = item.Value;
                        }

                        var Path = uploadPath + "" + fileName;
                        using (var fs = new FileStream(Path, chunk == "0" ? FileMode.Create : FileMode.Append))
                        {
                            await section.Body.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            formAccumulator.Append(key.ToString(), value);

                            if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }

                var result = formAccumulator.GetResults();

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            string uploadcompletepath = uploadPath + "\\" + fileName;
            string miduploadcompletepath = uploadPath + "\\" + "thumb_" + fileName;
            
            Image.Generate_Thumbs(uploadcompletepath, "", miduploadcompletepath, 
                Jugnoon.Videos.Configs.GeneralSettings.thumbnail_width,
                Jugnoon.Videos.Configs.GeneralSettings.thumbnail_height, 0, 0, 
                Jugnoon.Settings.Configs.MediaSettings.quality);

            string url = VideoUrlConfig.Thumb_Url(UserName) + "\\" + "thumb_" + fileName;
            
            string fileType = System.IO.Path.GetExtension(fileName);
            string fileIndex = fileName.Replace(fileType, "");

            return Ok(new { jsonrpc = "2.0", result = "OK", fname = fileName, url = url, filetype = fileType, filename = fileName, fileIndex = fileIndex });
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }

        [HttpPost("authorize_author")]
        public ActionResult authorize_author()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Videos>(json);
            var isaccess = VideoBLL.Check(_context, data.id, data.userid);
            return Ok(new { isaccess = isaccess });
        }

        [HttpPost("update_thumbnail")]
        public ActionResult update_thumbnail()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<JGN_Videos>(json);

            if (data.id == 0)
            {
                return Ok(new { status = "error", message = "no record found" });
            }
            if (data.picturename == "")
            {
                return Ok(new { status = "error", message = SiteConfig.videoLocalizer["_no_thumbnail"].Value });
            }

            VideoBLL.Update_Field_V3(_context, data.id, data.picturename, "thumb_url");

            return Ok(new { status = "success", message = SiteConfig.videoLocalizer["_video_thumbnail_updated"].Value });
        }

        #region Update Configuration API Calls

        [HttpPost("configs_general")]
        public ActionResult configs_general()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.General>(json);

            _general_options.Update(opt =>
            {
                opt.extensions = data.extensions;
                opt.max_size = data.max_size;
                opt.max_concurrent_uploads = data.max_concurrent_uploads;
                opt.delete_original = data.delete_original;
                opt.videoUploader_Type = data.videoUploader_Type;
                opt.enable_public_uploads = data.enable_public_uploads;
                opt.enable_playlists = data.enable_playlists;
                opt.enable_download = data.enable_download;
                opt.enable_public_uploads = data.enable_public_uploads;
                opt.enable_favorites = data.enable_favorites;
                opt.thumbnail_rotator_option = data.thumbnail_rotator_option;
                opt.default_path = data.default_path;
                opt.thumbnail_width = data.thumbnail_width;
                opt.thumbnail_height = data.thumbnail_height;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_aws")]
        public ActionResult configs_aws()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Aws>(json);

            _aws_options.Update(opt =>
            {
                opt.enable = data.enable;
                opt.enable_admin = data.enable_admin;
                opt.bucket = data.bucket;
                opt.source_directory_path = data.source_directory_path;
                opt.publish_directory_path = data.publish_directory_path;
                opt.thumbnail_directory_path = data.thumbnail_directory_path;
                opt.elastic_transcoder_directory = data.elastic_transcoder_directory;
                opt.public_url = data.public_url;
                opt.private_url = data.private_url;
                opt.cloudFront_keypair = data.cloudFront_keypair;
                opt.cloudFront_keyfilename = data.cloudFront_keyfilename;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_ffmpeg")]
        public ActionResult configs_ffmpeg()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Ffmpeg>(json);

            _ffmpeg_options.Update(opt =>
            {
                opt.enable = data.enable;
                opt.enable_admin = data.enable_admin;
                opt.video_Publishing_Type = data.video_Publishing_Type;
                opt.mp4_240p_Settings = data.mp4_240p_Settings;
                opt.mp4_360p_Settings = data.mp4_360p_Settings;
                opt.mp4_480p_Settings = data.mp4_480p_Settings;
                opt.mp4_720p_Settings = data.mp4_720p_Settings;
                opt.mp4_1080p_Settings = data.mp4_1080p_Settings;
                opt.encoding_options = data.encoding_options;
                opt.ffmpeg_path = data.ffmpeg_path;
                opt.mp4box_path = data.mp4box_path;
                opt.enable_clips = data.enable_clips;
                opt.clip_length = data.clip_length;
                opt.enable_preview_video = data.enable_preview_video;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_direct")]
        public ActionResult configs_direct()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Direct>(json);

            _direct_options.Update(opt =>
            {
                opt.enable = data.enable;
                opt.enable_admin = data.enable_admin;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_movie")]
        public ActionResult configs_movie()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Movie>(json);
            _movie_options.Update(opt =>
            {
                opt.enable = data.enable;
                opt.enabled_embed = data.enabled_embed;
                opt.enable_admin = data.enable_admin;
                opt.enable_embed_admin = data.enable_embed_admin;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_youtube")]
        public ActionResult configs_youtube()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Youtube>(json);

            _youtube_options.Update(opt =>
            {
                opt.enable = data.enable;
                opt.enable_admin = data.enable_admin;
                opt.key = data.key;
            });

            return Ok(new
            {
                status = 200
            });

        }

        [HttpPost("configs_player")]
        public ActionResult configs_player()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<Jugnoon.Videos.Settings.Player>(json);

            _player_options.Update(opt =>
            {
                opt.auto_play = data.auto_play;
                opt.enable_videojs = data.enable_videojs;
            });

            return Ok(new
            {
                status = 200
            });

        }
        #endregion

    }
}
/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
