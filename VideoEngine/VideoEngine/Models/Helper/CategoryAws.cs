using Jugnoon.Scripts;
using Jugnoon.Settings;
using System;
using System.Text;
using Jugnoon.Utility;
using Jugnoon.Framework;
using System.Threading.Tasks;

namespace Jugnoon.Helper
{
    public class Aws
    {
        public static async Task<string> UploadPhoto(ApplicationDbContext context, string files, string filePath, string aws_directory)
        {
            var str = new StringBuilder();

            if (Configs.AwsSettings.enable)
            {
                if (Configs.AwsSettings.bucket != "")
                {
                    var photos = files.Split(char.Parse(","));
                    foreach (var photo in photos)
                    {
                        if (str.ToString() != "")
                            str.Append(",");
                        if (!photo.StartsWith("http"))
                        {
                            str.Append(await _Process(context, photo, filePath, aws_directory));
                        }
                        else
                        {
                            str.Append(photo);
                        }
                    }
                }
            }
            else
            {
                str.Append(files);
            }
            return str.ToString();
        }

        private static async Task<string> _Process(ApplicationDbContext context, string filename, string path, string aws_directory)
        {
            string Org_Path = path + "" + filename;

            if (!aws_directory.EndsWith("/"))
                aws_directory = aws_directory + "/";

            filename = Guid.NewGuid().ToString().Substring(0, 8) + "-" + filename;

            string org_filename = filename;
           
            if (aws_directory != "")
            {
                if (aws_directory.EndsWith("/"))
                    org_filename = aws_directory + "" + org_filename;
                else
                    org_filename = aws_directory + "" + org_filename;
            }
            
            // add key to avoid duplications
            string contenttype = "image/jpeg";
            if (org_filename.EndsWith(".gif"))
                contenttype = "image/gif";
            else if (org_filename.EndsWith(".png"))
                contenttype = "image/png";

            string status = "";
            if (!System.IO.File.Exists(Org_Path))
            {
                return filename;
            }
            status = await CloudStorage.UploadFileAsync(context, Org_Path, org_filename, Configs.AwsSettings.bucket, contenttype);

            // delete file
            if (System.IO.File.Exists(Org_Path))
                System.IO.File.Delete(Org_Path);

            // prepare url
            string _publish_path = "";
            string public_url = Configs.AwsSettings.cdn_URL ;

            if (public_url != "")
            {
                if (public_url.StartsWith("http"))
                {
                    _publish_path = public_url;
                }
                else
                {
                    _publish_path = "https://" + public_url;

                }

                if (!_publish_path.EndsWith("/"))
                {
                    _publish_path = _publish_path + "/";

                }
                // publish path
                if (aws_directory != "")
                {
                    _publish_path = _publish_path + aws_directory;
                    if (!_publish_path.EndsWith("/"))
                        _publish_path = _publish_path + "/";
                }
            }
           
            if (_publish_path != "")
                filename = _publish_path + "" + filename;


            return filename;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
