using System;
using Jugnoon.Utility;
using Jugnoon.Entity;
using System.Collections.Generic;
using System.Linq;
using Jugnoon.Framework;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Jugnoon.Models;
using Jugnoon.Attributes;
/// <summary>
///  This class process and insert data at time of application initialization and after database creation in database for handling difference configurations and meta information.
/// </summary>
namespace Jugnoon.BLL
{
    public class DBDumpBLL
    {

        /// <summary>
        /// Predefined identity roles unless manually customized
        /// </summary>
        public static async Task InitRoles(RoleManager<ApplicationRole> rollManager)
        {
            string[] roles = { "Admin", "User", "Manager" };
            foreach ( var role in roles )
            {
                var roleExist = await rollManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await rollManager.CreateAsync(new ApplicationRole()
                    {
                        Name = role,
                        Description = ""
                    });
                }
            }
        }

        /// <summary>
        /// Predefined languages supported in application unless manually customized.
        /// </summary>
        /// <param name="context"></param>
        public static async Task InitLanguages(ApplicationDbContext context)
        {
            if (await LanguageBLLC.Count(context, new LanguageEntity() { }) == 0)
            {
                var Languages = new List<JGN_Languages>
                {
                     new JGN_Languages { culturename = "en-US", language = "English", region = "United State", isdefault = 1, isselected = 1 },
                     new JGN_Languages { culturename = "ar-SA", language = "Arabic", region = "Saudi Arabia", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "de-DE", language = "German", region = "Germany", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "es-ES", language = "Spanish", region = "Spain", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "fr-FR", language = "French", region = "France", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "it-IT", language = "Italian", region = "Italy", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "ja-JP", language = "Japanese", region = "Japan", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "pt-BR", language = "Portuguese", region = "Brazil", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "ru-RU", language = "Russian", region = "Russian Federation", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "tr-TR", language = "Turkish", region = "Turkey", isdefault = 0, isselected = 1 },
                     new JGN_Languages { culturename = "zh-CHS", language = "Chinese (simplified)", region = "China", isdefault = 0, isselected = 1 }
                };

                foreach (var Language in Languages)
                {
                    await LanguageBLLC.Process(context, Language);
                }
            }
        }

        /// <summary>
        /// Add predefined categories used for general purpose e.g as abuse reporting purpose unless customized manually. If you want to add more or modify existing categories please do that from category management part of control panel.
        /// </summary>
        /// <param name="context"></param>
        public static async Task InitializeCategories(ApplicationDbContext context)
        {
            if (await CategoryBLL.Count(context, new CategoryEntity() { type = (byte)CategoryBLL.Types.AbuseReport }) == 0)
            {
                var Categories = new List<JGN_Categories>
                { 
                     new JGN_Categories { title = "General", type = (byte)CategoryBLL.Types.Videos, priority = 100, isenabled = 1  },
                     new JGN_Categories { title = "Sexual Content", type = 4, priority = 100, isenabled = 1  },
                     new JGN_Categories { title = "Voilent or Repulsive Contents", type = 4, priority = 99, isenabled = 1  },
                     new JGN_Categories { title = "Hateful or Abusive Content", type = 4, priority = 98, isenabled = 1  },
                     new JGN_Categories { title = "Harmful Dangerous Acts", type = 4, priority = 96, isenabled = 1  },
                     new JGN_Categories { title = "Child Abuse", type = 4, priority = 95, isenabled = 1  },
                     new JGN_Categories { title = "Spam", type = 4, priority = 94, isenabled = 1  },
                     new JGN_Categories { title = "Infringes My Rights", type = 4, priority = 95, isenabled = 1  }
                };

                foreach (var Category in Categories)
                {
                    await CategoryBLL.Process(context, Category);
                }
            }
        }

