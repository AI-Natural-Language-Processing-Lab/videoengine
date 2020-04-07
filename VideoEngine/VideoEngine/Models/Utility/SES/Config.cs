
using Jugnoon.Utility;

namespace Jugnoon.SES
{
    public class Config
    {
        public static bool isEnabled
        {
            get
            {
                return Settings.Configs.SmtpSettings.enable_SES;
            }
        }

        public static string Host
        {
            get
            {
                return Settings.Configs.SmtpSettings.ses_host;
            }
        }

        public static string UserName
        {
            get
            {
                return Settings.Configs.SmtpSettings.ses_username;
            }
        }

        public static string Password
        {
            get
            {
                return Settings.Configs.SmtpSettings.ses_password;
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
