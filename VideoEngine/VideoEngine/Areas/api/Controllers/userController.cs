using Jugnoon.BLL;
using Jugnoon.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using Jugnoon.Utility;
using Jugnoon.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Identity;
using Jugnoon.Models;
using Microsoft.Extensions.Localization;
using VideoEngine.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Jugnoon.Services;
using Newtonsoft.Json;
using Jugnoon.Attributes;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jugnoon.Localize;

namespace VideoEngine.Areas.api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {

        ApplicationDbContext _context;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
         public userController(
         IOptions<SiteConfiguration> settings,
         IMemoryCache memoryCache,
         ApplicationDbContext context,
         UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         SignInManager<ApplicationUser> signInManager,
          IStringLocalizer<GeneralResource> generalLocalizer,
         IWebHostEnvironment _environment,
         IHttpContextAccessor _httpContextAccessor,
         IEmailSender emailSender,
         IOptions<General> generalSettings,
         IOptions<Aws> awsSettings,
         IOptions<Media> mediaSettings,
         IOptions<Smtp> smtpSettings,
         IOptions<Features> featureSettings,
         IOptions<Registration> registerSettings
         )
        {
            // readable configuration
            Configs.GeneralSettings = generalSettings.Value;
            Configs.AwsSettings = awsSettings.Value;
            Configs.MediaSettings = mediaSettings.Value;
            Configs.SmtpSettings = smtpSettings.Value;
            Configs.FeatureSettings = featureSettings.Value;
            Configs.RegistrationSettings = registerSettings.Value;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
            SiteConfig.Config = settings.Value;
            SiteConfig.Cache = memoryCache;
           
            SiteConfig.userManager = userManager;
            SiteConfig.roleManager = roleManager;
             SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.Environment = _environment;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        // Load All Identity Roles
        [HttpGet("load_roles")]
        public ActionResult load_roles()
        {
            //  return Ok(new { posts = IdentityRoleBLL.Load() });
            return Ok(new { status = "error", message = "Not yet implemented" });
        }

        // Create Identity Role
        [HttpPost("create_role")]
        public async Task<ActionResult> create_role()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var role = JsonConvert.DeserializeObject<ApplicationRole>(json);
            var roleExist = await SiteConfig.roleManager.RoleExistsAsync(role.Name);
            if (!roleExist)
            {
                await SiteConfig.roleManager.CreateAsync(role);
            }
            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_created"].Value });
        }

        [HttpPost("load")]
        public async Task<ActionResult> load()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<MemberEntity>(json);
            var _posts = await UserBLL.LoadItems(_context, data);

            /* setup thumb path */
            foreach (var item in _posts)
            {
                item.img_url = UserUrlConfig.ProfilePhoto(item.UserName, item.picturename, 0); // default set
                item.url = UserUrlConfig.ProfileUrl(item, Configs.RegistrationSettings.uniqueFieldOption);
            }

            var _records = 0;
            if (data.id == "")
                _records = UserBLL.Count(_context, data);

            var _categories = new List<JGN_Categories>();
           
