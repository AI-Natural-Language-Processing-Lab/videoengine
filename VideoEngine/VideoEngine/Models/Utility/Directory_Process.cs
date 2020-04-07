using System.IO;
using Jugnoon.Settings;
namespace Jugnoon.Utility
{  
    public class Directory_Process
    {
        /// <summary>
        /// Generate required directories for saving user own media or other contents
        /// </summary>
        /// <param name="strPath"></param>
        public static void CreateRequiredDirectories(string strPath)
        {
            Directory.CreateDirectory(strPath);
            Directory.CreateDirectory(strPath + "default/");
            // create directory to store videos
            Directory.CreateDirectory(strPath + "flv/");
            // create directory to store media thumbs
            Directory.CreateDirectory(strPath + "thumbs/");

            // create directory to store profile and normal photos    
            Directory.CreateDirectory(strPath + "photos/");
            Directory.CreateDirectory(strPath + "photos/thumbs/");
            Directory.CreateDirectory(strPath + "photos/midthumbs/");
        }
    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
