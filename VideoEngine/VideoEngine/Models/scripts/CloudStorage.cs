using Jugnoon.Framework;
using Jugnoon.Utility;
using System.Threading.Tasks;

namespace Jugnoon.Scripts
{
    public class CloudStorage
    {      

        public static string UploadFileV2(string path, string FileName, string BucketName, string ContentType)
        {
            return AmazonUtil.uploadToS3("", FileName, BucketName, path);
        }

        public static string UploadFile(string path, string FileName, string BucketName, string ContentType)
        {
            return AmazonUtil.uploadToS3("", FileName, BucketName, path);
        }

        public static async Task<string> UploadFileAsync(ApplicationDbContext context, string path, string FileName, string BucketName, string ContentType)
        {
            return await AmazonUtil.uploadToS3Async(context, "", FileName, BucketName, path);
        }

        public static string DeleteFile(string FileName, string BucketName)
        {
            return AmazonUtil.DeleteFile(BucketName, FileName);
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
