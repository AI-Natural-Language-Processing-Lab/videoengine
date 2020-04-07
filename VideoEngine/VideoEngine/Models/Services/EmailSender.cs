using Jugnoon.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jugnoon.Services
{
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Routine email sending method (used for normal smtp + mandril + ses and others)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailProcess.Send_Mail(email, subject, message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Used specifically with Mandrill email with templates
        /// </summary>
        /// <param name="to_email"></param>
        /// <param name="from_email"></param>
        /// <param name="display_name"></param>
        /// <param name="subject"></param>
        /// <param name="templateName"></param>
        /// <param name="MergeVariables"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string to_email, string from_email, string display_name, string subject, string templateName, Dictionary<string, dynamic> MergeVariables)
        {
            var fromEmailDisplayName = display_name;
            if (from_email == "")
            {
                from_email = Settings.Configs.GeneralSettings.admin_mail;
                fromEmailDisplayName = Settings.Configs.GeneralSettings.admin_mail_name;
            }
            var toEmails = new List<string>();
            toEmails.Add(to_email);

            try
            {
                Mandrill.EmailProcess.SendMail(from_email, fromEmailDisplayName, toEmails, subject, templateName, MergeVariables);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.CompletedTask;
        }

    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
