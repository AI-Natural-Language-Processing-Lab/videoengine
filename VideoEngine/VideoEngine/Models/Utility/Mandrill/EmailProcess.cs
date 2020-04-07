
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jugnoon.Mandrill
{
    public class EmailProcess
    {
        /// <summary>
        /// Sending Email through Mandrill API (Without Using Mandril Templates)
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toMails"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="async"></param>
        public static void SendMail(string fromEmail, string fromName, List<string> toMails, string subject, string htmlBody, bool async = false)
        {
            try
            {
                var api = new MandrillApi(Config.Key);

                var email = new EmailMessage()
                {
                    FromEmail = fromEmail,
                    FromName = fromName,
                    Subject = subject,
                    Html = htmlBody
                };
                var to = toMails.Select(mailTo => new EmailAddress(mailTo)).ToList();
                email.To = to;

                // Send email
                var smReq = new SendMessageRequest(email);
                var output = api.SendMessage(smReq).Result;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

        }

        /// <summary>
        /// Sending Email through Mandril API (Using Mandril Templates)
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toMails"></param>
        /// <param name="subject"></param>
        /// <param name="templateName"></param>
        /// <param name="MergeVariables"></param>
        /// <returns></returns>
        public static List<EmailResult> SendMail(string fromEmail, string fromName, List<string> toMails, string subject, string templateName, Dictionary<string, dynamic> MergeVariables)
        {
            try
            {
                var api = new MandrillApi(Config.Key);

                var email = new EmailMessage()
                {
                    FromEmail = fromEmail,
                    FromName = fromName,
                    Subject = subject,
                };
                email.Merge = true;
                email.MergeLanguage = "mailchimp";
                //email.Headers.Add("Reply-To", "workscene@mail.workscene.com");

                foreach (var item in MergeVariables)
                {
                    email.AddGlobalVariable(item.Key, item.Value);
                }


                var to = toMails.Select(mailTo => new EmailAddress(mailTo)).ToList();
                email.To = to;

                // Send email
                var smReq = new SendMessageTemplateRequest(email, templateName);
                var output = api.SendMessageTemplate(smReq).Result;
                return output;

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
