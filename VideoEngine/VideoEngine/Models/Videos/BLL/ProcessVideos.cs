using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Jugnoon.Entity;
using Jugnoon.Framework;
using Jugnoon.Scripts;
using Jugnoon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Jugnoon.BLL;
using Jugnoon.Videos.Models;
using System.Threading.Tasks;

namespace Jugnoon.Videos
{
    public class ProcessVideos
    {
        public static async Task<object> direct_proc(ApplicationDbContext context, List<SaveVideoInfo> data)
        {
            foreach (var _rec in data)
            {
                var _counter = 1;
                foreach (var thumb in _rec.video_thumbs)
                {
                    var _prefix = "00";
                    if (_counter <= 9)
                        _prefix = "00";
                    else if (_counter <= 99)
                        _prefix = "0";
                    else
                        _prefix = "";
                    
                    /* save base64 image in physical path */
                    byte[] image = Convert.FromBase64String(thumb.filename.Replace("data:image/png;base64,", ""));
                    string thumbFileName = _rec.videofilename.Remove(_rec.videofilename.LastIndexOf(".")) + "_" + _prefix + "" + _counter + ".png";
                    _counter++;
                    string path = VideoUrlConfig.Thumbs_Path(_rec.userid) + "/" + thumbFileName;
                    if (File.Exists(path))
                        File.Delete(path);

                    File.WriteAllBytes(path, image);

                    if (thumb.selected)
                        _rec.thumbfilename = thumbFileName;

                    _rec.tfile = _rec.thumbfilename;
                }
                var PubPrefix = Guid.NewGuid().ToString().Substring(0, 12) + "-"; // avoid duplication in cloud
                try
                {
                    // in case of direct uploader, there is no published video, source video is published.
                    // shift source video to published directory
                    var SourcePath = VideoUrlConfig.Source_Video_Path(_rec.userid); // no source video there
                    var publishedPath = VideoUrlConfig.Published_Video_Path(_rec.userid);
                    var thumbsPath = VideoUrlConfig.Thumbs_Path(_rec.userid);
                    if (File.Exists(publishedPath + "/" + _rec.videofilename))
                        File.Delete(publishedPath + "/" + _rec.videofilename);

                    File.Move(SourcePath + "/" + _rec.videofilename, publishedPath + "/" + _rec.videofilename);
                    if (Jugnoon.Settings.Configs.AwsSettings.enable)
                    {
                        if (Configs.AwsSettings.bucket != "")
                        {
                            //var previewVideoUrl = ""; // not yet added in this version
                            var _arr = new ArrayList();
                            _arr.Add(_rec.videofilename);

                            if (_rec.thumbfilename != null)
                            {
                                var _thumbFileName = _rec.thumbfilename;
                                if (_thumbFileName.Contains("_"))
                                    _thumbFileName = _thumbFileName.Remove(_thumbFileName.LastIndexOf("_"));
                                else
                                    _thumbFileName = _thumbFileName.Replace(".jpg", "");
                            }
                            // string status = MediaCloudStorage.UploadMediaFiles("", "", publishedPath, _arr, PubPrefix, thumbsPath, _thumbFileName, _rec.thumbs.Count(), "", _rec.userid, ".png");
                            // thumb is not provided, just sent via thumb_url in this version
                            string status = MediaCloudStorage.UploadMediaFiles("", "", publishedPath, _arr, PubPrefix, "", "", 0, "", _rec.userid, ".png");
                            if (status == "PubFailed" || status == "ThumbFailed")
                            {
                                ErrorLgBLL.Add(context, "Error Uploading to Cloud", "", "Error Code 1009, message: storing content to cloud failed");
                            }
                        }
                        else
                        {
                            ErrorLgBLL.Add(context, "Error Uploading to Cloud", "", "Cloud Storage Enabled But No Cloud Storage Settings Available");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;
                }

                var cloud_url = Url.prepareUrl(_rec.userid);

                // update entity values
                _rec.videofilename = _rec.videofilename;
                _rec.thumbfilename = _rec.tfile;
                _rec.originalvideofilename = _rec.sf;
                _rec.pub_url = cloud_url.publish_filename + PubPrefix + _rec.videofilename;
                _rec.thumb_url = _rec.thumb_url; // cloud_url.thumb_filename + _rec.thumbfilename;
                _rec.org_url = "";
                _rec.isexternal = 0;
                _rec.youtubeid = "";
                _rec.ispublished = 1;
                _rec.embed_script = "";
               
                await VideoBLL.AddVideo(context, _rec);
               
            }
            return new { status = "success", message = "Record processed successfully" };
        }

        public static async Task<object> aws_proc(ApplicationDbContext context, List<SaveVideoInfo> data)
        {
            if (Jugnoon.Settings.Configs.AwsSettings.enable)
            {
                if (Configs.AwsSettings.bucket != "")
                {
                    try
                    {
                        foreach (var _rec in data)
                        {
                            _rec.isenabled = 1; // by default enabled
                            if (Jugnoon.Settings.Configs.GeneralSettings.content_approval == 1)
                                _rec.isapproved = 1;
                            else
                                _rec.isapproved = 0;

                            var SourcePath = VideoUrlConfig.Source_Video_Path(_rec.userid); // no source video there
                            if (File.Exists(SourcePath + "/" + _rec.sf))
                            {                                
                                string source_key_prefix = Jugnoon.Videos.Configs.AwsSettings.elastic_transcoder_directory + "" + Guid.NewGuid().ToString().Substring(0, 10);
                                _rec.originalvideofilename = source_key_prefix + "-" + UtilityBLL.ReplaceSpaceWithHyphin_v2(_rec.sf);
                                // Save unpublished video data in database first before uploading file to aws

                                // update entity values
                                _rec.videofilename = "";
                                _rec.thumbfilename = "";
                                _rec.pub_url = "";
                                _rec.thumb_url = "";
                                _rec.org_url = "";
                                _rec.isexternal = 0;
                                _rec.youtubeid = "";
                                _rec.ispublished = 0;
                                _rec.embed_script = "";

                                await VideoBLL.AddVideo(context, _rec);

                                string status = MediaCloudStorage.UploadMediaFiles_Elastic(SourcePath, _rec.sf, _rec.originalvideofilename, _rec.userid);
                                if (status == "OrgFailed")
                                {
                                    ErrorLgBLL.Add(context, "Error Uploading to Cloud", "", "Error Code 1009, message: storing content to cloud failed");
                                }
                            }
                            else
                            {
                                ErrorLgBLL.Add(context, "Cloud Upload Error", "", "File Not Exist " + SourcePath + "/" + _rec.sf);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLgBLL.Add(context, "Upload Video Error", "", ex.Message);
                    }

                }
            }

            return new { status = "success", message = "Record processed successfully" };
        }

        public static async Task<object> ffmpeg_proc(ApplicationDbContext context, List<SaveVideoInfo> data)
        {
            foreach (var _rec in data)
            {
                var _ref_filename = "";
                var _SelectedIndex = "";
                foreach (var thumb in _rec.video_thumbs)
                {
                    if (thumb.selected)
                    {
                        if (Jugnoon.Settings.Configs.AwsSettings.enable)
                        {
                            _rec.thumbfilename = Path.GetFileName(thumb.filename).Remove(Path.GetFileName(thumb.filename).LastIndexOf("_")) + "/img" + thumb.filename.Remove(0, thumb.filename.LastIndexOf("_"));
                            // _rec.thumbfilename = Path.GetFileNameWithoutExtension(thumb.filename) + "/img_" + thumb.filename.Remove(0, thumb.filename.LastIndexOf("_"));
                        }
                        else
                        {
                            _rec.thumbfilename = Path.GetFileName(thumb.filename);
                        }
                        _ref_filename = Path.GetFileName(thumb.filename);

                        _SelectedIndex = thumb.fileIndex;
                        _rec.thumb_url = thumb.filename;
                    }

                }
                var _thumbFile = "";
                var PubPrefix = Guid.NewGuid().ToString().Substring(0, 12) + "-"; // avoid duplication in cloud
                try
                {
                    if (Jugnoon.Settings.Configs.AwsSettings.enable)
                    {
                        if (Configs.AwsSettings.bucket != "")
                        {
                            var SourcePath = VideoUrlConfig.Source_Video_Path(_rec.userid);
                            var publishedPath = VideoUrlConfig.Published_Video_Path(_rec.userid);
                            var thumbsPath = VideoUrlConfig.Thumbs_Path(_rec.userid);
                            var previewVideoUrl = ""; // not yet added in this version
                            var _arr = new ArrayList();
                            _arr.Add(_rec.pf);
                            var _thumbFileName = _ref_filename;
                            if (_thumbFileName.Contains("_"))
                                _thumbFileName = _thumbFileName.Remove(_thumbFileName.LastIndexOf("_"));
                            else
                                _thumbFileName = _thumbFileName.Replace(".jpg", "");

                            // add key to avoid duplication
                            thumbsPath = thumbsPath + "\\" + _thumbFileName + "_";
                           
                            _thumbFileName = PubPrefix + "-" + _thumbFileName;
                            var ext = Path.GetExtension(_ref_filename);
                            _thumbFile = _thumbFileName + "/" + _SelectedIndex + ext; // "img_008" + ext;
                            string status = MediaCloudStorage.UploadMediaFiles(SourcePath, _rec.sf, publishedPath, _arr, PubPrefix, thumbsPath, _thumbFileName, 15, previewVideoUrl, _rec.userid,Path.GetExtension(ext));
                            if (status == "PubFailed" || status == "ThumbFailed")
                            {
                                ErrorLgBLL.Add(context, "Error Uploading to Cloud", "", "Error Code 1009, message: storing content to cloud failed");
                            }
                        }
                        else
                        {
                            ErrorLgBLL.Add(context, "Error Uploading to Cloud", "", "Cloud Storage Enabled But No Cloud Storage Settings Available");
                        }
                    }
                }
                catch (Exception ex)
                {
                    var err = ex.Message;

                }

                var cloud_url = Url.prepareUrl(_rec.userid);

                // update entity values
                _rec.videofilename = _rec.pf;
                _rec.thumbfilename = Path.GetFileName(_rec.tfile);
                _rec.originalvideofilename = _rec.sf;
                _rec.pub_url = cloud_url.publish_filename + PubPrefix + _rec.pf;
                _rec.thumb_url = cloud_url.thumb_filename + _thumbFile;
                _rec.org_url = cloud_url.publish_filename + _rec.sf;
                _rec.preview_url = "";
                _rec.isexternal = 0;
                _rec.youtubeid = "";
                _rec.ispublished = 1;
                _rec.embed_script = "";

                await VideoBLL.AddVideo(context, _rec);

            }
            return new { status = "success", message = "Record processed successfully" };
        }

        public static async Task<object> yt_proc(ApplicationDbContext context, List<SaveVideoInfo> data)
        {
            foreach (var _rec in data)
            {
                _rec.isenabled = 1; // by default enabled

                // update entity values
                _rec.videofilename = "";
                _rec.thumbfilename = "";
                _rec.originalvideofilename = "";
                _rec.pub_url = "";
                _rec.thumb_url = _rec.preview_url;
                _rec.org_url = "";
                _rec.isexternal = 1;
                _rec.youtubeid = _rec.youtubeid;
                _rec.ispublished = 1;
                _rec.embed_script = "";

                await VideoBLL.AddVideo(context, _rec);

            }
            return new { status = "success", message = "Record processed successfully" };
        }

      

        /* single youtbue video fetcher */
        public object yt_single_proc(ApplicationDbContext context, YoutubeEntity query)
        {
            var model = new JGN_Videos();
            string ytid = Fetch_YoutubeID(query.term);
            if (ytid == "")
            {
                return new { status = "error", message = "Youtube video url failed to validate, please enter proper youtube id" };
            }

            string DeveloperID = Configs.YoutubeSettings.key; // SiteConfiguration.YoutubeDeveloperKey;

            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = DeveloperID,
                    ApplicationName = this.GetType().ToString()
                });
                var searchListRequest = youtubeService.Videos.List("snippet,contentDetails");
                searchListRequest.Id = ytid;

                var searchListResponse = searchListRequest.Execute();

                List<string> videos = new List<string>();

                foreach (var searchResult in searchListResponse.Items)
                {

                    string videoid = searchResult.Id;
                    if (videoid.Contains(":"))
                        videoid = videoid.Remove(0, videoid.LastIndexOf(":") + 1);

                    string video_watch_id = "/watch?v=" + videoid;
                    model.title = searchResult.Snippet.Title;
                    model.description = searchResult.Snippet.Description;
                    model.duration = ""; // searchResult.ContentDetails.Duration.ToString();

                    ArrayList thumb_urls = new ArrayList();
                    /* foreach (MediaThumbnail thumbnail in video.Thumbnails)
                    {
                        thumb_urls.Add(thumbnail.Url);
                        // str.AppendLine("\tThumbnail URL: " + thumbnail.Url + "<br />");
                        // str.AppendLine("\tThumbnail time index: " + thumbnail.Time + "<br />");
                    }*/
                    if (searchResult.Snippet.Thumbnails.High != null)
                        thumb_urls.Add(searchResult.Snippet.Thumbnails.High.Url);
                    else if (searchResult.Snippet.Thumbnails.Medium != null)
                        thumb_urls.Add(searchResult.Snippet.Thumbnails.Medium.Url);
                    else if (searchResult.Snippet.Thumbnails.Standard != null)
                        thumb_urls.Add(searchResult.Snippet.Thumbnails.Standard.Url);
                    //else if (searchResult.Snippet.Thumbnails.Default != null)
                    //    thumb_urls.Add(searchResult.Snippet.Thumbnails.Default.Url);

                    if (thumb_urls.Count > 1)
                    {
                        int count = Convert.ToInt32(thumb_urls.Count / 2);
                        model.preview_url = thumb_urls[count].ToString();
                    }
                    else
                    {
                        model.preview_url = thumb_urls[0].ToString();
                    }

                    /*foreach (Google.GData.YouTube.MediaContent mediaContent in searchResult.Snippet.contents)
                    {
                       // str.AppendLine("\tMedia Location: " + mediaContent.Url + "<br />");
                       // str.AppendLine("\tMedia Type: " + mediaContent.Format + "<br />");
                       this.duration = mediaContent.duration;
                    }*/

                    model.youtubeid = video_watch_id;
                }
            }
            catch (Exception ex)
            {
                ErrorLgBLL.Add(context, "Embed Video - Fetch Error", "", ex.Message);
                return new { status = "error", message = ex.Message };
            }


            /*YouTubeQuery query = new YouTubeQuery("http://gdata.youtube.com/feeds/api/videos/" + ytid);
            //Feed<Video> videoFeed = req.Get<Video>(query);
            Video video = req.Retrieve<Video>(query);
            ProcessVideoEntry(video); */
            var lst = new List<JGN_Videos>();
            lst.Add(model);
            return new { status = "success", posts = lst };

        }
        private string Fetch_YoutubeID(string URL)
        {
            string reg = @"(?<=v(\=|\/))(?<yid>([-a-zA-Z0-9_]+))|(?<=youtu\.be\/)(?<yid2>([-a-zA-Z0-9_]+))";
            var VDMatch = Regex.Match(URL, reg);
            if (VDMatch.Success)
            {
                if (VDMatch.Groups["yid"].Value != "")
                    return VDMatch.Groups["yid"].Value; // normal pattern
                else
                    return VDMatch.Groups["yid2"].Value; // pattern like http://youtu.be/[id]
            }
            else
                return "";
        }
        /* multiple youtube video fetcher */
        public object yt_multiple_proc(ApplicationDbContext context, YoutubeEntity entity)
        {
            var _youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Configs.YoutubeSettings.key,
                ApplicationName = this.GetType().ToString()
            });

