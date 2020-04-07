using System.Threading.Tasks;
using Jugnoon.BLL;
using Jugnoon.Utility;
using Jugnoon.Models;
using Jugnoon.Framework;
using System.Collections.Generic;

namespace Jugnoon.Services
{
    public static class EmailSenderExtensions
    {

        /// <summary>
        /// Send email through contact us form
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Task ContactUsEmailAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, ContactUsViewModel entity)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                var lst = MailTemplateBLL.Get_Template(context, "CONTACTUS").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[fullname\\]", entity.SenderName);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[fullname\\]", entity.SenderName);
                    contents = MailProcess.Process2(contents, "\\[phone\\]", entity.PhoneNumber);
                    contents = MailProcess.Process2(contents, "\\[email\\]", entity.EmailAddress);
                    contents = MailProcess.Process2(contents, "\\[message\\]", entity.Body);

                    // attach signature
                    contents = MailProcess.Process2(contents, "\\[website\\]", Settings.Configs.GeneralSettings.website_title);
                    contents = MailProcess.Process2(contents, "\\[website_url\\]", SiteConfiguration.URL);

                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(email, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Send email reset autorization and processing link to user
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Task ChangeEmailResetAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, string username, string link)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                if (username.Contains("@"))
                    username = "User";
                var lst = MailTemplateBLL.Get_Template(context, "USREMLCREQ").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    contents = MailProcess.Process2(contents, "\\[key_url\\]", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(link));
                    contents = MailProcess.Process2(contents, "\\[emailaddress\\]", email);
                    contents = MailProcess.Process2(contents, "\\[domainname\\]", Jugnoon.Settings.Configs.GeneralSettings.website_title);
                    contents = MailProcess.Process2(contents, "\\[unsubscribeurl\\]", UrlConfig.UnsubscribeUrl);

                    // attach signature

                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(email, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Send notification to user when email reset event completed successfully
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Task ChangeEmailResetCompletedAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, string username)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                if (username.Contains("@"))
                    username = "User";
                var lst = MailTemplateBLL.Get_Template(context, "USREMLCHNG").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(email, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sending password reset and processling link to user email
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Task SendForgotPasswordResetAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, string username, string link)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                if (username.Contains("@"))
                    username = "User";
                var lst = MailTemplateBLL.Get_Template(context, "FORPASS").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    string url = "<a href=\"" + link + "\">" + link + "</a>";
                    contents = MailProcess.Process2(contents, "\\[key_url\\]", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(link));

                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(email, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Send notification to user when password reset event completed successfully
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="link"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, string username, string link, string password)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                var lst = MailTemplateBLL.Get_Template(context, "USRREG").Result;
                if (lst.Count > 0)
                {
                    if (username.Contains("@"))
                        username = "User";
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    contents = MailProcess.Process2(contents, "\\[email\\]", email);
                    contents = MailProcess.Process2(contents, "\\[password\\]", password);
                    string url = "<a href=\"" + link + "\">" + link + "</a>";
                    contents = MailProcess.Process2(contents, "\\[key_url\\]", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(link));

                    // attach signature
                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(email, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Send email notification to admin when new user signup
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Task SendEmailNotificationAsync(this IEmailSender emailSender, ApplicationDbContext context, string email, string username)
        {
            if (Settings.Configs.SmtpSettings.enable_email)
            {
                var lst = MailTemplateBLL.Get_Template(context, "USRREGADM").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    subject = MailProcess.Prepare_Email_Signature(subject);

                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    contents = MailProcess.Process2(contents, "\\[email\\]", email);

                    // attach signature
                    contents = MailProcess.Prepare_Email_Signature(contents);

                    return emailSender.SendEmailAsync(Jugnoon.Settings.Configs.GeneralSettings.admin_mail, subject, contents);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Sample method for sending email via Mandril using Mailchimp Email Templates
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="email"></param>
        /// <param name="activation_link"></param>
        /// <param name="contact_link"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static Task Mandril_Template(this IEmailSender emailSender, string email, string activation_link, string contact_link, string templateName)
        {
            var templateVariables = new Dictionary<string, dynamic>();
            // Mailchimp Email Template Variables
            templateVariables.Add("ACTIVATION_LINK", activation_link);
            templateVariables.Add("CONTACT_FORM", contact_link);
            // Email Subject
            var subject = "Account Activation.";
            // Send Email
            return emailSender.SendEmailAsync(email, "", "", subject, templateName, templateVariables);
        }
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
