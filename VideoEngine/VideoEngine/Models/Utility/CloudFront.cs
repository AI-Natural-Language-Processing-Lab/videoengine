using System;
using System.IO;
using Amazon.CloudFront;
/// <summary>
///  Utility class responsible for generating AWS CloudFront Signed Urls
/// </summary>
namespace Jugnoon.Utility
{
    public class CloudFront
    {
        public static void SetupSignedCookie(string resourceUri)
        {
            var keyPairId = Videos.Configs.AwsSettings.cloudFront_keypair;
            var privateKeyFileName = Videos.Configs.AwsSettings.cloudFront_keyfilename;
            string pathtokey = SiteConfig.Environment.ContentRootPath + "/wwwroot/security/aws/cloudfront/" + privateKeyFileName;
            var privateKey = new FileInfo(pathtokey);
            string distribution = new Uri(resourceUri).Host.TrimEnd('/');

            var cookies = AmazonCloudFrontCookieSigner.GetCookiesForCannedPolicy(
                distribution,
                keyPairId,
                privateKey,
                DateTime.Today.AddYears(1));

            Helper.Cookie.WriteCookie(cookies.Expires.Key, cookies.Expires.Value);
            Helper.Cookie.WriteCookie(cookies.Signature.Key, cookies.Signature.Value);
            Helper.Cookie.WriteCookie(cookies.KeyPairId.Key, cookies.KeyPairId.Value);
        }

        public static string CreateCannedPrivateURL(string urlString, int timeout)
        {
            var keyPairId = Videos.Configs.AwsSettings.cloudFront_keypair;
            var privateKey = Videos.Configs.AwsSettings.cloudFront_keyfilename;
            return GetSignedURL(urlString, privateKey, keyPairId, timeout);
        }

        private static string GetSignedURL(string resourceUrl, string KeyFileName, string KEYPAIR_ID, int ExP)
        {
            string pathtokey = SiteConfig.Environment.ContentRootPath + "/wwwroot/security/aws/cloudfront/" + KeyFileName;
            FileInfo privateKey = new FileInfo(pathtokey);
            string file = new Uri(resourceUrl).PathAndQuery.Trim('/');
            string distribution = new Uri(resourceUrl).Host.TrimEnd('/');
            return AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                AmazonCloudFrontUrlSigner.Protocol.http,
                distribution,
                privateKey,
                file,
                KEYPAIR_ID,
                DateTime.Now.AddSeconds((double)ExP));
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