        /// <summary>
        /// Predefined advertisement (Adsense) script slots used throughout the application unless manually control advertisement.
        /// </summary>
        /// <param name="_context"></param>
        public static async Task InitializeAdScript(ApplicationDbContext _context)
        {
            if (!await AdsBLL.Count_Script(_context))
            {
                int adult = 1;
                int nonadult = 0;
                await AdsBLL.Add_Script(_context, "Horizontal - 728x90", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Horizontal - 728x90", "no script", adult);
                await AdsBLL.Add_Script(_context, "Horizontal - 468x60", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Horizontal - 468x60", "no script", adult);
                await AdsBLL.Add_Script(_context, "Horizontal - 234x60", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Horizontal - 234x60", "no script", adult);
                await AdsBLL.Add_Script(_context, "Vertical - 120x600", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Vertical - 120x600", "no script", adult);
                await AdsBLL.Add_Script(_context, "Vertical - 160x600", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Vertical - 160x600", "no script", adult);
                await AdsBLL.Add_Script(_context, "Vertical - 120x240", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Vertical - 120x240", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 336x280", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 336x280", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 300x250", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 300x250", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 250x250", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 250x250", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 200x200", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 200x200", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 180x150", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 180x150", "no script", adult);
                await AdsBLL.Add_Script(_context, "Square - 125x125", "no script", nonadult);
                await AdsBLL.Add_Script(_context, "Square - 125x125", "no script", adult);
            }
        }
        /// <summary>
        /// Add pre-defined section and few default attributes for user dynamic profile, for more control visit control panel -> users -> settings section.
        /// </summary>
        /// <param name="context"></param>
        public static async Task InitializeUserProfileDynamicAttributes(ApplicationDbContext context)
        {
            // Add default section to manage all user profile dynamic attributes
            if (await AttrTemplatesSectionsBLL.Count(context, new AttrTemplateSectionEntity() { attr_type = Attr_Type.UserProfile }) == 0)
            {
                var Sections = new List<JGN_Attr_TemplateSections>
                {    new JGN_Attr_TemplateSections { title = "General", attr_type = (byte)Attr_Type.UserProfile, priority = 100, showsection = 0  },
                };


                foreach (var Section in Sections)
                {
                    var output = await AttrTemplatesSectionsBLL.Add(context, Section);

                    // Add few pre-defined user profile attributes. for more visit control panel -> users -> settings section
                    var Attributes = new List<JGN_Attr_Attributes>
                        {    new JGN_Attr_Attributes {
                            sectionid = output.id,
                            title = "About Me",
                            value = "",
                            attr_type = (byte)Attr_Type.UserProfile,
                            options = "",
                            element_type = 5, // rich text editor
                            isrequired = 0,
                            variable_type = 0,
                            icon = "0",
                            helpblock = "Enter your brief bio.",
                            min = 0,
                            max = 0,
                            priority = 100  },
                            new JGN_Attr_Attributes {
                            sectionid = output.id,
                            title = "Website",
                            value = "",
                            attr_type = (byte)Attr_Type.UserProfile,
                            options = "",
                            element_type = 0, // textbox
                            isrequired = 0,
                            variable_type = 0,
                            icon = "0",
                            helpblock = "Enter website",
                            min = 0,
                            max = 0,
                            priority = 99  },
                            new JGN_Attr_Attributes {
                            sectionid = output.id,
                            title = "City",
                            value = "City",
                            attr_type = (byte)Attr_Type.UserProfile,
                            options = "",
                            element_type = 0, // textbox
                            isrequired = 0,
                            variable_type = 0,
                            icon = "0",
                            helpblock = "Enter city",
                            min = 0,
                            max = 0,
                            priority = 98  }
                       };

                    foreach (var Attribute in Attributes)
                    {
                        await AttrAttributeBLL.Add(context, Attribute);
                    }
                }


            }
        }

        /// <summary>
        /// Add predefined mail templates used within application at time of initializing the application.
        /// </summary>
        /// <param name="context"></param>
        public static async Task InitializeMailTemplates(ApplicationDbContext context)
        {
            if (await MailTemplateBLL.Count(context, new MailTemplateEntity() { type = "-1" }) == 0)
            {
                var MailTemplates = new List<JGN_MailTemplates>
                {
                     new JGN_MailTemplates {
                         templatekey = "USRREG",
                         description = "Signup Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[password],[key_url],[website],[website_url],[email]",
                         subject = "Member Registration on [website]",
                         contents = "<p>Hello [username],<br /><br />thank you for creating an account on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>To activate your member account, please, click on the link below:</p>" +
                         "<p>[key_url]</p>" + 
                         "<p>Login: [email]</p>" +
                         "<p>Password: [password]</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRREGACT",
                         description = "Signup Activated Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[key_url],[website_url],",
                         subject = "Member Activation on [website]",
                         contents = "<p>Hello [username],<br /><br />thank you for creating an account on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>Your account has been activate and now you can login to access your account:</p>" +
                         "<p>[key_url]</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRCHEMAIL",
                         description = "Change Email Template",
                         type = "general",
                         subjecttags = "[username],[website]",
                         tags =  "[username],[key_url],[website]",
                         subject = "Change Email on [website],[website_url]",
                         contents = "<p>Hello [username],<br /><br />change your email on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>click on the link below:</p>" +
                         "<p>[key_url]</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "FORPASS",
                         description = "Forgot Password Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[url],[website],[website_url],[email],[password]",
                         subject = "Forgot Password on [website]",
                         contents = "<p>Hello [username],<br /><br />you requeted to retrieve your lost password or update password on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>To update or retrieve your password click on link below:</p>" +
                         "<p>[key_url]</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRPROFUPD",
                         description = "User Profile Updated Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[website_url]",
                         subject = "Forgot Password on [website]",
                         contents = "<p>Hello [username],<br /><br />As requested, your profile has been updated on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRPROFUPD",
                         description = "User Profile Updated Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[website_url]",
                         subject = "Forgot Password on [website]",
                         contents = "<p>Hello [username],<br /><br />As requested, your profile has been updated on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USREMLCREQ",
                         description = "Email Change Valition Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[key_url],[website],[website_url]",
                         subject = "Change Email Request on [website]",
                         contents = "<p>Hello [username],<br /><br />you requested to change your email address on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>To update or retrieve your email address click on link below:</p>" +
                         "<p>[key_url]</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USREMLCHNG",
                         description = "Email Changed Template",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[website_url]",
                         subject = "Email Changed on [website]",
                         contents = "<p>Hello [username],<br /><br />As requested, your email has been changed on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRREGADM",
                         description = "Admin Notification (new user registered)",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[website_url]",
                         subject = "New Registeration on [website]",
                         contents = "<p>Hello,<br /><br />[username] completed registration on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRCNTAPP",
                         description = "Notification (approve content)",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[website],[website_url]",
                         subject = "Your [contenttype] has been [status] on [website]",
                         contents = "<p>Hello [username],<br /><br />your posted [contenttype] has been [status] on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "USRAPRCNT",
                         description = "Notification (added content)",
                         type = "general",
                         subjecttags = "[username]",
                         tags =  "[username],[contenttype],[url],[website]",
                         subject = "[username] Added new [contenttype] on [website]",
                         contents = "<p>Hello,<br /><br />[username] added new [contenttype] on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         return_template_signature()
                     },
                     new JGN_MailTemplates {
                         templatekey = "CONTACTUS",
                         description = "Contact US (message)",
                         type = "general",
                         subjecttags = "[fullname]",
                         tags =  "[fullname],[phone],[email],[message]",
                         subject = "[fullname] sent Message on [website]",
                         contents = "<p>Hello,<br /><br />New message has been sent by [fullname] on <a href='[website_url]' title='[website]'>[website]</a>.</p>" +
                         "<p>User Information</p>" +
                         "<p>Name: [fullname]</p>" +
                         "<p>Phone: [phone]</p>" +
                         "<p>Email: [email]</p>" +
                         "<p>Message: [message]</p>" +
                         return_template_signature()
                     },

                };

                foreach (var Template in MailTemplates)
                {
                    await MailTemplateBLL.Add(context, Template);
                }
            }
        }

        private static string return_template_signature()
        {
            return "<p>Best Regards</p>" +
                "<p>[website] Team</p>" +
                "<p><a href='[website_url]' title='[website]'>[website]</a></p>";
        }

        /// <summary>
        /// Initialize Admin Role / Objects
        /// </summary>
        /// <param name="_context"></param>
        public static async Task InitializeAdminRoleObjects(ApplicationDbContext _context, ApplicationUser AdminUser)
        {
            if (await RoleBLL.Count(_context, new RoleEntity() { }) == 0)
            {
                short admin_role_id = 0;
                short guest_role_id = 0;
                var AdminRoles = new List<JGN_Roles>
                {
                     new JGN_Roles { rolename = "Guest"  },
                     new JGN_Roles { rolename = "Admin"  }
                };

                foreach (var Role in AdminRoles)
                {
                    var obj = await RoleBLL.Add(_context, Role);
                    if (Role.rolename == "Guest")
                        guest_role_id = obj.id;
                    else if (Role.rolename == "Admin")
                        admin_role_id = obj.id;
                }

                await InitializeObjects(_context, AdminUser, admin_role_id, guest_role_id);
            }
        }

        /// <summary>
        /// Initialize Admin Role / Objects
        /// </summary>
        /// <param name="_context"></param>
        public static async Task InitializeObjects(ApplicationDbContext _context, ApplicationUser AdminUser, short admin_role_id, short guest_role_id)
        {
            if (await RoleObjectBLL.Count(_context, new RoleObject() { }) == 0)
            {
                short full_admin_access_obj_id = 0;
                short readonly_admin_access_obj_id = 0;

                var AdminRoleOjbects = new List<JGN_RoleObjects>
                {
                     new JGN_RoleObjects { objectname = "AdminFullAccess", description = "Provide full access to all features available in control panel", uniqueid = "1521140258949" },
                     new JGN_RoleObjects { objectname = "AdminReadonlyAccess", description = "This object allows only readonly access to all features in control panel", uniqueid = "1521143471417" },
                     new JGN_RoleObjects { objectname = "UsersFullAccess", description = "This object allows full access to users section in control panel.", uniqueid = "1521143362403" },
                     new JGN_RoleObjects { objectname = "UsersReadOnlyAccess", description = "This object allows readonly access to users section in control panel.", uniqueid = "1521143407965" },
                     new JGN_RoleObjects { objectname = "VideoFullAccess", description = "Video section full access", uniqueid = "1521153486644" },
                     new JGN_RoleObjects { objectname = "VideosReadOnlyAccess", description = "Video section read only access.", uniqueid = "1521395130448" },
                     new JGN_RoleObjects { objectname = "PhotosFullAccess", description = "Photo section full access", uniqueid = "1521395185368" },
                     new JGN_RoleObjects { objectname = "PhotosReadOnlyAccess", description = "Photo section read only access.", uniqueid = "1521395801970" },
                     new JGN_RoleObjects { objectname = "ArtistsFullAccess", description = "Photo section full access", uniqueid = "1521395832536" },
                     new JGN_RoleObjects { objectname = "ArtistsReadonlyAccess", description = "Photo section read only access.", uniqueid = "1521395855358" },
                     new JGN_RoleObjects { objectname = "ForumsFullAccess", description = "Photo section full access", uniqueid = "1521395897976" },
                     new JGN_RoleObjects { objectname = "ForumsReadonlyAccess", description = "Photo section read only access.", uniqueid = "1521395939384" },
                     new JGN_RoleObjects { objectname = "QAFullAccess", description = "QA section full access", uniqueid = "1521395965196" },
                     new JGN_RoleObjects { objectname = "QAReadOnlyAccess", description = "QA section read only access.", uniqueid = "1521396022188" },
                     new JGN_RoleObjects { objectname = "PollsFullAccess", description = "QA section full access", uniqueid = "1521396059866" },
                     new JGN_RoleObjects { objectname = "PollsReadOnlyAccess", description = "QA section read only access.", uniqueid = "1521396089122" },
                     new JGN_RoleObjects { objectname = "BlogsFullAccess", description = "Blogs section full access", uniqueid = "1521396112858" },
                     new JGN_RoleObjects { objectname = "BlogsReadOnlyAccess", description = "Blogs section read only access.", uniqueid = "1521396141248" },
                     new JGN_RoleObjects { objectname = "WikiFullAccess", description = "Blogs section full access", uniqueid = "1521396214790" },
                     new JGN_RoleObjects { objectname = "WikiReadOnlyAccess", description = "Blogs section read only access.", uniqueid = "1521396235692" },
                     new JGN_RoleObjects { objectname = "SettingsFullAccess", description = "Blogs section full access", uniqueid = "1521396255768" },
                     new JGN_RoleObjects { objectname = "SettingsReadonlyAccess", description = "Blogs section read only access.", uniqueid = "1521396280060" },
                };

                foreach (var RoleObj in AdminRoleOjbects)
                {
                    var obj = await RoleObjectBLL.Add(_context, RoleObj);
                    if (RoleObj.uniqueid == "1521140258949")
                        full_admin_access_obj_id = obj.id;
                    else if (RoleObj.uniqueid == "1521143471417")
                        readonly_admin_access_obj_id = obj.id;
                }

                // associate appropriate object with proper role
                await RolePermission.Add(_context, new JGN_RolePermissions
                {
                    roleid = admin_role_id,
                    objectid = full_admin_access_obj_id
                });
                await RolePermission.Add(_context, new JGN_RolePermissions
                {
                    roleid = guest_role_id,
                    objectid = readonly_admin_access_obj_id
                });

                // Create Admin User
                AdminUser.created_at = DateTime.Now;
                AdminUser.isenabled = 1;
                await CreateUser(_context, AdminUser, AdminUser.password, "Admin", admin_role_id);
                // admin user
                /*await CreateUser(_context, new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    created_at = DateTime.Now,
                    isenabled = 1,
                    firstname = "Site",
                    lastname = "Admin"
                }, "Admin123$", "Admin", admin_role_id);*/
                // guest user
                /*await CreateUser(_context, new ApplicationUser
                {
                    UserName = "guest",
                    Email = "guest@example.com",
                    created_at = DateTime.Now,
                    isenabled = 1,
                    firstname = "Site",
                    lastname = "Guest"
                }, "Guest123$", "Admin", guest_role_id);*/
            }
        }

        /// <summary>
        /// Create Default Users
        /// </summary>
        /// <param name="_context"></param>
        public static async Task CreateUser(ApplicationDbContext _context, ApplicationUser user, string password, string roleName, short controlPanel_RoleID)
        {
            var admin_result = await SiteConfig.userManager.CreateAsync(user, password);
            if (admin_result.Succeeded)
            {
                await SiteConfig.userManager.AddToRoleAsync(user, roleName);

                // Init User Profile
                await UserProfileBLL.InitializeUserProfile(_context, user);

                Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, user.Id.ToString()));

                // enable account directly
                await UserBLL.Update_Field_IdAsync(_context, user.Id, "EmailConfirmed", true);

                // update controlpanel role
                await UserBLL.Update_Field_IdAsync(_context, user.Id, "roleid", controlPanel_RoleID);
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
