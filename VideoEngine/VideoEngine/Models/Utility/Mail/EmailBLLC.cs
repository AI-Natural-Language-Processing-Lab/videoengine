using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Collections.Generic;

namespace Jugnoon.Utility
{
    public class EmailBLLC
    {
        /// <summary>
        /// Sending email using asp.net core built-in smtp module
        /// </summary>
        /// <param name="from"></param>
        /// <param name="fromdisplayname"></param>
        /// <param name="recepient"></param>
        /// <param name="bcc"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendMailMessage(string from, string fromdisplayname, string recepient, string bcc, string cc, string subject, string body)
        {
            // check if mandrill email is enabled
            if (Mandrill.Config.isEnabled)
            {
                var emails = new List<string>();
                emails.Add(recepient);
                Mandrill.EmailProcess.SendMail(from, fromdisplayname, emails, subject, body);
            }
            else if (SES.Config.isEnabled)
            {
                SES.EmailProcess.SendMailMessage(from, fromdisplayname, recepient, bcc, cc, subject, body);
            }
            else
            {
                // normal smtp mail processing
                string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reLenient = new Regex(patternLenient);

                bool isLenientMatch = reLenient.IsMatch(recepient);

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();
                // Set the recepient address of the mail message
                mMailMessage.From = new MailAddress(from, fromdisplayname);
                // Set the recepient address of the mail message
                if (isLenientMatch)
                {
                    mMailMessage.To.Add(new MailAddress(recepient));
                    if (bcc != null)
                    {
                        isLenientMatch = reLenient.IsMatch(bcc);
                        if (isLenientMatch)
                            mMailMessage.To.Add(new MailAddress(bcc));
                    }
                    if (cc != null)
                    {
                        isLenientMatch = reLenient.IsMatch(cc);
                        if (isLenientMatch)
                            mMailMessage.To.Add(new MailAddress(cc));
                    }

                    mMailMessage.Subject = subject;

                    mMailMessage.Body = body;

                    mMailMessage.IsBodyHtml = true;

                    mMailMessage.Priority = MailPriority.Normal;

                    SmtpClient mSmtpClient = new SmtpClient();

                    mSmtpClient.Send(mMailMessage);
                }
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
