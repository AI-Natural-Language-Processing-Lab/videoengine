
namespace Jugnoon.Mandrill
{
    public class Config
    {
        public static bool isEnabled
        {
            get
            {
                return Jugnoon.Settings.Configs.SmtpSettings.enable_mandril;
            }
        }

        public static string Key
        {
            get
            {
                return Jugnoon.Settings.Configs.SmtpSettings.mandril_key;
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
