using Jugnoon.BLL;
using Jugnoon.Settings;
using Jugnoon.Utility;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Jugnoon.Framework;
using Microsoft.AspNetCore.Identity;
using Jugnoon.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Jugnoon.Services;
using reCAPTCHA.AspNetCore;
using Jugnoon.Localize;

namespace VideoEngine.Controllers
{
    public class authController : Controller
    {
        ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly IRecaptchaService _recaptcha;

        // Dependencies Injections
        public authController(
            IRecaptchaService recaptcha,
           IOptions<SiteConfiguration> settings,
           SignInManager<ApplicationUser> signInManager,
           ApplicationDbContext context,
           UserManager<ApplicationUser> userManager,
           RoleManager<ApplicationRole> roleManager,
            IStringLocalizer<GeneralResource> generalLocalizer,
           IWebHostEnvironment environment,
           IHttpContextAccessor _httpContextAccessor,
           IEmailSender emailSender,
           IOptions<General> generalSettings,
           IOptions<Features> featureSettings,
           IOptions<Premium> premiumSettings,
           IOptions<Smtp> smtpSettings,
           IOptions<Registration> registerationSettings
           )
        {
            _recaptcha = recaptcha;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _environment = environment;
            // readable configuration
            Configs.GeneralSettings = generalSettings.Value;
            Configs.SmtpSettings = smtpSettings.Value;
            Configs.FeatureSettings = featureSettings.Value;
            Configs.PremiumSettings = premiumSettings.Value;
            Configs.RegistrationSettings = registerationSettings.Value;

            SiteConfig.Config = settings.Value;
            SiteConfig.userManager = userManager;
            SiteConfig.roleManager = roleManager;
            SiteConfig.generalLocalizer = generalLocalizer;
            SiteConfig.HttpContextAccessor = _httpContextAccessor;
        }

        [TempData]
        public string ErrorMessage { get; set; }
     

        #region Create Account
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["Page"] = "signup";
            if (Configs.RegistrationSettings.uniqueFieldOption == 1)
            {
                ModelState.Remove("UserName");
            }
            if (ModelState.IsValid)
            {
                // Rechapcha Validation
                if (Configs.RegistrationSettings.enableChapcha)
                {
                    var recaptcha = await _recaptcha.Validate(Request);
                    if (!recaptcha.success)
                    {
                        ModelState.AddModelError("Recaptcha", SiteConfig.generalLocalizer["_invalid_chapcha"].Value);
                        return View("~/Views/Home/index.cshtml", model);
                    }
                }


                if (Configs.RegistrationSettings.enablePrivacyCheck)
                {
                    if (!model.Agreement)
                    {
                        ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_accept_aggrement"].Value);
                        return View("~/Views/Home/index.cshtml", model);
                    }
                }

                var UserName = model.UserName;
                if (Configs.RegistrationSettings.uniqueFieldOption == 1)
                {
                    UserName = model.Email;
                }

                var user = new ApplicationUser
                {
                    UserName = UserName,
                    Email = model.Email,
                    created_at = DateTime.Now,
                    firstname = model.FirstName,
                    lastname = model.LastName,
                    isenabled = 1
                };
                var result = await SiteConfig.userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Init User Profile
                    await UserProfileBLL.InitializeUserProfile(_context, user);

                    // Create Required Directories
                    Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, user.Id.ToString()));

                    var code = await SiteConfig.userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                    await _emailSender.SendEmailConfirmationAsync(_context, model.Email, UserName, callbackUrl, model.Password);

                    await _emailSender.SendEmailNotificationAsync(_context, model.Email, UserName);

