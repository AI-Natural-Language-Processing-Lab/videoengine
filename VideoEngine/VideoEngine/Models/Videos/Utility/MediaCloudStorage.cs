using Jugnoon.Scripts;
using Jugnoon.Settings;
using System.Collections;
using System.IO;

namespace Jugnoon.Videos
{
    public class MediaCloudStorage
    {
        public static string UploadMediaFiles(string OrgDir, string OrgFile, string PubDir, ArrayList PubFiles, string PubPrefix, string ThumbDir, string ThumbFileName, int TotalThumbs, string previewvideo, string username, string ext = ".jpg")
        {
            string Contenttype = "";
            // Published Video Transferring
            string PublishedVideoStatus = "";
            string OriginalVideoStatus = "";
            string ThumbStatus = "";

            string video_path = Videos.Configs.AwsSettings.publish_directory_path; // old ref CloudSettings.VideoPath;
            string source_path = Videos.Configs.AwsSettings.source_directory_path; // old ref CloudSettings.VideoOriginalPath;
            string thumbs_path = Videos.Configs.AwsSettings.thumbnail_directory_path; // old ref CloudSettings.VideoCoverPath;

            string pub_prefix = username + "/";
            string thumb_prefix = username + "/";
            string source_prefix = username + "/";

            if (video_path != "")
            {
                if (video_path.EndsWith("/"))
                    pub_prefix = video_path + "" + pub_prefix;
                else
                    pub_prefix = video_path + "/" + pub_prefix;
            }
            if (source_path != "")
            {
                if (source_path.EndsWith("/"))
                    source_prefix = source_path + "" + source_prefix;
                else
                    source_prefix = source_path + "/" + source_prefix;
            }
            if (thumbs_path != "")
            {
                if (thumbs_path.EndsWith("/"))
                    thumb_prefix = thumbs_path + "" + thumb_prefix;
                else
                    thumb_prefix = thumbs_path + "/" + thumb_prefix;
            }

            string _filename = "";
            int i = 0;
            // Upload Preview Video
            if (previewvideo != "")
            {
                if (File.Exists(PubDir + "/" + previewvideo))
                {
                    var info = new FileInfo(PubDir + "/" + previewvideo);
                    Contenttype = "";

                    _filename = pub_prefix + PubPrefix + previewvideo;
                    PublishedVideoStatus = CloudStorage.UploadFile(PubDir + "/" + previewvideo, _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (PublishedVideoStatus == "OK" || PublishedVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(PubDir + "/" + previewvideo))
                            File.Delete(PubDir + "/" + previewvideo);
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = PublishedVideoStatus;
                        return "PubFailed";
                    }
                }
            }
            // Upload Published Videos and Audio Files

            for (i = 0; i <= PubFiles.Count - 1; i++)
            {
                if (File.Exists(PubDir + "/" + PubFiles[i].ToString()))
                {
                    FileInfo info = new FileInfo(PubDir + "/" + PubFiles[i].ToString());
                    Contenttype = "";

                    _filename = pub_prefix + PubPrefix + PubFiles[i].ToString();
                    PublishedVideoStatus = CloudStorage.UploadFile(PubDir + "/" + PubFiles[i].ToString(), _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (OriginalVideoStatus == "OK" || OriginalVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(PubDir + "/" + PubFiles[i].ToString()))
                            File.Delete(PubDir + "/" + PubFiles[i].ToString());
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = PublishedVideoStatus;
                        return "PubFailed";
                    }
                }
            }
            // Published Original Videos If Allowed
            if (!Videos.Configs.GeneralSettings.delete_original && OrgDir != "")
            {
                if (File.Exists(OrgDir + "/" + OrgFile))
                {
                    FileInfo info = new FileInfo(OrgDir + "/" + OrgFile);
                    Contenttype = "";
                    OriginalVideoStatus = CloudStorage.UploadFile(OrgDir + "/" + OrgFile, source_prefix, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (OriginalVideoStatus == "OK" || OriginalVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(OrgDir + "/" + OrgFile))
                            File.Delete(OrgDir + "/" + OrgFile);
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = OriginalVideoStatus;
                        // reset path for file as local server
                        return "OrgFailed";
                    }
                }
            }

            if (ThumbDir != "")
            {
                // Thumb Transferring
                //string StartIndex = ThumbFileName.Remove(ThumbFileName.LastIndexOf("_"));
                string ThumbPath = ThumbDir; // ThumbDir + "/" + ThumbFileName + "_";
                i = 0; // 15 thumb need to be renamed
                int counter = 1;
                for (i = 0; i <= TotalThumbs - 1; i++)
                {
                    string TPath = "";
                    string TFileName = "";
                    if (counter < 10)
                    {
                        TPath = ThumbPath + "00" + counter + ext;
                        TFileName = ThumbFileName + "/img_00" + counter + ext;
                    }
                    else if (counter <= 99)
                    {
                        TPath = ThumbPath + "0" + counter + ext;
                        TFileName = ThumbFileName + "/img_0" + counter + ext;
                    }
                    else
                    {
                        TPath = ThumbPath + counter + ext;
                        TFileName = ThumbFileName + "/img_" + counter + ext;
                    }
                    counter++;
                    if (File.Exists(TPath))
                    {
                        FileInfo info = new FileInfo(TPath);
                        Contenttype = "";
                        _filename = thumb_prefix + TFileName;
                        ThumbStatus = CloudStorage.UploadFile(TPath, _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                        if (ThumbStatus == "OK" || ThumbStatus == "")
                        {
                            // File successfully uploaded, delete file from server
                            if (File.Exists(TPath))
                                File.Delete(TPath);
                        }
                        else
                        {
                            // send log to administarator along with video information to review error.
                            string log = ThumbStatus;
                            return "ThumbFailed";
                        }
                    }
                }
            }
            
            return "Success";
        }

        // upload media files (Elastic Transcoder Version)
        public static string UploadMediaFiles_Elastic(string OrgDir, string OrgFile, string Key, string username)
        {
            string Contenttype = "";
            string OriginalVideoStatus = "";
            /*if (username == null || username == "")
                username = SiteConfig.userManager.GetUserName(User);*/

            //if (File.Exists(OrgDir + "/" + OrgFile))
            //{
                FileInfo info = new FileInfo(OrgDir + "/" + OrgFile);
                Contenttype = "";
                OriginalVideoStatus = CloudStorage.UploadFile(OrgDir + "/" + OrgFile, Key, Jugnoon.Videos.Configs.AwsSettings.bucket, Contenttype); // old ref: CloudSettings.ElasticTranscoder_VideoBucketName
                if (OriginalVideoStatus == "OK" || OriginalVideoStatus == "")
                {
                    // File successfully uploaded, delete file from server
                    if (File.Exists(OrgDir + "/" + OrgFile))
                        File.Delete(OrgDir + "/" + OrgFile);
                }
                else
                {
                    // send log to administarator along with video information to review error.
                    string log = OriginalVideoStatus;
                    // reset path for file as local server
                    return "OrgFailed";
                }
            //}

            return "Success";
        }

        // sending username too
        public static string UploadMediaFiles_V2(string OrgDir, string OrgFile, string PubDir, ArrayList PubFiles, string ThumbDir, string ThumbFileName, int TotalThumbs, string previewvideo, string username, string ext = "jpg")
        {
            string Contenttype = "";
            // Published Video Transferring
            string PublishedVideoStatus = "";
            string OriginalVideoStatus = "";
            string ThumbStatus = "";

            string video_path = Jugnoon.Videos.Configs.AwsSettings.publish_directory_path; // CloudSettings.VideoPath;
            string source_path = Jugnoon.Videos.Configs.AwsSettings.source_directory_path;  // CloudSettings.VideoOriginalPath;
            string thumbs_path = Jugnoon.Videos.Configs.AwsSettings.thumbnail_directory_path;  // CloudSettings.VideoCoverPath;

            string pub_prefix = username + "/";
            string thumb_prefix = username + "/";
            string source_prefix = username + "/";

            if (video_path != "")
            {
                if (video_path.EndsWith("/"))
                    pub_prefix = video_path + "" + pub_prefix;
                else
                    pub_prefix = video_path + "/" + pub_prefix;
            }
            if (source_path != "")
            {
                if (source_path.EndsWith("/"))
                    source_prefix = source_path + "" + source_prefix;
                else
                    source_prefix = source_path + "/" + source_prefix;
            }
            if (thumbs_path != "")
            {
                if (thumbs_path.EndsWith("/"))
                    thumb_prefix = thumbs_path + "" + thumb_prefix;
                else
                    thumb_prefix = thumbs_path + "/" + thumb_prefix;
            }

            string _filename = "";
            int i = 0;
            // Upload Preview Video
            if (previewvideo != "")
            {
                if (File.Exists(PubDir + "/" + previewvideo))
                {
                    var info = new FileInfo(PubDir + "/" + previewvideo);
                    Contenttype = "";

                    _filename = pub_prefix + previewvideo;
                    PublishedVideoStatus = CloudStorage.UploadFile(PubDir + "/" + previewvideo, _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (PublishedVideoStatus == "OK" || PublishedVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(PubDir + "/" + previewvideo))
                            File.Delete(PubDir + "/" + previewvideo);
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = PublishedVideoStatus;
                        return "PubFailed";
                    }
                }
            }
            // Upload Published Videos and Audio Files

            for (i = 0; i <= PubFiles.Count - 1; i++)
            {
                if (File.Exists(PubDir + "/" + PubFiles[i].ToString()))
                {
                    FileInfo info = new FileInfo(PubDir + "/" + PubFiles[i].ToString());
                    Contenttype = "";

                    _filename = pub_prefix + PubFiles[i].ToString();
                    PublishedVideoStatus = CloudStorage.UploadFile(PubDir + "/" + PubFiles[i].ToString(), _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (PublishedVideoStatus == "OK" || PublishedVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(PubDir + "/" + PubFiles[i].ToString()))
                            File.Delete(PubDir + "/" + PubFiles[i].ToString());
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = PublishedVideoStatus;
                        return "PubFailed";
                    }
                }
            }
            // Published Original Videos If Allowed
            if (!Videos.Configs.GeneralSettings.delete_original && OrgDir != "")
            {
                if (File.Exists(OrgDir + "/" + OrgFile))
                {
                    FileInfo info = new FileInfo(OrgDir + "/" + OrgFile);
                    Contenttype = "";
                    OriginalVideoStatus = CloudStorage.UploadFile(OrgDir + "/" + OrgFile, source_prefix, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (PublishedVideoStatus == "OK" || PublishedVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(OrgDir + "/" + OrgFile))
                            File.Delete(OrgDir + "/" + OrgFile);
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = OriginalVideoStatus;
                        // reset path for file as local server
                        return "OrgFailed";
                    }
                }
            }

            // Thumb Transferring
            //string StartIndex = ThumbFileName.Remove(ThumbFileName.LastIndexOf("_"));
            string ThumbPath = ThumbDir + "/" + ThumbFileName + "_";
            i = 0; // 15 thumb need to be renamed
            int counter = 1;
            for (i = 0; i <= TotalThumbs - 1; i++)
            {
                string TPath = "";
                string TFileName = "";
                if (counter < 10)
                {
                    TPath = ThumbPath + "00" + counter + "." + ext;
                    TFileName = ThumbFileName + "_00" + counter + "." + ext;
                }
                else if (counter <= 99)
                {
                    TPath = ThumbPath + "0" + counter + "." + ext;
                    TFileName = ThumbFileName + "_0" + counter + "." + ext;
                }
                else
                {
                    TPath = ThumbPath + counter + "." + ext;
                    TFileName = ThumbFileName + "_" + counter + "." + ext;
                }
                counter++;
                if (File.Exists(TPath))
                {
                    FileInfo info = new FileInfo(TPath);
                    Contenttype = "";
                    _filename = thumb_prefix + TFileName;
                    ThumbStatus = CloudStorage.UploadFile(TPath, _filename, Videos.Configs.AwsSettings.bucket, Contenttype);
                    if (PublishedVideoStatus == "OK" || PublishedVideoStatus == "")
                    {
                        // File successfully uploaded, delete file from server
                        if (File.Exists(TPath))
                            File.Delete(TPath);
                    }
                    else
                    {
                        // send log to administarator along with video information to review error.
                        string log = ThumbStatus;
                        return "ThumbFailed";
                    }
                }
            }
            return "Success";
        }

        /// <summary>
        /// Delete all video files related for single record, published video, original video if exist, video thumbs
        /// </summary>
        public static void DeleteVideoFiles(string PublishedVideoFileName, string OriginalVideoFileName, string ThumbFileName)
        {
            // Delete Published Video
            CloudStorage.DeleteFile(PublishedVideoFileName, Videos.Configs.AwsSettings.bucket); // old ref: CloudSettings.VideoBucketName

            // Delete Original Video
            if (Videos.Configs.GeneralSettings.delete_original)
            {
                CloudStorage.DeleteFile(OriginalVideoFileName, Videos.Configs.AwsSettings.bucket);
            }

            // Delete Images
            string StartIndex = ThumbFileName.Remove(ThumbFileName.LastIndexOf("_"));
            int i = 0; // 15 thumb need to be renamed
            int counter = 1;
            for (i = 0; i <= 14; i++)
            {
                string TFileName = "";
                if (counter < 10)
                {
                    TFileName = StartIndex + "_00" + counter + ".jpg";
                }
                else
                {
                    TFileName = StartIndex + "_0" + counter + ".jpg";
                }

                CloudStorage.DeleteFile(TFileName, Videos.Configs.AwsSettings.bucket);
            }

        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

