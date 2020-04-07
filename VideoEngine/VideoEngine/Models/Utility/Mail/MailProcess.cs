using System;
using System.Text.RegularExpressions;
using Jugnoon.Settings;
/// <summary>
/// Utility class for processing mails.
/// </summary>
namespace Jugnoon.Utility
{
    public class MailProcess
    {
        public static string Process2(string text, string keyword, string value)
        {
            return Regex.Replace(text, keyword, value);
        }

        public static string Prepare_Email_Signature(string email_html)
        {
            email_html = Regex.Replace(email_html, "\\[website\\]", Configs.GeneralSettings.website_title);

            email_html = Regex.Replace(email_html, "\\[website_url\\]", SiteConfiguration.URL);

            email_html = Regex.Replace(email_html, "\\[UNSUBSCRIBEURL\\]", UrlConfig.UnsubscribeUrl);

            email_html = Regex.Replace(email_html, "\\[COMPANYLOGO\\]", SiteConfiguration.URL + Configs.MediaSettings.logo_path);

            return email_html;
        }


        public static void Send_Mail(string emailaddress, string subject, string content)
        {
            //// Sender Address
            string fromEmail = Configs.GeneralSettings.admin_mail;
            string fromEmailDisplayName = Configs.GeneralSettings.admin_mail_name;
            if (fromEmail == "")
                return;

            try
            {
                if (Configs.SmtpSettings.enable_email)
                {
                    EmailBLLC.SendMailMessage(fromEmail, fromEmailDisplayName, emailaddress, null, null, subject, content);
                }

            }
            catch (Exception ex)
            {
                throw ex;
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