                    var redirect_url = "/activate";
                    if (returnUrl != null && returnUrl != "")
                        redirect_url = returnUrl;
                    return Redirect(redirect_url);
                }
                AddErrors(result);
            }

            return View("~/Views/Home/index.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            ViewData["Page"] = "confirm-email";
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await SiteConfig.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"UnabletoloaduserwithID '{userId}'.");
            }
            var result = await SiteConfig.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                ViewData["Page"] = "confirm-email";
            else
                ViewData["Page"] = "error";

            return View("~/Views/Home/index.cshtml");
        }
        #endregion
        
        #region Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Page"] = "signin";
            if (Configs.RegistrationSettings.enableChapcha)
            {
                var recaptcha = await _recaptcha.Validate(Request);
                if (!recaptcha.success)
                {
                    ModelState.AddModelError("Recaptcha", "Invalid Chapcha. Please try again!");
                    return View("~/Views/Home/index.cshtml", model);
                }
            }
            if (ModelState.IsValid)
            {
                // IP Address tracking and processing
                string ipaddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                if (BlockIPBLL.Validate(_context, ipaddress))
                {
                    model.Message = SiteConfig.generalLocalizer["_ip_blocked"].Value;
                    return View("~/Views/Home/index.cshtml", model);
                }

                ApplicationUser user;
                if (model.Email.Contains("@"))
                {
                    user = await SiteConfig.userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_invalid_login_attempt"].Value);
                        return View("~/Views/Home/index.cshtml", model);
                    }
                    else
                    {
                        if (user.isenabled == 0)
                        {
                            // user account is suspeneded
                            ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_account_suspended"].Value);
                            return View("~/Views/Home/index.cshtml", model);
                        }
                        model.Email = user.UserName;
                    }
                }
                else
                {
                    user = await SiteConfig.userManager.FindByNameAsync(model.Email);
                    if (user != null)
                    {
                        if (user.isenabled == 0)
                        {
                            // user account is suspeneded
                            ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_account_suspended"].Value);
                            return View("~/Views/Home/index.cshtml", model);
                        }
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Store IP Address Log 
                    if (Configs.GeneralSettings.store_ipaddress)
                        UserLogBLL.Add(_context, user.Id, SiteConfig.HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());


                    // Update Last Login Activity
                    UserBLL.Update_Field_Email(_context, model.Email, "last_login", DateTime.Now);

                    // Create User Directory for Media Storage
                    var dirPath = SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, model.Email.ToLower().ToString());
                    if (!System.IO.Directory.Exists(dirPath))
                    {
                        Directory_Process.CreateRequiredDirectories(dirPath);
                    }

                    // Subscription Enabled
                    if (!UserPackagesBLL.Check_Package_Feature()) // if packages enabled
                    {
                        if (Configs.PremiumSettings.premium_option == 1) // if membership subscription option is on
                        {
                            int membertype = UserBLL.Get_MemberType(_context, model.Email);
                            if (membertype == 2) // premium member
                            {
                                if (!UserBLL.Check_membership_Status(_context, model.Email))
                                {
                                    // user membership subscription expired.
                                    // i: update user status as normal
                                    UserBLL.Update_Field_Email(_context, model.Email, "type", (byte)0);
                                }
                            }
                        }
                    }

                    if (returnUrl == null || returnUrl == "")
                        returnUrl = "/account/";
                    return Redirect(returnUrl); // LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    ViewData["Page"] = "lockout";
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_invalid_login_attempt"].Value);
                    return View("~/Views/Home/index.cshtml", model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            ViewData["Page"] = "loginwith2fa";
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/index.cshtml", model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"UnabletoloaduserwithID '{SiteConfig.userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                //if (returnUrl == null)
                returnUrl = "/account/";
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                ViewData["Page"] = "lockout";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_invalid_authentication_code"].Value);
                return View("~/Views/Home/index.cshtml");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            ViewData["Page"] = "loginwithrecoverycode";
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/index.cshtml", model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unabletoloadtwo-factorauthenticationuser.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                if (returnUrl == null)
                    returnUrl = "/account/";

                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                ViewData["Page"] = "lockout";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, SiteConfig.generalLocalizer["_invalid_recovery_code_entered"].Value);
                return View("~/Views/Home/index.cshtml");
            }
        }

        #endregion

        #region External Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Errorfromexternalprovider: {remoteError}";
                return RedirectToAction(nameof(login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ViewData["Page"] = "signin";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                //if (returnUrl == null)
                returnUrl = "/account/";
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                ViewData["Page"] = "lockout";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["Page"] = "external-login";
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("~/Views/Home/index.cshtml", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            ViewData["Page"] = "external-login-confirmation";
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException(SiteConfig.generalLocalizer["_error_external_login_information"].Value);
                }
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    created_at = DateTime.Now,
                    isenabled = 1
                };
                var result = await SiteConfig.userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    // Init User Profile
                    await UserProfileBLL.InitializeUserProfile(_context, user);
                    // Create Required Directories
                    Directory_Process.CreateRequiredDirectories(SiteConfig.Environment.ContentRootPath + UtilityBLL.ParseUsername(SystemDirectoryPaths.UserDirectory, user.Id.ToString()));

                    result = await SiteConfig.userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Home/index.cshtml", model);
        }


        #endregion
        
        #region Forgot Password

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ViewData["Page"] = "forgot-password";
            if (ModelState.IsValid)
            {
                if (Configs.RegistrationSettings.enableChapcha)
                {
                    var recaptcha = await _recaptcha.Validate(Request);
                    if (!recaptcha.success)
                    {
                        ModelState.AddModelError("Recaptcha", SiteConfig.generalLocalizer["_invalid_chapcha"].Value);
                        return View("~/Views/Home/index.cshtml", model);
                    }
                }

                var user = await SiteConfig.userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await SiteConfig.userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ViewData["Page"] = "forgot-password";
                    return View("~/Views/Home/index.cshtml");
                }

                var code = await SiteConfig.userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.PasswordResetLink(user.Id, code, Request.Scheme);

                await _emailSender.SendForgotPasswordResetAsync(_context, user.Email, user.UserName, callbackUrl);

                ViewData["Page"] = "forgot-password-confirmation";
                return View("~/Views/Home/index.cshtml");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            ViewData["Page"] = "reset-password";
            if (code == null)
            {                
                throw new ApplicationException(SiteConfig.generalLocalizer["_invalid_code_supplied"].Value);
            }
            ViewData["Code"] = code;
            return View("~/Views/Home/index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ViewData["Page"] = "reset-password";
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/index.cshtml", model);
            }

            if (Configs.RegistrationSettings.enableChapcha)
            {
                var recaptcha = await _recaptcha.Validate(Request);
                if (!recaptcha.success)
                {
                    ModelState.AddModelError("Recaptcha", SiteConfig.generalLocalizer["_invalid_chapcha"].Value);
                    return View("~/Views/Home/index.cshtml", model);
                }
            }

            var user = await SiteConfig.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewData["Page"] = "forgot-password";
                return View("~/Views/Home/index.cshtml");
            }

            var result = await SiteConfig.userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                ViewData["Page"] = "reset-password-confirmation";
                return View("~/Views/Home/index.cshtml");
            }
            AddErrors(result);
            return View("~/Views/Home/index.cshtml");
        }
        #endregion

        #region Change Email
        [HttpGet]
        public IActionResult changeemail(string code = null, string user = null)
        {
            ViewData["Page"] = "change-email";
            if (code == null || user == null)
            {
                throw new ApplicationException(SiteConfig.generalLocalizer["_email_reset_code_error"].Value);
            }
            var model = new ResetEmailViewModel { Code = code, Id = user };
            ViewData["Code"] = code;
            ViewData["Id"] = user;
            return View("~/Views/Home/index.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult changeemail(ResetEmailViewModel model)
        {
            ViewData["Page"] = "change-email";
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/index.cshtml", model);
            }

            // validate id and key
            if (UserBLL.Validate_User_Key(_context, model.Id, model.Code))
            {
                // update email
                UserBLL.Update_Field_Id(_context, model.Id, "email", model.Email);

                var user = SiteConfig.userManager.FindByIdAsync(model.Id).Result;
                if (user != null)
                {
                    _emailSender.ChangeEmailResetCompletedAsync(_context, user.Email, user.UserName);
                }
            }
            else
            {
                throw new ApplicationException(SiteConfig.generalLocalizer["_email_reset_failed"].Value);
            }

            ViewData["Page"] = "signin";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
    }
}


/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
