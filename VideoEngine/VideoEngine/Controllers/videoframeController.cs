using Microsoft.AspNetCore.Mvc;
using Jugnoon.Utility;
using Jugnoon.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Jugnoon.Videos;
using Jugnoon.Videos.Models;
using Jugnoon.Localize;
using System.Collections.Generic;

namespace VideoEngine.Controllers
{
    public class videoframeController : Controller
    {
        ApplicationDbContext _context;
        public videoframeController(
           IOptions<SiteConfiguration> settings,
           IMemoryCache memoryCache,
           ApplicationDbContext context,
           IStringLocalizer<GeneralResource> generalLocalizer,
           IStringLocalizer<VideoResource> videoLocalizer,
           IWebHostEnvironment _environment,
           IHttpContextAccessor _httpContextAccessor,
           IOptions<General> generalSettings,
           IOptions<Features> featureSettings,
           IOptions<Media> mediaSettings,
           IOptions<Premium> premiumSettings,
           IOptions<Smtp> smtpSettings,
           IOptions<Jugnoon.Videos.Settings.General> generalVideoSettings,
           IOptions<Jugnoon.Videos.Settings.Aws> awsVideoSettings
           )
        {
            _context = context;

            // readable settings (global)
            Jugnoon.Settings.Configs.GeneralSettings = generalSettings.Value;
            Jugnoon.Settings.Configs.FeatureSettings = featureSettings.Value;
            Jugnoon.Settings.Configs.PremiumSettings = premiumSettings.Value;
            Jugnoon.Settings.Configs.SmtpSettings = smtpSettings.Value;
            Jugnoon.Settings.Configs.MediaSettings = mediaSettings.Value;
            // video settings specific
            Jugnoon.Videos.Configs.GeneralSettings = generalVideoSettings.Value;
            Jugnoon.Videos.Configs.AwsSettings = awsVideoSettings.Value;


            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
            SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.videoLocalizer = videoLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }
        // GET: media
        public async Task<IActionResult> Index(long? pid)
        {
            var model = new VideoFrameViewModel();
            model.isAllowed = true;
            if (pid == null)
            {
                model.isAllowed = false;
                model.DetailMessage = SiteConfig.generalLocalizer["_no_records"].Value;
                return View(model);
            }

            var _list = await VideoBLL.LoadItems(_context, new VideoEntity()
            {
                id = (long)pid,
                ispublic = false,
                issummary = false,
                isdropdown = false,
                nofilter = true
            });

            if (_list.Count == 0)
            {
                model.isAllowed = false;
                model.DetailMessage = SiteConfig.generalLocalizer["_no_records"].Value;
                return View(model);
            }

            model.Data = _list[0];

            if (_list[0].isapproved == (byte)ApprovedTypes.Disabled)
            {
                model.Message = SiteConfig.videoLocalizer["_video_pending_status"].Value;
            }
            if (_list[0].isenabled == (byte)EnabledTypes.Disabled)
            {
                model.isAllowed = false;
                model.DetailMessage = SiteConfig.videoLocalizer["_video_blocked_status"].Value;
                return View(model);
            }

            // simple view statistics
            _list[0].views = _list[0].views + 1;
            VideoBLL.Update_Field_V3(_context, (long)pid, model.Data.views, "views");

            model.Data.views++;

            // Video Path settings
            string VideoPath = "";
            string PictureName = "";


            if (model.Data.type == 0)
            {
                // video settings
                // if streamoutputs exist, video transcoded via Elastic Trancoder. It have own setup below.
                if (_list[0].streamoutputs == null || _list[0].streamoutputs == "")
                {
                    VideoPath = VideoUrlConfig.Return_FLV_Video_Url(model.Data.pub_url, model.Data.userid) + "/" + model.Data.videofilename;
                    /*string MP4Path = "";
                    if (VideoPath.EndsWith(".webm"))
                        MP4Path = VideoPath.Replace(".webm", ".mp4");*/
                }

                if (model.Data.thumb_url != "none")
                    PictureName = model.Data.thumb_url;
                else
                    PictureName = VideoUrlConfig.Return_Video_Thumb_Url(model.Data.thumb_url, model.Data.userid) + "/" + model.Data.thumbfilename;

            }
            else
            {
                // audio settings
                VideoPath = VideoUrlConfig.Return_MP3_Audio_Url(model.Data.pub_url, model.Data.userid) + "/" + model.Data.videofilename;
                if (model.Data.thumb_url != "none")
                    PictureName = model.Data.thumb_url;
                else if (_list[0].coverurl.StartsWith("http"))
                    PictureName = _list[0].coverurl;
                else
                    PictureName = VideoUrlConfig.Return_Video_Thumb_Url(model.Data.thumb_url, model.Data.userid) + "/" + model.Data.thumbfilename;
            }

            string _script = _list[0].embed_script;
            string _videoUrl = "";
            if (_list[0].preview_url != null && _list[0].preview_url != "")
                _videoUrl = _list[0].preview_url;
            else
                _videoUrl = _list[0].pub_url;

            model.Player = new VideoJsModelView()
            {
                EmbedScript = "",
                Type = model.Data.type
            };

            if (_list[0].coverurl != null && _list[0].coverurl != "")
                model.Player.PictureUrl = _list[0].coverurl;
            else
                model.Player.PictureUrl = PictureName;

            // attach video data with player model
            // useful when you bind some video data within player
            model.Player.Data = _list[0];

            if (model.Data.type == 0)
            {
                // video player settings
                // if AWS Uploader Enabled
                if (_list[0].streamoutputs != null && _list[0].streamoutputs != "")
                {
                    // enabled advance adaptive streaming / mpeg-dash / hls
                    model.Player.enabledAWS = true;
                    model.Player.VideoFeed = new List<SupportedVideos>();
                    var supportedStreamOptions = _list[0].streamoutputs.Split(char.Parse(","));
                    foreach (var option in supportedStreamOptions)
                    {
                        /*if (option == "mpegdash")
                        {
                            //CloudFront.CreateCannedPrivateURL(_list[0].pub_url + "mpegdash.mpd", 3000),
                            model.Player.VideoFeed.Add(new SupportedVideos()
                            {
                                src = CloudFront.CreateCannedPrivateURL(_list[0].pub_url + "mpegdash.mpd", 900), 
                                type = "application/dash+xml"
                            });
                        }
                        
                        if (option == "hls")
                        {
                            // CloudFront.CreateCannedPrivateURL(_list[0].pub_url + "hls.m3u8", 3000),
                            model.Player.VideoFeed.Add(new SupportedVideos()
                            {
                                src = CloudFront.CreateCannedPrivateURL(_list[0].pub_url + "hls.m3u8", 900),
                                type = "application/x-mpegURL"
                            });
                        }*/
                        if (option == "mp4_480")
                        {
                            model.Player.VideoFeed.Add(new SupportedVideos()
                            {
                                //src = CloudFront.CreateCannedPrivateURL(_list[0].pub_url.Replace("d1fpw0dove1bub.cloudfront.net", "d16gr1zx2upz2x.cloudfront.net") + "menifest_480.mp4",900),
                                src = _list[0].pub_url.Replace(Jugnoon.Videos.Configs.AwsSettings.private_url, Jugnoon.Videos.Configs.AwsSettings.public_url) + "menifest_480.mp4",
                                type = "video/mp4"
                            });
                        }
                        // enable hd streaming (make sure it is enabled in AWS Lambda to publish
                        /*if (option == "mp4_720")
                        {
                            model.Player.VideoFeed.Add(new SupportedVideos()
                            {
                                src = CloudFront.CreateCannedPrivateURL(_list[0].pub_url + "menifest_720.mp4", 3000),
                                type = "video/mp4"
                            });
                        }*/
                    }
                }
                // If cloud enabled
                else if (_script != "" || _list[0].youtubeid != "")
                {
                    // third party embed video script enabled
                    if (_script == "")
                    {
                        model.Player.EmbedScript = _list[0].youtubeid;
                    }
                    else
                    {
                        model.Player.EmbedScript = _script;
                    }
                }
                else if (_videoUrl.StartsWith("http"))
                {
                    // cloud front streaming
                    model.Player.VideoUrl = CloudFront.CreateCannedPrivateURL(_videoUrl, 60);
                }
                else if (_list[0].videofilename.EndsWith("mp4"))
                {

                    if (TokenBLL.isTokenEnabled)
                    {
                        // Token Based Stream Authentication Enabled
                        // Recommended to stream on HTML5 Players
                        string maxParam = VideoBLL.GenerateBandwidthParam();
                        string Token = TokenBLL.Add(_context);
                        model.Player.VideoUrl = "/stream/token.ashx?f=" + _list[0].videofilename + "&u=" + _list[0].userid + "&tk=" + Token + "" + maxParam;
                        if (_list[0].originalvideofilename.EndsWith(".png"))
                            model.Player.PictureUrl = "/contents/member/" + _list[0].userid + "/thumbs/" + _list[0].originalvideofilename;
                    }
                    else if (VideoBLL.EnableHttpStreaming)
                    {
                        // Http Streaming Enabled
                        // Recommended to stream on HTML5 Players
                        string maxParam = VideoBLL.GenerateBandwidthParam();
                        model.Player.VideoUrl = "/stream/stream.ashx?f=" + _list[0].videofilename + "&u=" + _list[0].userid + "" + maxParam;
                    }
                    else
                    {
                        // Direct Play
                        model.Player.VideoUrl = VideoPath;
                    }
                }
                else
                {
                    // load and play videos normally
                    model.Player.VideoUrl = VideoPath;
                }
            }
            else
            {
                // audio player settings
                model.Player.VideoUrl = VideoPath;
                model.Player.PictureUrl = PictureName;
            }

            string meta_title = _list[0].title;
            if (meta_title == "")
                meta_title = "Media No " + _list[0].id;

            string _desc = UtilityBLL.StripHTML(_list[0].description);
            if (_desc == "")
                _desc = _list[0].title;
            else if (_desc.Length > 160)
                _desc = _desc.Substring(0, 160);

            ViewBag.title = meta_title;
            ViewBag.description = _desc + ", date uploaded: " + _list[0].created_at + ", VID: " + _list[0].id;

            return View(model);
        }

        public IActionResult preview(string url = null)
        {
            var model = new VideoPreviewViewModel();
            if (url == null)
            {
                model.isExist = false;
            }
            else
            {
                model.isExist = true;
                model.previewUrl = WebUtility.HtmlDecode(url).Replace(",/videoframe/preview","");
            }
            return View(model);
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
