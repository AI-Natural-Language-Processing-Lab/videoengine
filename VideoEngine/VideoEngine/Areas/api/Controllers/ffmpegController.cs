using System;
using System.IO;
using Jugnoon.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using VideoEngine.Models;
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
using System.Collections.Generic;
using Newtonsoft.Json;
using Jugnoon.Videos;
using Jugnoon.Setup;
using Jugnoon.Videos.Models;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ffmpegController : ControllerBase
    {
        private readonly IWritableOptions<Jugnoon.Videos.Settings.Ffmpeg> _ffmpeg_options;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public ffmpegController(
             IOptions<SiteConfiguration> settings,
              IStringLocalizer<GeneralResource> generalLocalizer,
             IWebHostEnvironment _environment,
             IHttpContextAccessor _httpContextAccessor,
             IWritableOptions<Jugnoon.Videos.Settings.Ffmpeg> ffmpeg_options,
             IOptions<Jugnoon.Videos.Settings.Ffmpeg> ffmpeg_settings,
             IOptions<Jugnoon.Videos.Settings.General> generalVideoSettings
         )
        {
            // readable configuration
            Jugnoon.Videos.Configs.GeneralSettings = generalVideoSettings.Value;
            Jugnoon.Videos.Configs.FfmpegSettings = ffmpeg_settings.Value;
            // other
            SiteConfig.Config = settings.Value;
            _ffmpeg_options = ffmpeg_options;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        // GET: api/ffmpeg
        // GET: api/ffmpeg
        // Store all encoding processes in static (shared) list of mediahandler objects
        // Each mediahandler object, control single encoding process accross application
        // static object required to manage each encoding separately in shared environment 
        //where there is chances of concurrent encoding request at a time.
        public static List<MediaHandler> _lst = new List<MediaHandler>();

        //
        // GET: /api/video/published
        [HttpPost("published")]
        public ActionResult published()
        {
            Response.ContentType = "text/plain";

            var json = new StreamReader(Request.Body).ReadToEnd();
            List<VideoFiles> data = JsonConvert.DeserializeObject<List<VideoFiles>>(json);
            string publishedPath = VideoUrlConfig.Published_Video_Path(data[0].username);

            var _log = new List<VideoLog>();
            foreach (VideoFiles file in data)
            {
                if (System.IO.File.Exists(publishedPath + "/" + file.filename))
                {
                    // file uploaded successfully.

                    // write code to upload published file to cloud storage e.g amazon s3 here.

                    string ErrorCode = file.errorcode;
                    string fileName = file.filename; // published filename
                    // rename published file to avoid duplicate issue
                    string newfileName = Guid.NewGuid().ToString().Substring(0, 15) + System.IO.Path.GetExtension(fileName);
                    System.IO.File.Copy(publishedPath + "/" + file.filename, publishedPath + "/" + newfileName);
                    // renmove older file
                    System.IO.File.Delete(publishedPath + "/" + file.filename);
                    
                    string sourcefilename = file.sfle; // source file name
                    string duration = file.duration;
                    string durationsec = file.durationsec; // video duration in seconds

                    // write code to delete original video if not required after publishing
                    if (System.IO.File.Exists(publishedPath + "/" + file.sfle))
                    {
                        System.IO.File.Delete(publishedPath + "/" + file.sfle);
                    }

                    // file operation completed, create log
                    _log.Add(new VideoLog()
                    {
                        errorcode = file.errorcode,
                        message = SiteConfig.generalLocalizer["_records_processed"].Value,
                        publishedfile = file.filename,
                        sourcefile = file.sfle,
                        status = "success"
                    });
                }
                else
                {
                    // file not found
                    _log.Add(new VideoLog()
                    {
                        errorcode = file.errorcode,
                       message = SiteConfig.generalLocalizer["_no_records"].Value,
                        publishedfile = file.filename,
                        sourcefile = file.sfle,
                        status = "error"
                    });
                }
            }
            // complete published operation
            var _report = new PublishedReport()
            {
                status = "success",
                message = SiteConfig.generalLocalizer["_records_processed"].Value,
                report = _log
            };

            return Ok(_report);

        }

        //
        // GET: /api/video/encode
        [HttpPost("encode")]
        public ActionResult encode()
        {
            var rootPath = SiteConfig.Environment.ContentRootPath + "/wwwroot/";
            var ffmpegPath = rootPath + Jugnoon.Videos.Configs.FfmpegSettings.ffmpeg_path;
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<EncodeFFMPEGVideo>>(json);
            int ActionType = data[0].tp;
            string username = data[0].userid;

            var _response = new Dictionary<string, string>();
            _response["encodeoutput"] = "2.0";

            string Source = "";
            string Published = "";
            string ProcessID = "";
            switch (ActionType)
            {
                case 0:
                    // encode video
                    Source = data[0].sf;
                    Published = data[0].pf;

                    if (Source != "" && Published != null)
                    {
                        var _mhandler = new MediaHandler();
                        string RootPath = SiteConfig.Environment.ContentRootPath;
                        _mhandler.FFMPEGPath = ffmpegPath;
                        _mhandler.InputPath = UrlConfig.Upload_Path(username, "default");
                        _mhandler.OutputPath = UrlConfig.Upload_Path(username, "flv");
                        _mhandler.BackgroundProcessing = true;
                        _mhandler.FileName = Source;
                        _mhandler.OutputFileName = Published.Replace(Path.GetExtension(Published), "");
                        switch(Jugnoon.Videos.Configs.FfmpegSettings.encoding_options)
                        {
                            case 0:
                                // 240p
                                _mhandler.Parameters = Jugnoon.Videos.Configs.FfmpegSettings.mp4_240p_Settings;
                                break;
                            case 1:
                                // 360p
                                _mhandler.Parameters = Jugnoon.Videos.Configs.FfmpegSettings.mp4_360p_Settings;
                                break;
                            case 2:
                                // 480p
                                _mhandler.Parameters = Jugnoon.Videos.Configs.FfmpegSettings.mp4_480p_Settings;
                                break;
                            case 3:
                                // 720
                                _mhandler.Parameters = Jugnoon.Videos.Configs.FfmpegSettings.mp4_720p_Settings;
                                break;
                            case 4:
                                // 1080p
                                _mhandler.Parameters = Jugnoon.Videos.Configs.FfmpegSettings.mp4_1080p_Settings;
                                break;
                        }
                        _mhandler.OutputExtension = ".mp4";
                        _mhandler.vinfo = _mhandler.ProcessMedia();
                        if (_mhandler.vinfo.ErrorCode > 0)
                        {
                            // remove file if failed to publish properly
                            if (System.IO.File.Exists(RootPath + "/" + _mhandler.InputPath))
                                System.IO.File.Delete(RootPath + "/" + _mhandler.InputPath);

                            _response["encodeoutput"] = "2.0";
                            _response["ecode"] = _mhandler.vinfo.ErrorCode.ToString();
                            _response["edesc"] = _mhandler.vinfo.FFMPEGOutput.ToString();


                            var _message = new System.Text.StringBuilder();
                            _message.Append("<h4>Video Upload Error</h4>");
                            _message.Append("<p>Error:" + _mhandler.vinfo.ErrorCode + " _ _ " + _mhandler.vinfo.ErrorMessage + "</p>");
                            _message.Append("<p>Source FileName: " + Source);
                            _message.Append("<p>Published FileName: " + Published);
                            MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Publishing Error, description", _message.ToString());
                            
                            return Ok(_response);
                        }
                        else
                        {
                            // _mhandler.vinfo.ProcessID = Guid.NewGuid().ToString(); // unique guid to attach with each process to identify proper object on progress bar and get info request
                            _lst.Add(_mhandler);
                            _response["encodeoutput"] = "2.0";
                            _response["ecode"] = _mhandler.vinfo.ErrorCode.ToString();
                            _response["procid"] = _mhandler.vinfo.ProcessID; // _mhandler.vinfo.ProcessID;
                            return Ok(_response);
                        }
                    }
                    break;
                case 1:
                    // get progress status
                    ProcessID = data[0].pid;
                    if (ProcessID != "")
                    {
                        string completed_process = "0";
                        if (_lst.Count > 0)
                        {
                            int i = 0;
                            for (i = 0; i <= _lst.Count - 1; i++)
                            {
                                if (_lst[i].vinfo.ProcessID == ProcessID)
                                {
                                    completed_process = Math.Round(_lst[i].vinfo.ProcessingCompleted, 2).ToString();
                                }
                            }
                        }

                        _response["encodeoutput"] = "2.0";
                        _response["status"] = completed_process;
                        return Ok(_response);
                    }

                    break;
                case 2:
                    // get information
                    ProcessID = data[0].pid;
                    if (ProcessID != "")
                    {
                        if (_lst.Count > 0)
                        {
                            int i = 0;
                            for (i = 0; i <= _lst.Count - 1; i++)
                            {
                                if (_lst[i].vinfo.ProcessID == ProcessID)
                                {
                                    _response["status"] = "OK";
                                    _response["ecode"] = _lst[i].vinfo.ErrorCode.ToString();
                                    _response["fname"] = _lst[i].vinfo.FileName.ToString();

                                    _response["dur"] = _lst[i].vinfo.Duration.ToString();
                                    _response["dursec"] = _lst[i].vinfo.Duration_Sec.ToString();


                                    // remove from list of corrent processes if processes reach this point
                                    // store all information of completed process and remove it from list of concurrent processes
                                    // e.g
                                    VideoInfo current_uploaded_video_info = _lst[i].vinfo;
                                    _lst.Remove(_lst[i]);

                                    // Validation 
                                    int plength = 0;
                                    string path = UrlConfig.Upload_Path(username, "flv") + "\\" + data[0].pf;
                                    if (System.IO.File.Exists(path))
                                    {
                                        FileInfo flv_info = new FileInfo(path);
                                        plength = (int)flv_info.Length;
                                    }
                                    if (plength == 0)
                                    {
                                        var _message = new System.Text.StringBuilder();
                                        _message.Append("<h4>Video Publishing Error</h4>");
                                        _message.Append("<p>Error: 0kb file generated</p>");
                                        _message.Append("<p>Source FileName: " + Source);
                                        _message.Append("<p>Published FileName: " + Published);
                                        MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Publishing Error, description", _message.ToString());
                                    }

                                    // ii: add meta information to mp4 video
                                    try
                                    {
                                        var mp4med = new MediaHandler();
                                        mp4med.MP4BoxPath = ffmpegPath;
                                        string _mp4_temp_path = "\"" + UrlConfig.Upload_Path(username, "flv") + "/" + data[0].pf + "\"";
                                        string meta_filename = data[0].sf.Replace(".mp4", "_meta.mp4");
                                        mp4med.Parameters = "-isma -hint -add " + _mp4_temp_path + "";
                                        mp4med.FileName = meta_filename;
                                        mp4med.InputPath = UrlConfig.Upload_Path(username, "flv");
                                        mp4med.Set_MP4_Buffering();

                                        // check whether file created
                                        string pubPath = UrlConfig.Upload_Path(username, "flv");
                                        if (System.IO.File.Exists(pubPath + "\\" + meta_filename))
                                        {
                                            // remove temp mp4 file
                                            if (System.IO.File.Exists(pubPath + "" + data[0].pf))
                                                System.IO.File.Delete(pubPath + "\\" + data[0].pf);

                                            _response["fname"] = meta_filename;
                                        }
                                        else
                                        {
                                            // file not created by mp4box
                                            // rename published mp4 as _meta.mp4
                                            System.IO.File.Move(pubPath + "\\" + data[0].pf, pubPath + "\\" + meta_filename);
                                            _response["fname"] = meta_filename;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var _message = new System.Text.StringBuilder();
                                        _message.Append("<h4>Video Meta Information Error</h4>");
                                        _message.Append("<p>Error: " + ex.Message + "</p>");
                                        _message.Append("<p>Source FileName: " + Source);
                                        _message.Append("<p>Published FileName: " + Published);
                                        MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Publishing Error, description", _message.ToString());
                                    }

                                    _response["isenable"] = "1";

                                    // Thumb Grabbing Script
                                    string thumb_start_index = "";
                                    try
                                    {
                                        var med = new MediaHandler();
                                        med.FFMPEGPath = ffmpegPath;
                                        med.InputPath = UrlConfig.Upload_Path(username, "default"); // RootPath + "\\" + SourcePath;
                                        med.OutputPath = UrlConfig.Upload_Path(username, "thumbs"); // RootPath + "\\" + PublishedPath;
                                        med.FileName = data[0].sf; // source file
                                        thumb_start_index = med.FileName.Replace(Path.GetExtension(med.FileName), "_");
                                        med.Image_Format = "jpg";
                                        med.VCodec = "image2"; //optional
                                        med.ACodec = "";
                                        med.ImageName = thumb_start_index;
                                        med.Multiple_Thumbs = true;
                                        med.ThumbMode = 0;
                                        med.No_Of_Thumbs = 10;
                                        med.Thumb_Start_Position = 5; // start grabbing thumbs from 5th second
                                                                      //if (this.BackgroundProcessing)
                                                                      //    med.BackgroundProcessing = true;
                                        int width = Jugnoon.Videos.Configs.GeneralSettings.thumbnail_width;
                                        if (width > 0)
                                            med.Width = width;
                                        int height = Jugnoon.Videos.Configs.GeneralSettings.thumbnail_height;
                                        if (height > 0)
                                            med.Height = height;
                                        var tinfo = med.Grab_Thumb();
                                        if (tinfo.ErrorCode > 0)
                                        {
                                            // Error occured in grabbing thumbs - Rollback process
                                            _response["ecode"] = "1006";
                                            _response["edesc"] = "Grabbing thumbs from video failed";

                                            var _message = new System.Text.StringBuilder();
                                            _message.Append("<h4>Thumb Generation Error</h4>");
                                            _message.Append("<p>Error: " + _response["edesc"] + "</p>");
                                            _message.Append("<p>Source FileName: " + Source);
                                            _message.Append("<p>Published FileName: " + Published);
                                            MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Thumb Generation Error, description", _message.ToString());

                                            // call rollback script here
                                            return Ok(_response);
                                        }

                                        // Validate Thumbs
                                        path = UrlConfig.Upload_Path(username, "thumbs") + "/" + thumb_start_index;
                                        if (!System.IO.File.Exists(path + "004.jpg") || !System.IO.File.Exists(path + "008.jpg") || !System.IO.File.Exists(path + "011.jpg"))
                                        {
                                            // thumb failed try again grabbing thumbs from published video
                                            med.InputPath = UrlConfig.Upload_Path(username, "default");
                                            med.FileName = data[0].pf; // grab thumb from encoded video
                                            tinfo = med.Grab_Thumb();
                                            if (tinfo.ErrorCode > 0)
                                            {
                                                // Error occured in grabbing thumbs - Rollback process
                                                _response["ecode"] = "1006";
                                                _response["edesc"] = "Grabbing thumbs from video failed";
                                                // rollback script here
                                                var _message = new System.Text.StringBuilder();
                                                _message.Append("<h4>Thumb Generation Error</h4>");
                                                _message.Append("<p>Error: " + _response["edesc"] + "</p>");
                                                _message.Append("<p>Source FileName: " + Source);
                                                _message.Append("<p>Published FileName: " + Published);
                                                MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Thumb Generation Error, description", _message.ToString());
                                                return Ok(_response);
                                            }
                                            // Disable Video
                                            if (!System.IO.File.Exists(path + "004.jpg") || !System.IO.File.Exists(path + "008.jpg") || !System.IO.File.Exists(path + "011.jpg"))
                                            {
                                                _response["isenable"] = "0"; // disable video - thumbs not grabbed properly.
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _response["ecode"] = "1010";
                                        _response["edesc"] = ex.Message;

                                        var _message = new System.Text.StringBuilder();
                                        _message.Append("<h4>Thumb Generation Error</h4>");
                                        _message.Append("<p>Error: " + ex.Message + "</p>");
                                        _message.Append("<p>Source FileName: " + Source);
                                        _message.Append("<p>Published FileName: " + Published);
                                        MailProcess.Send_Mail(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, "Thumb Generation Error, description", _message.ToString());
                                    }

                                    _response["tfile"] = thumb_start_index + "" + "008.jpg";
                                    _response["fIndex"] = thumb_start_index;
                                    _response["img_url"] = Config.GetUrl("contents/member/" + username + "/thumbs/"); // + _response["tfile"]);
                                }
                            }
                        }
                        return Ok(_response);
                    }

                    break;
                case 3:
                    // final check
                    ProcessID = data[0].pid;
                    Source = data[0].sf;
                    Published = data[0].pf;

                    if (ProcessID != "" && Source != "" && Published != "")
                    {
                        if (_lst.Count > 0)
                        {
                            int i = 0;
                            for (i = 0; i <= _lst.Count - 1; i++)
                            {
                                if (_lst[i].vinfo.ProcessID == ProcessID)
                                {
                                    if (_lst[i].vinfo.ProcessingCompleted >= 100)
                                    {
                                        // check whether published file uploaded properly
                                        string publishedPath = UrlConfig.Upload_Path(username, "flv");

                                        if (!System.IO.File.Exists(publishedPath + "/" + Published))
                                        {
                                            _response["status"] = "INVALID";// published file not found
                                        }
                                        else
                                        {
                                            _response["encodeoutput"] = "2.0";
                                            _response["status"] = "OK";
                                        }
                                    }
                                }
                            }
                        }

                        return Ok(_response);
                    }
                    break;
            }
            _response["status"] = "INVALID";
            return Ok(_response);
        }


        public static string GetErrorCode(string ProcessID)
        {
            string ErrorCode = "0";
            if (_lst.Count > 0)
            {
                int i = 0;
                for (i = 0; i <= _lst.Count - 1; i++)
                {
                    if (_lst[i].vinfo.ProcessID == ProcessID)
                    {
                        ErrorCode = _lst[i].vinfo.ErrorCode.ToString();
                    }
                }
            }

            return ErrorCode;
        }

        public static string GetFFMPEGOutPut(string ProcessID)
        {
            string Output = "";
            if (_lst.Count > 0)
            {
                int i = 0;
                for (i = 0; i <= _lst.Count - 1; i++)
                {
                    if (_lst[i].vinfo.ProcessID == ProcessID)
                    {
                        Output = _lst[i].vinfo.FFMPEGOutput.ToString();
                    }
                }
            }

            return Output;
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

            var uploadPath = SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(DirectoryPaths.UserVideosDefaultDirectoryPath, UserName.ToString());
            if (!Directory.Exists(uploadPath))
            {
                Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, UserName.ToString()));
            }

            /*if (!Directory.Exists(uploadPath))
            {
                return Ok(new { jsonrpc = "2.0", result = "Error", fname = uploadPath, message = "Main Directory Not Exist" });
            }

            if (!Directory.Exists(uploadPath + "default/"))
            {
                return Ok(new { jsonrpc = "2.0", result = "Error", fname = uploadPath + "default/", message = "Default Directory Not Exist" });
            }*/

            var fileName = "";
            try
            {
               
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

            }
            catch (Exception ex)
            {
                return Ok(new { jsonrpc = "2.0", result = "Error", fname = uploadPath, message = ex.Message });
            }
          

            string url = VideoUrlConfig.Source_Video_Url(UserName.ToString()) + "/" + fileName; 
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
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