            var model = new List<JGN_Videos>();

            var searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = entity.term;
            // order date
            switch (entity.order)
            {
                case 0:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                    break;
                case 1:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;
                    break;
                case 2:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
                    break;
                case 3:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Title;
                    break;
                case 4:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.VideoCount;
                    break;
                case 5:
                    searchListRequest.Order = SearchResource.ListRequest.OrderEnum.ViewCount;
                    break;
            }

            if (entity.youtubecategory != null && entity.youtubecategory != "")
            {
                searchListRequest.VideoCategoryId = entity.youtubecategory;
                searchListRequest.Type = "video";
            }

            // upload date
            if (entity.uploaddate != 3)
            {
                switch (entity.uploaddate)
                {
                    case 0:
                        searchListRequest.PublishedAfter = DateTime.Now.AddDays(-1);
                        break;
                    case 1:
                        searchListRequest.PublishedAfter = DateTime.Now.AddDays(-7);
                        break;
                    case 2:
                        searchListRequest.PublishedAfter = DateTime.Now.AddDays(-31);
                        break;
                }
            }

            var ChannelID = "";
            var ChannelTitle = "";
            if (entity.userid != null && entity.userid != "")
            {
                var channeListRequest = _youtubeService.Channels.List("snippet");
                channeListRequest.ForUsername = entity.userid;
                try
                {
                    var channeListResponse = searchListRequest.Execute();
                    foreach (var searchResult in channeListResponse.Items)
                    {
                        var data = searchResult;
                        ChannelID = searchResult.Snippet.ChannelId;
                        ChannelTitle = searchResult.Snippet.ChannelTitle;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLgBLL.Add(context, "Exception Youtube Service - Fetch Channels", "", ex.Message);
                }
            }

            // Replace with your search term.
            //searchListRequest.ChannelId = ""; // Search Channel ID
            //searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date; //
            // searchListRequest.RelatedToVideoId = "";
            // searchListRequest.VideoCategoryId = "";
            //searchListRequest.VideoDuration = SearchResource.ListRequest.VideoDurationEnum.Any;
            // searchListRequest.VideoLicense = SearchResource.ListRequest.VideoLicenseEnum.Any;
            /* Note that you must set the type parameter to video if you set a value for the eventType, videoCaption, videoCategoryId, videoDefinition, videoDimension, videoDuration, videoEmbeddable, videoLicense, videoSyndicated, or videoType parameters. */
            searchListRequest.MaxResults = 50;
            // searchListRequest.Type = "video";
            if (ChannelID != null && ChannelID != "")
                searchListRequest.ChannelId = ChannelID;

            //searchListRequest.ChannelId = "mediasoftpro1";
            try
            {
                var searchListResponse = searchListRequest.Execute();

                var videos = new List<string>();
                //List<string> channels = new List<string>();
                //List<string> playlists = new List<string>();

                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            // add youtube video functionality

                            string videoid = searchResult.Id.VideoId;
                            if (videoid.Contains(":"))
                                videoid = videoid.Remove(0, videoid.LastIndexOf(":") + 1);

                            string video_watch_id = "/watch?v=" + videoid;
                            if (!VideoBLL.Check_YOUTUBE_ID(context, video_watch_id))
                            {
                                string title = searchResult.Snippet.Title;
                                string description = searchResult.Snippet.Description;

                                string thumburl = "";
                                ArrayList thumb_urls = new ArrayList();
                                if (searchResult.Snippet.Thumbnails.Maxres != null)
                                    thumb_urls.Add(searchResult.Snippet.Thumbnails.Maxres.Url);
                                else if (searchResult.Snippet.Thumbnails.High != null)
                                    thumb_urls.Add(searchResult.Snippet.Thumbnails.High.Url);
                                else if (searchResult.Snippet.Thumbnails.Standard != null)
                                    thumb_urls.Add(searchResult.Snippet.Thumbnails.Standard.Url);
                                //else if (searchResult.Snippet.Thumbnails.Default != null)
                                //   thumb_urls.Add(searchResult.Snippet.Thumbnails.Default.Url);


                                if (thumb_urls.Count > 1)
                                {
                                    int count = Convert.ToInt32(thumb_urls.Count / 2);
                                    thumburl = thumb_urls[count].ToString();
                                }
                                else
                                {
                                    thumburl = thumb_urls[0].ToString();
                                }

                                // process and add video
                                string flv_filename = "";
                                string original_filename = "";
                                string thumb_filename = "";
                                string duration = "";
                                int isapproved = 1;
                                /*if (model.isapproved)
                                    isapproved = 1;*/
                                // set video actions : 1 -> on, 0 -> off
                                int isenabled = 1;
                                int ispublished = 1;
                                //int isapproved = 1;
                                int isprivate = 0;

                                int isexternal = 1;
                                string pub_url = "none";
                                string _embed = "";

                                string ipaddress = "";
                                //string username = "";
                                //username = Site_Settings.Yt_UserName;

                                // remove <a tags from description
                                if (description != null && description != "")
                                {
                                    Regex _href = new Regex(@"<a[^<]+?>", RegexOptions.Singleline); // remove <a href.....>
                                    Regex _hrefend = new Regex(@"</a>", RegexOptions.Singleline); // remove </a>
                                    description = _href.Replace(description, string.Empty);
                                    description = _hrefend.Replace(description, string.Empty);

                                    if (description.Length > 2000)
                                        description = description.Substring(0, 2000);

                                }
                                
                                var vd = new JGN_Videos();
                                vd.userid = entity.userid;
                                vd.title = title;
                                if (description != null)
                                    vd.description = description;
                                vd.tags = "";
                                vd.duration = duration;
                                vd.duration_sec = 0; // Convert.ToInt32(duration_sec);
                                vd.originalvideofilename = original_filename;
                                vd.videofilename = flv_filename;
                                vd.thumbfilename = thumb_filename;
                                vd.isprivate = (byte)isprivate;
                                vd.isenabled = (byte)isenabled;

                                vd.ispublished = (byte)ispublished;
                                vd.isapproved = (byte)isapproved;
                                vd.pub_url = pub_url;
                                vd.preview_url = thumburl;
                                vd.embed_script = _embed;
                                vd.isexternal = (byte)isexternal;
                                vd.ipaddress = ipaddress;
                                vd.type = 0;
                                vd.youtubeid = video_watch_id;

                                model.Add(vd);
                            }

                            break;
                        case "youtube#channel":
                            //channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                            break;

                        case "youtube#playlist":
                            //playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLgBLL.Add(context, "Exception Youtube Service - Process Videos", "", ex.Message);
                return new { status = "success", posts = ex.Message };
            }

            return new { status = "success", posts = model };
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
