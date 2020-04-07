namespace Jugnoon.Settings
{
    public class Smtp
    {
        /// <summary>
        /// Toggle on | off email functionality within website
        /// </summary>
        public bool enable_email { get; set; }

        /// <summary>
        /// Enable mandril as email sending option
        /// </summary>
        public bool enable_mandril { get; set; }

        /// <summary>
        /// If mandril option enable, mandril key will be required to continue
        /// </summary>
        public string mandril_key { get; set; }

        /// <summary>
        /// Enable AWS SES as core smtp option
        /// </summary>
        public bool enable_SES { get; set; }

        /// <summary>
        /// if SES enable, host will be needed.
        /// </summary>
        public string ses_host { get; set; }

        /// <summary>
        /// AWS SES - UserName
        /// </summary>
        public string ses_username { get; set; }

        /// <summary>
        /// AWS SES - Password
        /// </summary>
        public string ses_password { get; set; }
        
        /// <summary>
        /// General SMTP Option- Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// General SMTP Option- Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// General SMTP Option- FromAddress
        /// </summary>
        public string FromAddress { get; set; }

    }
}


/*
    * This file is subject to the terms and conditions defined in
    * file 'LICENSE.md', which is part of this source code package.
    * Copyright 2007 - 2020 MediaSoftPro
    * For more information email at support@mediasoftpro.com
 */
