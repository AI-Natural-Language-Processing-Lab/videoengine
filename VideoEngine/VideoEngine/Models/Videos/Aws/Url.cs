using Jugnoon.Framework;

namespace Jugnoon.Videos
{
    public class Url
    {
        /// <summary>
        ///  generate public / private video url
        /// </summary>
        public static VideoFileName prepareUrl(string username)
        {
            var obj = new VideoFileName()
            {
                publish_filename = "none",
                source_filename = "none",
                thumb_filename = "none"
            };
           
            if (Jugnoon.Settings.Configs.AwsSettings.enable)
            {
                var public_cdn = Configs.AwsSettings.public_url; 
                var private_cdn = Configs.AwsSettings.private_url;
                if (public_cdn != "" && private_cdn != "")
                {
                    var public_end_prefix = "/";
                    var private_end_prefix = "/";
                    if (public_cdn.EndsWith("/"))
                        public_end_prefix = "";
                    if (private_end_prefix.EndsWith("/"))
                        private_end_prefix = "";

                    if (public_cdn.StartsWith("http"))
                        obj.thumb_filename = public_cdn + public_end_prefix;
                    else
                        obj.thumb_filename = "https://" + public_cdn + public_end_prefix;

                    if (private_cdn.StartsWith("http"))
                    {
                        obj.publish_filename = private_cdn + private_end_prefix;
                        obj.source_filename = private_cdn + private_end_prefix;
                    }
                    else
                    {
                        obj.publish_filename = "https://" + private_cdn + private_end_prefix;
                        obj.source_filename = "https://" + private_cdn + private_end_prefix;
                    }

                    // publish path
                    if (Configs.AwsSettings.publish_directory_path != "")
                    {
                        obj.publish_filename = obj.publish_filename + Configs.AwsSettings.publish_directory_path;
                        if (!obj.publish_filename.EndsWith("/"))
                            obj.publish_filename = obj.publish_filename + "/";
                    }

                    // source path
                    if (Configs.AwsSettings.source_directory_path != "")
                    {
                        obj.source_filename = obj.source_filename + Configs.AwsSettings.source_directory_path;
                        if (!obj.source_filename.EndsWith("/"))
                            obj.source_filename = obj.source_filename + "/";
                    }

                    // thumb path
                    if (Configs.AwsSettings.thumbnail_directory_path != "")
                    {
                        obj.thumb_filename = obj.thumb_filename + Configs.AwsSettings.thumbnail_directory_path;
                        if (!obj.thumb_filename.EndsWith("/"))
                            obj.thumb_filename = obj.thumb_filename + "/";
                    }

                    // attach with user folder
                    obj.publish_filename = obj.publish_filename + "" + username + "/";
                    obj.source_filename = obj.source_filename + "" + username + "/";
                    obj.thumb_filename = obj.thumb_filename + "" + username + "/";

                }
            }

            return obj;
        }
    }
}

/* old reference code 
 *  */

/*var _publish_path = "none";
var _source_path = "";
var _thumb_path = "none";

if (Jugnoon.Settings.Configs.AwsSettings.enable)
{
    var public_cdn = Jugnoon.Videos.Configs.AwsSettings.public_url; // old ref CloudSettings.Streaming_Domain_Name;
    var private_cdn = Jugnoon.Videos.Configs.AwsSettings.private_url; 
    if (public_cdn != "" && private_cdn != "")
    {
        var public_end_prefix = "/";
        var private_end_prefix = "/";
        if (public_cdn.EndsWith("/"))
            public_end_prefix = "";
        if (private_end_prefix.EndsWith("/"))
            private_end_prefix = "";

        if (public_cdn.StartsWith("http"))
            _thumb_path = public_cdn + public_end_prefix;
        else
            _thumb_path = "https://" + public_cdn + public_end_prefix;

        if (private_cdn.StartsWith("http"))
        {
            _publish_path = private_cdn + private_end_prefix;
            _source_path = private_cdn + private_end_prefix;
        }
        else
        {
            _publish_path = "https://" + private_cdn + private_end_prefix;
            _source_path = "https://" + private_cdn + private_end_prefix;
        }

        // publish path
        if (Jugnoon.Videos.Configs.AwsSettings.publish_directory_path != "")
        {
            _publish_path = _publish_path + Jugnoon.Videos.Configs.AwsSettings.publish_directory_path;
            if (!_publish_path.EndsWith("/"))
                _publish_path = _publish_path + "/";
        }

        // source path
        if (Jugnoon.Videos.Configs.AwsSettings.source_directory_path != "")
        {
            _source_path = _publish_path + Jugnoon.Videos.Configs.AwsSettings.source_directory_path;
            if (!_source_path.EndsWith("/"))
                _source_path = _publish_path + "/";
        }

        // thumb path
        if (Jugnoon.Videos.Configs.AwsSettings.thumbnail_directory_path != "")
        {
            _thumb_path = _thumb_path + Jugnoon.Videos.Configs.AwsSettings.thumbnail_directory_path;
            if (!_thumb_path.EndsWith("/"))
                _thumb_path = _thumb_path + "/";
        }

        // attach with user folder
        _publish_path = _publish_path + "" + _rec.username;
        _source_path = _source_path + "" + _rec.username;
        _thumb_path = _thumb_path + "" + _rec.username;
    }
    /*else
    {
        string _pub_init_path = "https://s3.amazonaws.com/" + CloudSettings.VideoBucketName;

        // publish path
        if (CloudSettings.VideoPath != "")
        {
            _publish_path = _pub_init_path + CloudSettings.VideoPath;
            if (!_publish_path.EndsWith("/"))
                _publish_path = _publish_path + "/";
        }
        else
        {
            _publish_path = _pub_init_path;
        }

        // thumb path
        if (CloudSettings.VideoCoverPath != "")
        {
            _thumb_path = _pub_init_path + CloudSettings.VideoCoverPath;
            if (!_thumb_path.EndsWith("/"))
                _thumb_path = _thumb_path + "/";
        }
        else
        {
            _thumb_path = _pub_init_path;
        }

        // attach with user folder
        _publish_path = _publish_path + "" + _rec.username;
        //_source_path = _source_path + "" + _rec.username;
        _thumb_path = _thumb_path + "" + _rec.username;

    }

    //_preview_path = _publish_path + "/" + _preview_filename;
    _publish_path = _publish_path + "/" + _rec.videofilename;
    _source_path = _source_path + "/" + _rec.originalvideofilename;
    _thumb_path = _thumb_path + "/" + _rec.thumbfilename;

}*/



/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */

