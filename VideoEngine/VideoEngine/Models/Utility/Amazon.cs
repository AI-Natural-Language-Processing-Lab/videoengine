using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;
using Jugnoon.Framework;
/// <summary>
/// Utility class responsible for processing AWS Media Storage
/// </summary>
namespace Jugnoon.Utility
{

    public class AmazonUtil
    {
        public static object LogBLL { get; private set; }

        public static string GetUrl(string BucketName, string folder, string fileName, int Expires = 10)
        {
            var client = GetS3Client();
            if (folder != "")
                fileName = folder + "/" + fileName;
            var preSignedURL = client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {

                BucketName = BucketName,
                Key = fileName,
                Expires = System.DateTime.Now.AddMinutes(Expires)
            });
            return preSignedURL.ToString();
        }

        public static void DeleteFileList(string BucketName, string Prefix, string Delimiter)
        {
            var fileList = GetFileList(BucketName, Prefix, Delimiter);
            foreach (var item in fileList)
            {
                DeleteFile(BucketName, item.Key);
            }
        }

        public static string DeleteFile(string BucketName, string fileName)
        {
            var client = GetS3Client();

            DeleteObjectRequest request = new DeleteObjectRequest()
            {
                BucketName = BucketName,
                Key = fileName
            };

            var response = client.DeleteObjectAsync(request).Result;

            return response.HttpStatusCode.ToString();
        }

        public static List<S3Object> GetFileList(string BucketName, string Prefix, string Delimiter = "/")
        {
            var list = new List<S3Object>();
            var client = GetS3Client();
            using (client)
            {
                try
                {
                    ListObjectsRequest Lor = new ListObjectsRequest()
                    {
                        BucketName = BucketName,
                        Prefix = Prefix,
                        Delimiter = Delimiter
                    };
                    ListObjectsResponse response1 = client.ListObjectsAsync(Lor).Result;
                    foreach (S3Object s3Object in response1.S3Objects)
                    {
                        list.Add(s3Object);
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public static MemoryStream GetFile(string bucketName, string folder, string fileName)
        {
            var client = GetS3Client();
            if (folder != "")
                fileName = folder + "/" + fileName;
            var file = new MemoryStream();
            using (client)
            {
                try
                {
                    GetObjectResponse r = client.GetObjectAsync(new GetObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = fileName
                    }).Result;
                    try
                    {
                        BufferedStream stream2 = new BufferedStream(r.ResponseStream);
                        byte[] buffer = new byte[0x2000];
                        int count = 0;
                        while ((count = stream2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            file.Write(buffer, 0, count);
                        }
                    }
                    finally
                    {
                    }

                }
                catch (AmazonS3Exception ex)
                {
                    throw ex;
                }
            }
            return file;
        }

        public static string UploadFile(string folder, string fileName, string BucketName, Stream sm)
        {
            var client = GetS3Client();
            if (folder != "")
                fileName = folder + "/" + fileName;
            var request = new PutObjectRequest()
            {
                BucketName = BucketName,
                Key = fileName,
                InputStream = sm
            };
            var output = client.PutObjectAsync(request).Result;
            return output.HttpStatusCode.ToString();
        }

        public static string uploadToS3(string folder, string fileName, string BucketName, string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var client = GetS3Client(); // SiteConfig.S3Client;
                    if (folder != "")
                        fileName = folder + "/" + fileName;
                    var request = new PutObjectRequest()
                    {
                        BucketName = BucketName,
                        Key = fileName,
                        FilePath = filePath,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    var output = client.PutObjectAsync(request).Result;

                    // remove local file
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    if (output.HttpStatusCode.ToString() == "")
                        return "OK";
                    else
                        return output.HttpStatusCode.ToString();
                }
                catch (AmazonS3Exception s3Exception)
                {
                    throw s3Exception;
                }

            }
            else
            {
                return "OK";
            }
        }

        public static async Task<string> uploadToS3Async(ApplicationDbContext context, string folder, string fileName, string BucketName, string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var client = GetS3Client();
                    if (folder != "")
                        fileName = folder + "/" + fileName;
                    var request = new PutObjectRequest()
                    {
                        BucketName = BucketName,
                        Key = fileName,
                        FilePath = filePath,
                        CannedACL = S3CannedACL.PublicRead
                    };
                    var output = await client.PutObjectAsync(request);

                    // remove local file
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    if (output.HttpStatusCode.ToString() == "")
                        return "OK";
                    else
                        return output.HttpStatusCode.ToString();
                }
                catch (AmazonS3Exception s3Exception)
                {
                    BLL.ErrorLgBLL.Add(context, "S3 Error", "", s3Exception.Message + " _ " + s3Exception.InnerException + " _ " + fileName + " _ " + filePath);
                    return "none";
                }

            }
            else
            {
                return "OK";
            }
        }

        public static AmazonS3Client GetS3Client()
        {
            var AccessKey = Settings.Configs.AwsSettings.accessKey;
            var Secretkey = Settings.Configs.AwsSettings.secretKey;
            Amazon.AWSConfigs.AWSRegion = Settings.Configs.AwsSettings.region;
            return new AmazonS3Client(AccessKey, Secretkey);
        }

        public static string CreateBucket(string BucketName)
        {
            var status = "";
            var client = GetS3Client();
            var response = client.ListBucketsAsync().Result;
            bool found = false;
            foreach (var bucket in response.Buckets)
            {
                if (bucket.BucketName == BucketName)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                var bucketRequest = new PutBucketRequest();
                bucketRequest.BucketName = BucketName;
                bucketRequest.BucketRegionName = S3Region.US;
                var output = client.PutBucketAsync(bucketRequest).Result;
                status = output.HttpStatusCode.ToString();
            }
            return status;
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
