
namespace Jugnoon.Settings
{
    public class Authentication
    {
        /// <summary>
        /// Toggle on | of facebook as additional authentication provider
        /// </summary>
        public bool enable_facebook { get; set; }

        /// <summary>
        /// If facebook enable, enter required Facebook App ID
        /// </summary>
        public string fb_appId { get; set; }

        /// <summary>
        /// If facebook enable, enter required Facebook App Secrete
        /// </summary>
        public string fb_appSecrete { get; set; }

        /// <summary>
        /// Toggle on | of twitter as additional authentication provider
        /// </summary>
        public bool enable_twitter { get; set; }

        /// <summary>
        /// If twitter enable, enter required twitter consumer key here
        /// </summary>
        public string tw_consumer_key { get; set; }

        /// <summary>
        /// If twitter enable, enter required twitter consumer secrete here
        /// </summary>
        public string tw_consumer_secrete { get; set; }

        /// <summary>
        /// Toggle on | of google as additional authentication provider
        /// </summary>
        public bool enable_google { get; set; }

        /// <summary>
        /// If google enable, enter required google client id
        /// </summary>
        public string google_clientid { get; set; }

        /// <summary>
        /// If google enable, enter required  google client secrete here
        /// </summary>
        public string google_clientsecrete { get; set; }

    }
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