            return Ok(new { posts = _posts, categories = _categories, records = _records });
        }

        [HttpPost("load_reports")]
        public async Task<ActionResult> load_reports()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<MemberEntity>(json);
            var _reports = await UserBLL.LoadReport(_context, data);
            return Ok(new { data = _reports });
        }

        [HttpPost("getUserAuth")]
        public async Task<ActionResult> getUserAuth()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<MemberEntity>(json);

            data.nofilter = false;
            data.issummary = false;
            data.isdropdown = false;

            if (data.id == "")
            {
                return Ok(new { status = "error", message = "User Information Mising" });
            }

            // check user authorize
            //if (_signInManager.IsSignedIn(User))
            //{
                //var info = await SiteConfig.userManager.GetUserAsync(User);
                //if (info.Id == data.id)
                //{
                    var _posts = await UserProfileBLL.LoadItems(_context, data);
                    if (_posts.Count == 0)
                    {
                        return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
                    }
                    _posts[0].img_url = UserUrlConfig.ProfilePhoto(_posts[0].UserName, _posts[0].picturename, 0); // default set
                    _posts[0].url = UserUrlConfig.ProfileUrl(_posts[0], Configs.RegistrationSettings.uniqueFieldOption);
                    _posts[0].customize_register_date = UtilityBLL.CustomizeDate((DateTime)_posts[0].created_at, DateTime.Now);
                    if (_posts[0].last_login != null)
                    {
                        _posts[0].customize_last_login = UtilityBLL.CustomizeDate((DateTime)_posts[0].last_login, DateTime.Now);
                    }
                    var _roles = new List<JGN_Roles>();
                    if (data.isadmin)
                    if (_posts[0].roleid > 0)
                    {
                        _roles = await RoleBLL.LoadItems(_context, new RoleEntity()
                        {
                            id = _posts[0].roleid,
                        });
                        if (_roles.Count > 0)
                        {
                            foreach (var post in _posts)
                            {
                                _roles[0].permissions = await RolePermission.LoadItems(_context, new RoleDPermissionEntity()
                                {
                                    roleid = _roles[0].id,
                                    pagesize = 5000,
                                    pagenumber = 1
                                });
                            }
                        }
                    }

                    var authClaims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _posts[0].UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configs.GeneralSettings.jwt_private_key));


                    var token = new JwtSecurityToken(
                        issuer: SiteConfiguration.URL,
                        audience: SiteConfiguration.URL,
                        expires: DateTime.Now.AddHours(10),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new { 
                        status = "ok",
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        post = _posts[0],
                        role = _roles });
                //}
                //else
                //{
                //    return Ok(new { status = "error", message = "Bad Request" });
                //}
            //}
            //else
            //{
            //    return Ok(new { status = "error", message = "Authentication Failed" });
            // }
    
           
        }

        [HttpPost("getinfo")]
        public async Task<ActionResult> getinfo()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<MemberEntity>(json);

            if (data.id == "")
            {
                var _post = new ApplicationUser();
                _post.options = await AttrTemplatesSectionsBLL.LoadItems(_context, new AttrTemplateSectionEntity()
                {
                    templateid = 0, // if you want to manage multiple templates for dynamic attributes use it here
                    attr_type = Attr_Type.UserProfile,
                    order = "priority desc",
                    iscache = true
                });
                foreach (var option in _post.options)
                {
                    option.attributes = await AttrAttributeBLL.LoadItems(_context, new AttrAttributeEntity()
                    {
                        sectionid = option.id,
                        order = "priority desc",
                        iscache = true,
                        attr_type = (byte)Attr_Type.UserProfile
                    });
                }
                return Ok(new { status = "success", post = _post });
            }
            else
            {
                var _posts = await UserProfileBLL.LoadItems(_context, data);
                if (_posts.Count == 0)
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
                }

                // Raw Attributes
                _posts[0].options = await AttrTemplatesSectionsBLL.LoadItems(_context, new AttrTemplateSectionEntity()
                {
                    templateid = 0, // if you want to manage multiple templates for dynamic attributes use it here
                    order = "priority desc",
                    attr_type = Attr_Type.UserProfile,
                    iscache = true
                });
                foreach (var option in _posts[0].options)
                {
                    option.attributes = await AttrAttributeBLL.LoadItems(_context, new AttrAttributeEntity()
                    {
                        sectionid = option.id,
                        order = "priority desc",
                        iscache = true,
                        attr_type = (byte)Attr_Type.UserProfile
                    });
                }

                _posts[0].attr_values = await AttrValueBLL.LoadItems(_context, new AttrValueEntity()
                {
                    userid = _posts[0].Id,
                    attr_type = Attr_Type.UserProfile,
                    nofilter = false
                });

                _posts[0].img_url = UserUrlConfig.ProfilePhoto(_posts[0].UserName, _posts[0].picturename, 0); // default set
                _posts[0].url = UserUrlConfig.ProfileUrl(_posts[0], Configs.RegistrationSettings.uniqueFieldOption);
                _posts[0].customize_register_date = UtilityBLL.CustomizeDate((DateTime)_posts[0].created_at, DateTime.Now);
                if (_posts[0].last_login != null)
                {
                    _posts[0].customize_last_login = UtilityBLL.CustomizeDate((DateTime)_posts[0].last_login, DateTime.Now);
                }
                return Ok(new { status = "ok", post = _posts[0] });
            }
          
        }


        [HttpPost("userlog")]
        public async Task<ActionResult> userlog()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var entity = JsonConvert.DeserializeObject<UserIPEntity>(json);

            var _data = await UserLogBLL.LoadItems(_context, entity);

            return Ok(new { posts = _data });
        }

        [HttpPost("chpass")]
        public async Task<ActionResult> chpass()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var userInfo = JsonConvert.DeserializeObject<ApplicationUser>(json);

            if (userInfo.isadmin)
            {
                // Admin Version
                if (userInfo.Id != null && userInfo.Id != "")
                {
                    var user = await SiteConfig.userManager.FindByIdAsync(userInfo.Id);
                    if (user == null)
                    {
                        return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
                    }

                    var code = await SiteConfig.userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await SiteConfig.userManager.ResetPasswordAsync(user, code, userInfo.password);
                    if (result.Succeeded)
                    {
                        return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_updated"].Value });
                    }
                }

                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_error_occured"].Value });
            }
            else
            {
                if (userInfo.opassword == null)
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_error_occured"].Value });
                }

                // verify credential
                var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.opassword, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await SiteConfig.userManager.FindByIdAsync(userInfo.Id);

                    var code = await SiteConfig.userManager.GeneratePasswordResetTokenAsync(user);
                    var resetresult = await SiteConfig.userManager.ResetPasswordAsync(user, code, userInfo.password);
                    if (resetresult.Succeeded)
                    {
                        return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_updated"].Value });
                    }
                }

                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_record_failed"].Value });
            }
           
        }

        [HttpPost("cemail")]
        public async Task<ActionResult> cemail()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var userInfo = JsonConvert.DeserializeObject<ApplicationUser>(json);

            if (userInfo.isadmin)
            {
                // Admin Version
                if (userInfo.Id != null && userInfo.Id != "")
                {
                    if (userInfo.Email != "" && userInfo.Email.Contains("@"))
                    {
                        UserBLL.Update_Field_Id(_context, userInfo.Id, "email", userInfo.Email);
                        return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_updated"].Value });
                    }
                    else
                    {
                        return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_invalid_email"].Value });
                    }
                }
                else
                {
                    return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
                }
            }
            else
            {
                // My Account Version
                var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // generate own validation key
                    string val_key = Guid.NewGuid().ToString().Substring(0, 10);
                    // update user validation key
                    UserBLL.Update_Field_UserName(_context, userInfo.UserName, "val_key", val_key);

                    var URL = Config.GetUrl() + "login/changeemail?user=" + userInfo.Id + "&code=" + val_key;
                    await _emailSender.ChangeEmailResetAsync(_context, userInfo.Email, userInfo.UserName, URL);

                    return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_confirmation_mail_sent"].Value });
                }
                else
                {
                    return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_authentication_failed"].Value });
                }
            }
           
        }

        [HttpPost("archive")]
        public async Task<ActionResult> archive()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var userInfo = JsonConvert.DeserializeObject<ApplicationUser>(json);

            if (userInfo.Id != null && userInfo.Id != "")
            {
                UserBLL.Update_Field_Id(_context, userInfo.Id, "isenabled", (byte)3);

                await _signInManager.SignOutAsync();

                return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_deleted"].Value });
            }
            else
            {
                return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_missing_information"].Value });
            }
        }

        // Update Identity Role
        [HttpPost("ctype")]
        public async Task<ActionResult> ctype()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var model = JsonConvert.DeserializeObject<ApplicationUser>(json);
            
            if (model.Id != null && model.Id != "")
            {
                var user = await SiteConfig.userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // remove existing user role
                    var roles = await SiteConfig.userManager.GetRolesAsync(user);
                    await SiteConfig.userManager.RemoveFromRolesAsync(user, roles);
                    // add new role
                    await SiteConfig.userManager.AddToRoleAsync(user, model.role_name);
                }

                return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_updated"].Value });
            }
            else
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }
          
        }

        /// <summary>
        /// New Cropper Version via Angular App
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateavator")]
        public async Task<ActionResult> updateavator()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var model = JsonConvert.DeserializeObject<ApplicationUser>(json);
            if (model.Id == "")
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }

            byte[] image = Convert.FromBase64String(model.picturename.Replace("data:image/png;base64,", ""));
            string thumbFileName = model.Id.ToString() + ".png";

            // if cloud enabled
            try
            {
                var path = SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, model.Id.ToString());
                if (!Directory.Exists(path))
                    Directory_Process.CreateRequiredDirectories(path);

                var filepath = path + "/" + thumbFileName;
                if (System.IO.File.Exists(filepath))
                    System.IO.File.Delete(filepath);

                // local storage
                System.IO.File.WriteAllBytes(filepath, image);

                model.picturename = await Jugnoon.Helper.Aws.UploadPhoto(_context, thumbFileName, path, Configs.AwsSettings.user_photos_directory);
                // cleanup from local if cloud enabled and saved
                if (model.picturename.Contains("http"))
                {
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                ErrorLgBLL.Add(_context, "Error: User Picture Failed to Upload", "", ex.Message);
                model.picturename = "";
            }

            UserBLL.Update_Field_Id(_context, model.Id, "picturename", model.picturename);
            model.img_url = UserUrlConfig.ProfilePhoto(model.Id, model.picturename, 0);

            return Ok(new { status = "success", record = model, message = SiteConfig.generalLocalizer["_record_updated"].Value });

        }

        [HttpPost("updaterole")]
        public ActionResult updaterole()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var model = JsonConvert.DeserializeObject<ApplicationUser>(json);
            if (model.Id == "")
            {
                return Ok(new { status = "error", message = SiteConfig.generalLocalizer["_no_records"].Value });
            }
            if (model.roleid > 0)
              UserBLL.Update_Field_Id(_context, model.Id, "roleid", (short)model.roleid);
            
            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_record_updated"].Value });

        }

        /// <summary>
        /// Core api call responsible for creating new account and updating user profile from front end application or mobile app
        /// </summary>
        /// <returns></returns>
        [HttpPost("proc")]
        public async Task<ActionResult> proc()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<ApplicationUser>(json);
            
            if (data.Id != null && data.Id != "")
            {
                // Update Profile
                var record = await UserBLL.Update_User_Profile(_context, data, data.isadmin);
                
                /* attribute processing */
                foreach (var attr in data.attr_values)
                {
                    attr.userid = data.Id;
                    attr.attr_type = (byte)Attr_Type.UserProfile;
                    if (attr.id > 0)
                    {
                        /* update record */
                        await AttrValueBLL.Update(_context, attr);
                    }
                    else
                    {
                        /* add attribute */
                        await AttrValueBLL.Add(_context, attr);
                    }
                }

                record.img_url = UserUrlConfig.ProfilePhoto(record.UserName, record.picturename, 0);
                return Ok(new { status = "success", record = record, message = SiteConfig.generalLocalizer["_record_updated"].Value });
            }
            else
            {
                // Create New Account
                var user = new ApplicationUser
                {
                    UserName = data.UserName,
                    Email = data.Email,
                    created_at = DateTime.Now,
                    isenabled = 1, // internal use only (to suspend user account and all posted data at once)
                    firstname = data.firstname,
                    lastname = data.lastname
                };

                var result = await SiteConfig.userManager.CreateAsync(user, data.password);
                if (result.Succeeded)
                {
                    // role process
                    if (data.role_name != null && data.role_name != "")
                    {
                        var roleExist = await SiteConfig.roleManager.RoleExistsAsync(data.role_name);
                        if (!roleExist)
                        {
                            ApplicationRole role = new ApplicationRole();
                            role.Name = data.role_name;
                            role.CreatedDate = DateTime.Now;
                            await SiteConfig.roleManager.CreateAsync(role);
                        }

                        await SiteConfig.userManager.AddToRoleAsync(user, data.role_name);
                    }

                    // Init User Profile
                    await UserProfileBLL.InitializeUserProfile(_context, user);

                    Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, user.Id.ToString()));

                    // enable account directly
                    UserBLL.Update_Field_Id(_context, user.Id, "EmailConfirmed", true);

                    // setup url / picture url for app use only
                    data.Id = user.Id;
                    data.picturename = "none";
                    data.LockoutEnabled = false;
                    data.EmailConfirmed = true;
                    data.img_url = UserUrlConfig.ProfilePhoto(data.UserName, data.picturename, 0); // default set
                    data.url = UserUrlConfig.ProfileUrl(data, Configs.RegistrationSettings.uniqueFieldOption);

                    return Ok(new { status = "success", record = data, message = SiteConfig.generalLocalizer["_account_created"].Value });
                }
                else
                {
                    return Ok(new { status = "error", record = data, message = SiteConfig.generalLocalizer["_account_failed"].Value });
                }
            }
        }

        [HttpPost("action")]
        public async Task<ActionResult> action()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<MemberEntity>>(json);
         
            await UserBLL.ProcessAction(_context, data);

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("update_identity_role")]
        public async Task<ActionResult> update_identity_role()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<List<MemberEntity>>(json);

            await UserBLL.ProcessAction(_context, data);

            return Ok(new { status = "success", message = SiteConfig.generalLocalizer["_records_processed"].Value });
        }

        [HttpPost("dbusersetup")]
        public async Task<ActionResult> dbusersetup()
        {
            var json = new StreamReader(Request.Body).ReadToEnd();
            if (json == "")
            {
                return Ok(new { status = "Error", message = SiteConfig.generalLocalizer["_invalid_data"].Value });
            }
            var user = JsonConvert.DeserializeObject<ApplicationUser>(json);

            // Initialize Application Roles
            await DBDumpBLL.InitRoles(SiteConfig.roleManager);
            // Initialize Languages
            await DBDumpBLL.InitLanguages(_context);
            // Initialize Ad Script
            await DBDumpBLL.InitializeAdScript(_context);
            // Initialize Categories (General Abuse / Related)
            await DBDumpBLL.InitializeCategories(_context);
            // Initialize MailTemplates
            await DBDumpBLL.InitializeMailTemplates(_context);
            // Initialize Admin Control Panel Roles (Create Users)
            await DBDumpBLL.InitializeAdminRoleObjects(_context, user);
            // Initialize User Profile Dynamic Settings
            await DBDumpBLL.InitializeUserProfileDynamicAttributes(_context);
            return Ok(new
            {
                status = 200
            });
        }

        private void Emailo_MailTemplateProcess_EmlOptions(string username)
        {
            //if sending mail option enabled
            if (Jugnoon.Settings.Configs.SmtpSettings.enable_email)
            {
                string emailaddress = UserBLL.Return_Value_UserId(_context, username, "email");
                var lst = MailTemplateBLL.Get_Template(_context, "USREMLOPT").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    // attach signature
                    contents = MailProcess.Prepare_Email_Signature(contents);
                    MailProcess.Send_Mail(emailaddress, subject, contents);
                }
            }
        }

        private void ManageAccount_MailTemplateProcess(string username)
        {
            //if sending mail option enabled
            if (Jugnoon.Settings.Configs.SmtpSettings.enable_email)
            {
                string emailaddress = UserBLL.Return_Value_UserId(_context, username, "email");
                var lst = MailTemplateBLL.Get_Template(_context, "USRPASSCHN").Result;
                if (lst.Count > 0)
                {
                    string subject = MailProcess.Process2(lst[0].subject, "\\[username\\]", username);
                    string contents = MailProcess.Process2(lst[0].contents, "\\[username\\]", username);
                    // attach signature
                    contents = MailProcess.Prepare_Email_Signature(contents);
                    MailProcess.Send_Mail(emailaddress, subject, contents);
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


